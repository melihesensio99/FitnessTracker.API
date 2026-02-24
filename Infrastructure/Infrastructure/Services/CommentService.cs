using Application.Abstraction.Services;
using Application.Abstraction.UnitOfWorks;
using Application.Common.Pagination;
using Application.DTO.Community.Comment;
using Application.Exceptions;
using Application.Repositories.CommunityRepository;
using AutoMapper;
using Domain.Entities.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentLikeRepository _commentLikeRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            ICommentLikeRepository commentLikeRepository,
            IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _commentLikeRepository = commentLikeRepository;
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateCommentAsync(CreateCommentDto request, int userId)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null) throw new NotFoundException(nameof(Post), request.PostId);

            var comment = new Comment
            {
                PostId = request.PostId,
                UserId = userId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                ParentCommentId = request.ParentCommentId
            };

            // If this is a reply, validate parent comment exists and belongs to the same post
            if (request.ParentCommentId.HasValue)
            {
                var parentComment = await _commentRepository.GetByIdAsync(request.ParentCommentId.Value);
                if (parentComment == null)
                    throw new NotFoundException(nameof(Comment), request.ParentCommentId.Value);

                if (parentComment.PostId != request.PostId)
                    throw new InvalidOperationException("Yanıt yapılan yorum bu posta ait değil.");

                // Increment reply count on parent comment
                parentComment.ReplyCount++;
                _commentRepository.Update(parentComment);
            }

            // Increment comment count on the post
            post.CommentCount++;
            _postRepository.Update(post);

            await _commentRepository.AddAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCommentAsync(UpdateCommentDto request, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(request.Id);
            if (comment == null) throw new NotFoundException(nameof(Comment), request.Id);

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("Bu yorumu düzenleme yetkiniz yok.");

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            _commentRepository.Update(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetCommentByIdWithDetailsAsync(commentId, tracking: true);
            if (comment == null) throw new NotFoundException(nameof(Comment), commentId);

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("Bu yorumu silme yetkiniz yok.");

            var post = await _postRepository.GetByIdAsync(comment.PostId);
            if (post != null)
            {
                // Count the comment itself + all its replies
                int totalDeleted = 1 + (comment.Replies?.Count ?? 0);
                post.CommentCount = Math.Max(0, post.CommentCount - totalDeleted);
                _postRepository.Update(post);
            }

            // If this is a reply, decrement parent's reply count
            if (comment.ParentCommentId.HasValue)
            {
                var parentComment = await _commentRepository.GetByIdAsync(comment.ParentCommentId.Value);
                if (parentComment != null)
                {
                    parentComment.ReplyCount = Math.Max(0, parentComment.ReplyCount - 1);
                    _commentRepository.Update(parentComment);
                }
            }

            // Cascade delete will handle removing child replies
            _commentRepository.Remove(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PagedResponse<ResultCommentDto>> GetCommentsByPostIdAsync(int postId, PagedRequest request, int currentUserId)
        {
            var pagedComments = await _commentRepository.GetCommentsByPostIdAsync(postId, request, tracking: false);

            var dtos = new List<ResultCommentDto>();

            // Collect all comment IDs (parents + replies) for batch like lookup
            var allCommentIds = new List<int>();
            foreach (var comment in pagedComments.Data)
            {
                allCommentIds.Add(comment.Id);
                if (comment.Replies != null)
                {
                    allCommentIds.AddRange(comment.Replies.Select(r => r.Id));
                }
            }

            var likedCommentIds = new HashSet<int>();
            if (allCommentIds.Any())
            {
                var likedIds = await _commentLikeRepository.GetLikedCommentIdsByUserAsync(allCommentIds, currentUserId);
                likedCommentIds = new HashSet<int>(likedIds);
            }

            foreach (var comment in pagedComments.Data)
            {
                var dto = MapCommentToDto(comment, likedCommentIds);
                dtos.Add(dto);
            }

            return new PagedResponse<ResultCommentDto>
            {
                Data = dtos,
                TotalCount = pagedComments.TotalCount,
                CurrentPage = pagedComments.CurrentPage,
                PageSize = pagedComments.PageSize,
                TotalPages = pagedComments.TotalPages
            };
        }

        public async Task<bool> ToggleCommentLikeAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null) throw new NotFoundException(nameof(Comment), commentId);

            var existingLike = await _commentLikeRepository.GetCommentLikeAsync(commentId, userId);

            bool isLikedNow;
            if (existingLike != null)
            {
                _commentLikeRepository.Remove(existingLike);
                comment.LikeCount = Math.Max(0, comment.LikeCount - 1);
                isLikedNow = false;
            }
            else
            {
                var like = new CommentLike
                {
                    CommentId = commentId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                await _commentLikeRepository.AddAsync(like);
                comment.LikeCount++;
                isLikedNow = true;
            }

            _commentRepository.Update(comment);
            await _unitOfWork.SaveAsync();

            return isLikedNow;
        }

        private ResultCommentDto MapCommentToDto(Comment comment, HashSet<int> likedCommentIds)
        {
            var dto = new ResultCommentDto
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                UserName = comment.User?.Name,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                ParentCommentId = comment.ParentCommentId,
                LikeCount = comment.LikeCount,
                ReplyCount = comment.ReplyCount,
                IsLikedByCurrentUser = likedCommentIds.Contains(comment.Id),
                Replies = new List<ResultCommentDto>()
            };

            if (comment.Replies != null && comment.Replies.Any())
            {
                foreach (var reply in comment.Replies.OrderBy(r => r.CreatedAt))
                {
                    dto.Replies.Add(MapCommentToDto(reply, likedCommentIds));
                }
            }

            return dto;
        }
    }
}
