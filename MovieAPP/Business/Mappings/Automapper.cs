using AutoMapper;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mappings
{
    public class Automapper:Profile
    {
        public Automapper()
        {
            // Auth
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            //User
            CreateMap<User, UpdateProfileDTO>().ReverseMap();
            CreateMap<User, ChangeImage>().ReverseMap();
            //Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            //Movie
            CreateMap<Movie, MovieDTO>().ReverseMap();
            //Actor
            CreateMap<Actor,ActorDTO>().ReverseMap();
            //Comment
            CreateMap<Comment, CommentDTO>().ReverseMap();
            //Role
            CreateMap<Role, RoleDTO>().ReverseMap();
            //Favorite
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
        }
    }
}
