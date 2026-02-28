using Application.Abstraction.Services;
using Application.Abstraction.Storage;
using Application.DTO.User;
using Application.Events;
using Application.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IStorageService _storageService;
        private readonly IEventBus _eventBus;

        public UserService(UserManager<User> userManager, IStorageService storageService, IEventBus eventBus)
        {
            _userManager = userManager;
            _storageService = storageService;
            _eventBus = eventBus;
        }

        public async Task<UserResultDto> CreateUser(CreateUserDto createUser)
        {
            string? profilePictureUrl = null;
            if (createUser.ProfilePictureUrl != null && createUser.ProfilePictureUrl.Length > 0)
            {
                profilePictureUrl = await MediaHelper.UploadMediaAsync(
                    createUser.ProfilePictureUrl, _storageService, "profile-pictures");
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

        
            var rawToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));
            await _eventBus.PublishAsync(new UserRegisteredEvent
            {
                UserId = user.Id.ToString(),
                Email = user.Email!,
                Name = user.Name,
                VerificationToken = encodedToken
            });

            return new UserResultDto()
            {
                Message = "Kullanıcı başarıyla oluşturuldu.",
                Succeeded = true
            };
        }
    }
}
