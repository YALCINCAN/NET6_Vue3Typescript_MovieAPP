using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Favorite:IEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
