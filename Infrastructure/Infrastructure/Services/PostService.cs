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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public PostService(
            IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            IStorageService storageService,
            IMapper mapper)
        {
            _postRepository = postRepository;
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

        public async Task<PagedResponse<ResultPostDto>> GetPostsAsync(PagedRequest request, int? filterUserId = null)
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

            return MapPagedResponse(postsResult);
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

        public async Task<PagedResponse<ResultPostDto>> GetTrendingPostsAsync(PagedRequest request, int days = 7)
        {
            var pagedEntities = await _postRepository.GetTrendingPostsAsync(request, days, tracking: false);
            return MapPagedResponse(pagedEntities);
        }

        public async Task<PagedResponse<ResultPostDto>> GetLikedPostsByUserIdAsync(int userId, PagedRequest request)
        {
            var pagedEntities = await _postRepository.GetLikedPostsByUserIdAsync(userId, request, tracking: false);
            return MapPagedResponse(pagedEntities);
        }

        public async Task<PagedResponse<ResultPostDto>> SearchPostsAsync(string keyword, PagedRequest request)
        {
            var pagedEntities = await _postRepository.SearchPostsAsync(keyword, request, tracking: false);
            return MapPagedResponse(pagedEntities);
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

        private PagedResponse<ResultPostDto> MapPagedResponse(PagedResponse<Post> source)
        {
            return new PagedResponse<ResultPostDto>
            {
                Data = _mapper.Map<List<ResultPostDto>>(source.Data),
                TotalCount = source.TotalCount,
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages
            };
        }
    }
}
