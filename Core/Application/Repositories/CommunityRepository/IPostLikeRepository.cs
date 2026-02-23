using Domain.Entities.Community;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repositories.CommunityRepository
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {
        Task<PostLike?> GetAsync(int postId, int userId);
        Task<int> CountAsync(int postId);
    }
}
