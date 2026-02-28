using Application.Abstraction.Services;
using Application.Abstraction.Token;
using Application.DTO.Authentication;
using Application.Events;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IEventBus _eventBus;

        public AuthService(
            UserManager<User> userManager,
            ITokenHandler tokenHandler,
            IEventBus eventBus)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _eventBus = eventBus;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
          
            User? user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

   
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordValid)
                throw new Exception("Kullanıcı adı veya şifre hatalı.");


            TokenDto tokenDto = _tokenHandler.CreateAccessToken(user);
            return tokenDto;
        }

        public async Task ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
           
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
       
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                throw new Exception("Doğrulama başarısız veya link süresi dolmuş.");

            await _eventBus.PublishAsync(new EmailVerifiedEvent
            {
                UserId = user.Id.ToString(),
                Email = user.Email!,
                Name = user.Name
            });
        }
    }
}
