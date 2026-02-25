using Application.Common.Pagination;
using Application.DTO.Community.Post;
using Domain.Entities.Community;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IPostService
    {
        Task CreatePostAsync(CreatePostDto request, int userId);
        Task<PagedResponse<ResultPostDto>> GetPostsAsync(PagedRequest request, int currentUserId, int? filterUserId = null);
        Task DeletePostAsync(int postId, int userId);
        Task<PagedResponse<ResultPostDto>> GetTrendingPostsAsync(PagedRequest request, int currentUserId, int days = 7);
        Task<PagedResponse<ResultPostDto>> GetLikedPostsByUserIdAsync(int targetUserId, int currentUserId, PagedRequest request);
        Task<PagedResponse<ResultPostDto>> SearchPostsAsync(string keyword, PagedRequest request, int currentUserId);
        Task UpdatePostVisibilityAsync(int postId, int userId, Domain.Enums.VisibilityType visibility);
        Task UpdatePostAsync(UpdatePostDto request, int userId);
        Task<bool> ToggleLikeAsync(int postId, int userId);
    }
}
