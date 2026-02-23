using Application.Common.Pagination;
using Application.Repositories.CommunityRepository;
using Domain.Entities.Community;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using Persistence.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.CommunityRepository
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly FitnessTrackerDbContext _context;
        public PostRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        private IQueryable<Post> BaseQuery(bool tracking = false)
        {
            var query = _context.Posts.AsQueryable();
            if (!tracking) query = query.AsNoTracking();
            return query.Include(p => p.User).Include(p => p.Media);
        }

        public async Task<PagedResponse<Post>> GetLikedPostsByUserIdAsync(int userId, PagedRequest request, bool tracking = false)
        {
            return await BaseQuery(tracking)
                .Where(p => p.Likes.Any(l => l.UserId == userId))
                .OrderByDescending(p => p.CreatedAt)
                .ToPagedResponseAsync(request);
        }

        public async Task<Post?> GetPostByIdWithDetailsAsync(int id, bool tracking = false)
        {
            var query = _context.Posts.AsQueryable();
            if (!tracking) query = query.AsNoTracking();

            return await query
                .Include(p => p.User)
                .Include(p => p.Media)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResponse<Post>> GetPosts(PagedRequest request, bool tracking = false)
        {
            return await BaseQuery(tracking)
                .Where(x => x.Visibility == VisibilityType.Public)
                .OrderByDescending(x => x.CreatedAt)
                .ToPagedResponseAsync(request);
        }

        public async Task<PagedResponse<Post>> GetPostsByUserIdAsync(int userId, PagedRequest request, bool tracking = false)
        {
            return await BaseQuery(tracking)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToPagedResponseAsync(request);
        }

        public async Task<PagedResponse<Post>> GetTrendingPostsAsync(PagedRequest request, int days = 7, bool tracking = false)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);

            return await BaseQuery(tracking)
                .Where(p => p.Visibility == VisibilityType.Public && p.CreatedAt >= cutoffDate)
                .OrderByDescending(p => p.LikeCount + p.CommentCount)
                .ToPagedResponseAsync(request);
        }

        public async Task<bool> IsPostOwnedByUserAsync(int postId, int userId)
        {
            return await _context.Posts
                .AnyAsync(p => p.Id == postId && p.UserId == userId);
        }

        public async Task<PagedResponse<Post>> SearchPostsAsync(string keyword, PagedRequest request, bool tracking = false)
        {
            return await BaseQuery(tracking)
                .Where(p => p.Visibility == VisibilityType.Public && p.Content != null && p.Content.ToLower().Contains(keyword.ToLower()))
                .OrderByDescending(p => p.CreatedAt)
                .ToPagedResponseAsync(request);
        }
    }
}
