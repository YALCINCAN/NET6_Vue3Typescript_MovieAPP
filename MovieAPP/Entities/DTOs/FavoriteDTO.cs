using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class FavoriteDTO:IDTO
    {
        public string UserId { get; set; }
        public int MovieId { get; set; }
    }
}
