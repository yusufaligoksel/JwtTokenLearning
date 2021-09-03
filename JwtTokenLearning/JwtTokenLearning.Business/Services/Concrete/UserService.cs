using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtTokenLearning.Business.Dto;
using JwtTokenLearning.Business.Entities;
using JwtTokenLearning.Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace JwtTokenLearning.Business.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var user = new User
                {
                    Email = createUserDto.Email,
                    UserName = createUserDto.UserName,
                    FullAddress = createUserDto.FullAddress,
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.Lastname
                };

                var result = await _userManager.CreateAsync(user, createUserDto.Password);

                if (!result.Succeeded)
                {
                    var errorList = result.Errors.Select(x => x.Description).ToList();
                    return Response<UserDto>.Fail(new ErrorDto(errorList, true), 400);
                }

                var response = new UserDto();

                response.Email = user.Email;
                response.FirstName = user.FirstName;
                response.LastName = user.LastName;
                response.FullAddress = user.FullAddress;
                response.UserName = user.UserName;


                if (createUserDto.roles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, createUserDto.roles);
                    if (roleResult.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null)
                        {
                            foreach (var role in roles)
                            {
                                response.roles.Add(role);
                            }
                        }
                    }
                }

                return Response<UserDto>.Success(response, 200);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
