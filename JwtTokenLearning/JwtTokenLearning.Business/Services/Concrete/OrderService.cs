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
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IRepository<Order> _repository;
        public OrderService(IRepository<Order> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
