using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtTokenLearning.Business.Configuration;
using JwtTokenLearning.Business.Dto;
using JwtTokenLearning.Business.Entities;
using JwtTokenLearning.Business.Services.Abstract;

namespace JwtTokenLearning.Business.Services.Concrete
{
    public class TokenService:ITokenService
    {
        public TokenDto CreateToken(User user)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
