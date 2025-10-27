using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FristName { get; set; } = null!;
        public string LastName { get; set; } = null!;

    }
}
