using Application.DTO.Community.Post;
using Domain.Entities.Community;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IPostService
    {
        Task CreatePostAsync(CreatePostDto request, int userId);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> GetPostsAsync(Application.Common.Pagination.PagedRequest request, int? filterUserId = null);
        Task DeletePostAsync(int postId, int userId);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> GetTrendingPostsAsync(Application.Common.Pagination.PagedRequest request, int days = 7);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> GetLikedPostsByUserIdAsync(int userId, Application.Common.Pagination.PagedRequest request);
        Task<Application.Common.Pagination.PagedResponse<ResultPostDto>> SearchPostsAsync(string keyword, Application.Common.Pagination.PagedRequest request);
        Task UpdatePostVisibilityAsync(int postId, int userId, Domain.Enums.VisibilityType visibility);
    }
}
