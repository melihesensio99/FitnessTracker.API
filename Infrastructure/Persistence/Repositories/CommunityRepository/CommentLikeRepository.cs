using Application.Repositories.CommunityRepository;
using Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.CommunityRepository
{
    public class CommentLikeRepository : GenericRepository<CommentLike>, ICommentLikeRepository
    {
        private readonly FitnessTrackerDbContext _context;

        public CommentLikeRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CommentLike?> GetCommentLikeAsync(int commentId, int userId)
        {
            return await _context.CommentLikes
                .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);
        }

        public async Task<List<int>> GetLikedCommentIdsByUserAsync(IEnumerable<int> commentIds, int userId)
        {
            return await _context.CommentLikes
                .Where(l => l.UserId == userId && commentIds.Contains(l.CommentId))
                .Select(l => l.CommentId)
                .ToListAsync();
        }
    }
}
