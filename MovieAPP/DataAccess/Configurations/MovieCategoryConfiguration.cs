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
    public class MovieCategoryConfiguration : IEntityTypeConfiguration<MovieCategory>
    {
        public void Configure(EntityTypeBuilder<MovieCategory> builder)
        {
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.MovieId).IsRequired();
        }
    }
}
