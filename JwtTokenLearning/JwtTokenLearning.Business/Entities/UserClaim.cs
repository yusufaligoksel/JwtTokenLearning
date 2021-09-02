using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JwtTokenLearning.Business.Entities
{
    public class UserClaim:IdentityUserClaim<int>
    {
    }
}
