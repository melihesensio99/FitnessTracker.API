using Domain.Entities.Community;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories.CommunityRepository
{
    public interface ICommentLikeRepository : IGenericRepository<CommentLike>
    {
        Task<CommentLike?> GetCommentLikeAsync(int commentId, int userId);
        Task<List<int>> GetLikedCommentIdsByUserAsync(IEnumerable<int> commentIds, int userId);
    }
}
