using AutoMapper;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<AppUserView, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<Login, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<UsersMoviesView, UsersMovies>();

            CreateMap<UsersListsView, UsersLists>();

            CreateMap<MoviesView, Movies>();

            CreateMap<FavoriteView, Favorite>();
        }
    }
}
