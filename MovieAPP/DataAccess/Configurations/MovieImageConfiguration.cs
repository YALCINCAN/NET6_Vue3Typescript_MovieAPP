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
    public class MovieImageConfiguration : IEntityTypeConfiguration<MovieImage>
    {
        public void Configure(EntityTypeBuilder<MovieImage> builder)
        {
            builder.Property(x => x.Image).IsRequired();
            builder.Property(x => x.MovieId).IsRequired();
        }
    }
}