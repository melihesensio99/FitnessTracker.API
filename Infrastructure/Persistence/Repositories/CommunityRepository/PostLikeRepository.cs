using Application.Repositories.CommunityRepository;
using Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.CommunityRepository
{
    public class PostLikeRepository : GenericRepository<PostLike>, IPostLikeRepository
    {
        private readonly FitnessTrackerDbContext _context;

        public PostLikeRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<int>> GetLikedPostIdsByUserAsync(IEnumerable<int> postIds, int userId)
        {
            return await _context.PostLikes
                .Where(l => l.UserId == userId && postIds.Contains(l.PostId))
                .Select(l => l.PostId)
                .ToListAsync();
        }

        public async Task<PostLike?> GetPostLikeAsync(int postId, int userId)
        {
            return await _context.PostLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
        }
    }
}
