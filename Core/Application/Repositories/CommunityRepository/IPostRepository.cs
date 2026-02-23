using Application.Common.Pagination;
using Domain.Entities.Community;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.CommunityRepository
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<PagedResponse<Post>> GetPosts(PagedRequest request, bool tracking = false);
        Task<PagedResponse<Post>> GetPostsByUserIdAsync(int userId, PagedRequest request, bool tracking = false);
        Task<Post?> GetPostByIdWithDetailsAsync(int id, bool tracking = false);
        Task<PagedResponse<Post>> GetLikedPostsByUserIdAsync(int userId, PagedRequest request, bool tracking = false);
        Task<PagedResponse<Post>> SearchPostsAsync(string keyword, PagedRequest request, bool tracking = false);
        Task<PagedResponse<Post>> GetTrendingPostsAsync(PagedRequest request, int days = 7, bool tracking = false);
        Task<bool> IsPostOwnedByUserAsync(int postId, int userId);
    }
}
