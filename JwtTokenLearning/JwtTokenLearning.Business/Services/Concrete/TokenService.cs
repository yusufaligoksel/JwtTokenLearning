using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtTokenLearning.Business.Configuration;
using JwtTokenLearning.Business.Dto;
using JwtTokenLearning.Business.Entities;
using JwtTokenLearning.Business.Services.Abstract;
using JwtTokenLearning.Business.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtTokenLearning.Business.Services.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly CustomTokenOption _customTokenOption;

        public TokenService(UserManager<User> userManager, IOptions<CustomTokenOption> options)
        {
            _userManager = userManager;
            _customTokenOption = options.Value;
        }

        public TokenDto CreateToken(User user,IList<string> userRoles)
        {
            try
            {
                var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
                var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration);
                var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);

                SigningCredentials signingCredentials =
                    new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                    issuer: _customTokenOption.Issuer,
                    expires: accessTokenExpiration,
                    notBefore: DateTime.Now,
                    claims: GetClaims(user, _customTokenOption.Audience, userRoles),
                    signingCredentials: signingCredentials);


                var handler = new JwtSecurityTokenHandler();

                var token = handler.WriteToken(jwtSecurityToken);

                var tokenDto = new TokenDto
                {
                    AccessToken = token,
                    RefreshToken = CreateRefreshToken(),
                    AccessTokenExpriration = accessTokenExpiration,
                    RefreshTokenExpiration = refreshTokenExpiration
                };

                return tokenDto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimByClient(client),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpriration = accessTokenExpiration,
            };

            return tokenDto;
        }

        private IEnumerable<Claim> GetClaimByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()));

            return claims;
        }

        private IEnumerable<Claim> GetClaims(User userApp, List<String> audiences, IList<string> userRoles)
        {
            var userList = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
                userList.Add(new Claim(ClaimTypes.Role, role));

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userList;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
    }
}
