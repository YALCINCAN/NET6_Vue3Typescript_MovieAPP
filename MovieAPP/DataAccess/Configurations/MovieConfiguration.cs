using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Slug).IsRequired();
            builder.Property(x=>x.ReleaseDate).IsRequired();
            builder.Property(x=>x.Summary).IsRequired();
            builder.Property(x=>x.Imdb).IsRequired();
            builder.Property(x=>x.MainImage).IsRequired();
        }
    }
}
