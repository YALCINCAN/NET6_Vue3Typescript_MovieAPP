using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public string Image { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
