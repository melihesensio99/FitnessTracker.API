using Application.Abstraction.Services;
using Application.Abstraction.Storage;
using Application.DTO.Community.Post;
using Application.Abstraction.UnitOfWorks;
using Application.Repositories.CommunityRepository;
using AutoMapper;
using Domain.Entities.Community;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.Exceptions;

namespace Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public PostService(
            IPostRepository postRepository,
            IPostLikeRepository postLikeRepository,
            IUnitOfWork unitOfWork,
            IStorageService storageService,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task CreatePostAsync(CreatePostDto request, int userId)
        {
            var post = _mapper.Map<Post>(request);

            post.UserId = userId;
            post.CreatedAt = DateTime.UtcNow;
            post.Media = new List<PostMedia>();

            if (request.MediaFiles != null && request.MediaFiles.Any())
            {
                foreach (var file in request.MediaFiles)
                {
                    if (file.Length > 0)
                    {
                        using var stream = file.OpenReadStream();
                        string ext = Path.GetExtension(file.FileName);
                        string fileName = $"{Guid.NewGuid()}{ext}";

                        string fileUrl = await _storageService.UploadAsync(stream, fileName, file.ContentType);

                        post.Media.Add(new PostMedia
                        {
                            Url = fileUrl,
                            Type = DetermineMediaType(file.ContentType)
                        });
                    }
                }
            }

            await _postRepository.AddAsync(post);
            await _unitOfWork.SaveAsync();
        }

        private MediaType DetermineMediaType(string contentType)
        {
            if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            {
                return MediaType.Video;
            }
            return MediaType.Image;
        }

        public async Task<PagedResponse<ResultPostDto>> GetPostsAsync(PagedRequest request, int currentUserId, int? filterUserId = null)
        {
            PagedResponse<Post> postsResult;

            if (filterUserId.HasValue)
            {
                postsResult = await _postRepository.GetPostsByUserIdAsync(filterUserId.Value, request, false);
            }
            else
            {
                postsResult = await _postRepository.GetPosts(request, false);
            }

            return await MapPagedResponseAsync(postsResult, currentUserId);
        }

        public async Task DeletePostAsync(int postId, int userId)
        {
            var post = await _postRepository.GetPostByIdWithDetailsAsync(postId, tracking: true);
            if (post == null) throw new NotFoundException(nameof(Post), postId);

            if (post.UserId != userId) throw new UnauthorizedAccessException("Bu postu silme yetkiniz yok.");

            if (post.Media != null && post.Media.Any())
            {
                foreach (var media in post.Media)
                {
                    await _storageService.DeleteAsync(media.Url);
                }
            }

            _postRepository.Remove(post);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PagedResponse<ResultPostDto>> GetTrendingPostsAsync(PagedRequest request, int currentUserId, int days = 7)
        {
            var pagedEntities = await _postRepository.GetTrendingPostsAsync(request, days, tracking: false);
            return await MapPagedResponseAsync(pagedEntities, currentUserId);
        }

        public async Task<PagedResponse<ResultPostDto>> GetLikedPostsByUserIdAsync(int userId, int currentUserId, PagedRequest request)
        {
            var pagedEntities = await _postRepository.GetLikedPostsByUserIdAsync(userId, request, tracking: false);
            return await MapPagedResponseAsync(pagedEntities, currentUserId);
        }

        public async Task<PagedResponse<ResultPostDto>> SearchPostsAsync(string keyword, PagedRequest request, int currentUserId)
        {
            var pagedEntities = await _postRepository.SearchPostsAsync(keyword, request, tracking: false);
            return await MapPagedResponseAsync(pagedEntities, currentUserId);
        }

        public async Task UpdatePostVisibilityAsync(int postId, int userId, VisibilityType visibility)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null) throw new NotFoundException(nameof(Post), postId);

            if (post.UserId != userId) throw new UnauthorizedAccessException("Bu gönderinin görünürlüğünü değiştirme yetkiniz yok.");

            post.Visibility = visibility;

            _postRepository.Update(post);
            await _unitOfWork.SaveAsync();
        }

        private async Task<PagedResponse<ResultPostDto>> MapPagedResponseAsync(PagedResponse<Post> source, int currentUserId)
        {
            var dtos = _mapper.Map<List<ResultPostDto>>(source.Data);

            if (dtos.Any())
            {
                var postIds = dtos.Select(p => p.Id).ToList();
                var likedPostIds = await _postLikeRepository.GetLikedPostIdsByUserAsync(postIds, currentUserId);
                var likedPostIdSet = new HashSet<int>(likedPostIds);
                foreach (var dto in dtos)
                {
                    dto.IsLikedByCurrentUser = likedPostIdSet.Contains(dto.Id);
                }
            }

            return new PagedResponse<ResultPostDto>
            {
                Data = dtos,
                TotalCount = source.TotalCount,
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages
            };
        }

        public async Task<bool> ToggleLikeAsync(int postId, int userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null) throw new NotFoundException(nameof(Post), postId);

            var existingLike = await _postLikeRepository.GetPostLikeAsync(postId, userId);

            bool isLikedNow = false;
            if (existingLike != null)
            {
                _postLikeRepository.Remove(existingLike);
                post.LikeCount = Math.Max(0, post.LikeCount - 1);
                isLikedNow = false;
            }
            else
            {
                var like = new PostLike { PostId = postId, UserId = userId, CreatedAt = DateTime.UtcNow };
                await _postLikeRepository.AddAsync(like);
                post.LikeCount++;
                isLikedNow = true;
            }

            _postRepository.Update(post);
            await _unitOfWork.SaveAsync();

            return isLikedNow;
        }

        public async Task UpdatePostAsync(UpdatePostDto request, int userId)
        {
            var post = await _postRepository.GetPostByIdWithDetailsAsync(request.Id, tracking: true);
            if (post == null) throw new NotFoundException(nameof(Post), request.Id);

            if (post.UserId != userId) throw new UnauthorizedAccessException("Bu postu düzenleme yetkiniz yok.");

            post.Content = request.Content;
            post.Visibility = request.Visibility;
            post.UpdatedAt = DateTime.UtcNow;

            if (request.MediaFiles != null && request.MediaFiles.Any())
            {
                if (post.Media != null && post.Media.Any())
                {
                    foreach (var oldMedia in post.Media.ToList())
                    {
                        await _storageService.DeleteAsync(oldMedia.Url);
                        post.Media.Remove(oldMedia);
                    }
                }
                if (post.Media == null) post.Media = new List<PostMedia>();

                foreach (var file in request.MediaFiles)
                {
                    if (file.Length > 0)
                    {
                        using var stream = file.OpenReadStream();
                        string ext = Path.GetExtension(file.FileName);
                        string fileName = $"{Guid.NewGuid()}{ext}";
                        string fileUrl = await _storageService.UploadAsync(stream, fileName, file.ContentType);
                        post.Media.Add(new PostMedia
                        {
                            Url = fileUrl,
                            Type = DetermineMediaType(file.ContentType)
                        });
                    }
                }
            }

            _postRepository.Update(post);
            await _unitOfWork.SaveAsync();
        }
    }
}
