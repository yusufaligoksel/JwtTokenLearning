using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtTokenLearning.Business.Dto
{
    public class CreateUserDto
    {
        public CreateUserDto()
        {
            this.roles = new List<string>();
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string FullAddress { get; set; }
        public IEnumerable<string> roles { get; set; }
    }
}
