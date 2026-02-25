using Application.Abstraction.Services;
using Application.Abstraction.Storage;
using Application.DTO.User;
using Application.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IStorageService _storageService;

        public UserService(UserManager<User> userManager, IStorageService storageService)
        {
            _userManager = userManager;
            _storageService = storageService;
        }

        public async Task<UserResultDto> CreateUser(CreateUserDto createUser)
        {
           
            string? profilePictureUrl = null;
            if (createUser.ProfilePicture != null && createUser.ProfilePicture.Length > 0)
            {
                profilePictureUrl = await MediaHelper.UploadMediaAsync(
                    createUser.ProfilePicture, _storageService, "profile-pictures");
            }

            var user = new User()
            {
                Name = createUser.NameSurname,
                Email = createUser.Email,
                UserName = createUser.UserName,
                ProfilePictureUrl = profilePictureUrl
            };

            var result = await _userManager.CreateAsync(user, createUser.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new UserResultDto()
                {
                    Message = $"Kullanıcı oluşturulurken hata: {errors}",
                    Succeeded = false
                };
            }

            return new UserResultDto()
            {
                Message = "Kullanıcı başarıyla oluşturuldu.",
                Succeeded = true
            };
        }
    }
}
