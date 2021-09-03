using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtTokenLearning.Business.Configuration;
using JwtTokenLearning.Business.Dto;
using JwtTokenLearning.Business.Entities;

namespace JwtTokenLearning.Business.Services.Abstract
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user, IList<string> userRoles);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
