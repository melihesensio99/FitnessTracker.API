using Application.Common.Pagination;
using Application.Repositories.CommunityRepository;
using Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Persistence.Context.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.CommunityRepository
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly FitnessTrackerDbContext _context;

        public CommentRepository(FitnessTrackerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResponse<Comment>> GetCommentsByPostIdAsync(int postId, PagedRequest request, bool tracking = false)
        {
            var query = _context.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip(request.Skip)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResponse<Comment>
            {
                Data = data,
                TotalCount = totalCount,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }

        public async Task<Comment?> GetCommentByIdWithDetailsAsync(int commentId, bool tracking = false)
        {
            var query = _context.Comments
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<List<Comment>> GetRepliesByCommentIdAsync(int commentId, bool tracking = false)
        {
            var query = _context.Comments
                .Where(c => c.ParentCommentId == commentId)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt)
                .AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }
    }
}
