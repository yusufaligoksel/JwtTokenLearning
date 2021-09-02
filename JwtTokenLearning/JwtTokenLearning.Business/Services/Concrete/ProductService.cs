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
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IRepository<Product> _repository;
        public ProductService(IRepository<Product> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
