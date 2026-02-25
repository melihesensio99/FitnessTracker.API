using Application.DTO.Authentication;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
    }
}
