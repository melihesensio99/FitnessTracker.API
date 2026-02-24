using Application.DTO.Community.Post;
using Domain.Entities.Community;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IPostService
    {
        Task CreatePostAsync(CreatePostDto request, int userId);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> GetPostsAsync(Application.Common.Pagination.PagedRequest request, int currentUserId, int? filterUserId = null);
        Task DeletePostAsync(int postId, int userId);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> GetTrendingPostsAsync(Application.Common.Pagination.PagedRequest request, int currentUserId, int days = 7);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> GetLikedPostsByUserIdAsync(int targetUserId, int currentUserId, Application.Common.Pagination.PagedRequest request);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> SearchPostsAsync(string keyword, Application.Common.Pagination.PagedRequest request, int currentUserId);
        Task UpdatePostVisibilityAsync(int postId, int userId, Domain.Enums.VisibilityType visibility);
        Task UpdatePostAsync(UpdatePostDto request, int userId);
        Task<bool> ToggleLikeAsync(int postId, int userId);
    }
}
