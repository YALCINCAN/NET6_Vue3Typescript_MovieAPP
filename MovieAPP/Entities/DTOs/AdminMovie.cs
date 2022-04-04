using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class AdminMovie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string MainImage { get; set; }
        public decimal Imdb { get; set; }
        public string ReleaseDateString { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> ActorIds { get; set; }
        public IEnumerable<MovieImageDetail> MovieImages { get; set; }
    }
}
