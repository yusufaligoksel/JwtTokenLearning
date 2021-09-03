using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtTokenLearning.Business.Configuration;
using JwtTokenLearning.Business.Dto;
using JwtTokenLearning.Business.Entities;
using JwtTokenLearning.Business.Repository;
using JwtTokenLearning.Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JwtTokenLearning.Business.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly List<Client> _clients;
        private readonly IRepository<UserRefreshToken> _userRefreshToken;
        private readonly IUserRefreshTokenService _userRefreshTokenService;
        public AuthService(ITokenService tokenService,
            UserManager<User> userManager,
            IOptions<List<Client>> options,
            IRepository<UserRefreshToken> userRefreshToken,
            IUserRefreshTokenService userRefreshTokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _clients = options.Value;
            _userRefreshToken = userRefreshToken;
            _userRefreshTokenService = userRefreshTokenService;
        }
        public async Task<Response<TokenDto>> CreateToken(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Response<TokenDto>.Fail("Email veya şifre hatalıdır.", 400, true);

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.CreateToken(user, userRoles);

            var userRefreshToken = await _userRefreshToken.Table.Where(x => x.UserId == Convert.ToInt32(user.Id)).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshToken.InsertAsync(
                    new UserRefreshToken
                    {
                        UserId = Convert.ToInt32(user.Id),
                        Code = token.RefreshToken,
                        Expiration = token.RefreshTokenExpiration
                    });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }


            return Response<TokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var userRefreshToken = await _userRefreshToken.Table.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(userRefreshToken.UserId.ToString());
            IList<string> list = new List<string>();
            var token = _tokenService.CreateToken(user, list);

            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;

            return Response<TokenDto>.Success(token, 200);

        }

        public async Task<Response<string>> RevokeRefreshToken(string refreshToken)
        {
            var userRefreshToken = await _userRefreshToken.Table.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            await _userRefreshTokenService.DeleteAsync(userRefreshToken);

            return Response<string>.Success("Silindi.", 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.FirstOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
                return Response<ClientTokenDto>.Fail("ClientId or Client Secret not found", 404, true);

            var clientToken = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(clientToken, 200);
        }
    }
}
