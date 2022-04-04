﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Category:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<MovieCategory> MovieCategories { get; set; }
    }
}
