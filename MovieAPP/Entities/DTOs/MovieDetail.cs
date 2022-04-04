using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class MovieDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }
        public string MainImage { get; set; }
        public decimal Imdb { get; set; }
        public string ReleaseDate { get; set; }
        public List<CommentDetail> Comments { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public List<ActorDetail> Actors { get; set; }
        public List<MovieImageDetail> Images { get; set; }
    }
}
