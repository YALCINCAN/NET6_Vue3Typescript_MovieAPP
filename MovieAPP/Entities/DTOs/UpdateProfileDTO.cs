using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class UpdateProfileDTO : IDTO
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
