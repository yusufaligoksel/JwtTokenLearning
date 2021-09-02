using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtTokenLearning.Business.Entities;
using JwtTokenLearning.Business.Repository;
using JwtTokenLearning.Business.Services.Abstract;

namespace JwtTokenLearning.Business.Services.Concrete
{
    public class UserRefreshTokenServices:BaseService<UserRefreshToken>, IUserRefreshTokenService
    {
        private readonly IRepository<UserRefreshToken> _repository;
        public UserRefreshTokenServices(IRepository<UserRefreshToken> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
