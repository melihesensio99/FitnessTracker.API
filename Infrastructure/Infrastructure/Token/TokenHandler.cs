using Application.Abstraction.Token;
using Application.DTO.Authentication;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDto CreateAccessToken(User user)
        {
            var tokenDto = new TokenDto();

            // 1) Simetrik güvenlik anahtarı
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // 2) İmzalama bilgileri
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            // 3) Token süresi (15 dakika)
            tokenDto.Expiration = DateTime.UtcNow.AddMinutes(15);

            // 4) Claim'ler — Token içine gömülecek kullanıcı bilgileri
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.Name ?? "")
            };

            // 5) JWT Token oluştur
            var token = new JwtSecurityToken(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: tokenDto.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims
            );

            // 6) Token'ı string'e çevir
            tokenDto.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenDto;
        }
    }
}
