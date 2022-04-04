using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Movie:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }
        public string MainImage { get; set; }
        public decimal Imdb { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<MovieCategory> MovieCategories { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
        public ICollection<MovieImage> MovieImages { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
