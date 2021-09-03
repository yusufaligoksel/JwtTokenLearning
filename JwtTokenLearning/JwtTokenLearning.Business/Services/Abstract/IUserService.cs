using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtTokenLearning.Business.Dto;

namespace JwtTokenLearning.Business.Services.Abstract
{
    public interface IUserService
    {
        Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
    }
}
