using Application.Common.Pagination;
using Application.DTO.Community.Comment;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface ICommentService
    {
        Task CreateCommentAsync(CreateCommentDto request, int userId);
        Task UpdateCommentAsync(UpdateCommentDto request, int userId);
        Task DeleteCommentAsync(int commentId, int userId);
        Task<PagedResponse<ResultCommentDto>> GetCommentsByPostIdAsync(int postId, PagedRequest request, int currentUserId);
        Task<bool> ToggleCommentLikeAsync(int commentId, int userId);
    }
}
