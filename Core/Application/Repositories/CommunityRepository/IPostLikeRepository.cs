using Domain.Entities.Community;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories.CommunityRepository
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {
        Task<PostLike?> GetPostLikeAsync(int postId, int userId);
        Task<List<int>> GetLikedPostIdsByUserAsync(IEnumerable<int> postIds, int userId);
    }
}
