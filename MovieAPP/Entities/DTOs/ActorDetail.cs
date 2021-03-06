using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ActorDetail
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Slug { get; set; }
        public string About { get; set; }
        public string Image { get; set; }
        public IEnumerable<MovieWithCategoriesDTO> Movies { get; set; }

    }
}
