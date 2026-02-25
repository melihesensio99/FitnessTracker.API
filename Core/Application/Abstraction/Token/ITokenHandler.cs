using Application.DTO.Authentication;
using Domain.Entities;

namespace Application.Abstraction.Token
{
    public interface ITokenHandler
    {
        TokenDto CreateAccessToken(User user);
    }
}
