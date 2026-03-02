using Application.Abstraction.Services;
using Application.Common;
using Application.Common.Pagination;
using Application.DTO.Community.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            var userId = GetCurrentUserId();
            await _commentService.CreateCommentAsync(createCommentDto, userId);
            return Ok(ApiResponse<object>.SuccessMessages("Yorum başarıyla eklendi."));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            var userId = GetCurrentUserId();
            updateCommentDto.Id = id;
            await _commentService.UpdateCommentAsync(updateCommentDto, userId);
            return Ok(ApiResponse<object>.SuccessMessages("Yorum başarıyla güncellendi."));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = GetCurrentUserId();
            await _commentService.DeleteCommentAsync(id, userId);
            return Ok(ApiResponse<object>.SuccessMessages("Yorum başarıyla silindi."));
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId, [FromQuery] PagedRequest request)
        {
            var currentUserId = GetCurrentUserIdOrDefault();
            var response = await _commentService.GetCommentsByPostIdAsync(postId, request, currentUserId);
            return Ok(ApiResponse<PagedResponse<ResultCommentDto>>.SuccessResponse(response));
        }

        [HttpPost("{id}/toggle-like")]
        [Authorize]
        public async Task<IActionResult> ToggleCommentLike(int id)
        {
            var userId = GetCurrentUserId();
            var isLiked = await _commentService.ToggleCommentLikeAsync(id, userId);
            var message = isLiked ? "Yorum beğenildi." : "Yorum beğenmekten vazgeçildi.";
            return Ok(ApiResponse<bool>.SuccessResponse(isLiked, message));
        }
    }
}
