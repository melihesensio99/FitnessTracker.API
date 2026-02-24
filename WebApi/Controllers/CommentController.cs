using Application.Abstraction.Services;
using Application.Common;
using Application.Common.Pagination;
using Application.DTO.Community.Comment;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto, [FromQuery] int userId)
        {
            await _commentService.CreateCommentAsync(createCommentDto, userId);
            return Ok(ApiResponse<object>.SuccessMessage("Yorum başarıyla eklendi."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto updateCommentDto, [FromQuery] int userId)
        {
            updateCommentDto.Id = id;
            await _commentService.UpdateCommentAsync(updateCommentDto, userId);
            return Ok(ApiResponse<object>.SuccessMessage("Yorum başarıyla güncellendi."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id, [FromQuery] int userId)
        {
            await _commentService.DeleteCommentAsync(id, userId);
            return Ok(ApiResponse<object>.SuccessMessage("Yorum başarıyla silindi."));
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId, [FromQuery] PagedRequest request, [FromQuery] int currentUserId)
        {
            var response = await _commentService.GetCommentsByPostIdAsync(postId, request, currentUserId);
            return Ok(ApiResponse<PagedResponse<ResultCommentDto>>.SuccessResponse(response));
        }

        [HttpPost("{id}/toggle-like")]
        public async Task<IActionResult> ToggleCommentLike(int id, [FromQuery] int userId)
        {
            var isLiked = await _commentService.ToggleCommentLikeAsync(id, userId);
            var message = isLiked ? "Yorum beğenildi." : "Yorum beğenmekten vazgeçildi.";
            return Ok(ApiResponse<bool>.SuccessResponse(isLiked, message));
        }
    }
}
