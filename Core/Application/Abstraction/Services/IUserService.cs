using Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstraction.Services
{
    public interface IUserService
    {
     Task<UserResultDto> CreateUser(CreateUserDto createUser);
    }
}
