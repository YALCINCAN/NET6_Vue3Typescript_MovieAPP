using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class MovieWithCategoriesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string MainImage { get; set; }
        public decimal Imdb { get; set; }
        public string ReleaseDate { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
    }
}
