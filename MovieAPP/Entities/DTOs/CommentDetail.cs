using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class CommentDetail
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserId { get; set; }
        public UserDetail User { get; set; }
    }
}
