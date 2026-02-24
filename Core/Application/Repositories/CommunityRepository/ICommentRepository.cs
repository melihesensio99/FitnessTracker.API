using Application.Common.Pagination;
using Domain.Entities.Community;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories.CommunityRepository
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<PagedResponse<Comment>> GetCommentsByPostIdAsync(int postId, PagedRequest request, bool tracking = false);
        Task<Comment?> GetCommentByIdWithDetailsAsync(int commentId, bool tracking = false);
        Task<List<Comment>> GetRepliesByCommentIdAsync(int commentId, bool tracking = false);
    }
}
