using Application.Abstraction.Services;
using Application.Abstraction.Token;
using Application.DTO.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenHandler _tokenHandler;

        public AuthService(
            UserManager<User> userManager,
            ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            // 1) Kullanıcıyı bul (Kullanıcı adı veya e-posta ile)
            User? user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            // 2) Şifreyi doğrula (UserManager.CheckPasswordAsync kullanıyoruz, SignInManager'a gerek yok)
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordValid)
                throw new Exception("Kullanıcı adı veya şifre hatalı.");

            // 3) Token oluştur ve dön
            TokenDto tokenDto = _tokenHandler.CreateAccessToken(user);
            return tokenDto;
        }
    }
}
