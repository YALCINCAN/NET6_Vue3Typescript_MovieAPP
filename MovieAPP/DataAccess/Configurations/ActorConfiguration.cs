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
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(x => x.FullName).IsRequired();
            builder.Property(x => x.About).IsRequired();
            builder.Property(x => x.Image).IsRequired();
            builder.Property(x => x.Slug).IsRequired();
        }
    }
}
