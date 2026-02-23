using Application.Abstraction.Services;
using Application.Common;
using Application.Common.Pagination;
using Application.DTO.Community.Post;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto createPostDto, [FromQuery] int userId)
        {
            await _postService.CreatePostAsync(createPostDto, userId);
            return Ok(ApiResponse<object>.SuccessMessage("Post başarıyla oluşturuldu."));
        }

        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed([FromQuery] PagedRequest request)
        {
            var response = await _postService.GetPostsAsync(request);
            return Ok(ApiResponse<PagedResponse<ResultPostDto>>.SuccessResponse(response));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPosts(int userId, [FromQuery] PagedRequest request)
        {
            var response = await _postService.GetPostsAsync(request, filterUserId: userId);
            return Ok(ApiResponse<PagedResponse<ResultPostDto>>.SuccessResponse(response));
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrending([FromQuery] PagedRequest request, [FromQuery] int days = 7)
        {
            var response = await _postService.GetTrendingPostsAsync(request, days);
            return Ok(ApiResponse<PagedResponse<ResultPostDto>>.SuccessResponse(response));
        }

        [HttpGet("liked/{userId}")]
        public async Task<IActionResult> GetLikedPosts(int userId, [FromQuery] PagedRequest request)
        {
            var response = await _postService.GetLikedPostsByUserIdAsync(userId, request);
            return Ok(ApiResponse<PagedResponse<ResultPostDto>>.SuccessResponse(response));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string keyword, [FromQuery] PagedRequest request)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Arama kelimesi boş olamaz."));
            }

            var response = await _postService.SearchPostsAsync(keyword, request);
            return Ok(ApiResponse<PagedResponse<ResultPostDto>>.SuccessResponse(response));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id, [FromQuery] int userId)
        {
            await _postService.DeletePostAsync(id, userId);
            return Ok(ApiResponse<object>.SuccessMessage("Post başarıyla silindi."));
        }

        [HttpPut("{id}/visibility")]
        public async Task<IActionResult> UpdatePostVisibility(int id, [FromQuery] int userId, [FromQuery] Domain.Enums.VisibilityType visibility)
        {
            await _postService.UpdatePostVisibilityAsync(id, userId, visibility);
            return Ok(ApiResponse<object>.SuccessMessage("Görünürlük başarıyla güncellendi."));
        }
    }
}
