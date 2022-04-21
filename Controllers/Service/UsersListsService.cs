using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers.Service
{
    public class UsersListsService
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public UsersListsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<UsersMovies> GetAllUsersLists()
        {
            var allLists = _context.DbUsersMovies.ToList();
            return allLists;
        }

        public UsersLists UpdateUsersListsById(int Id, UsersListsView listView)
        {
            var _list = _context.DbUsersLists.FirstOrDefault(n => n.Id == Id);
            if (_list != null)
            {
                _list.ListName = listView.ListName;

                _mapper.Map(listView, _list);
                _context.SaveChanges();
                return _list;
            }
            else
            {
                throw new Exception("Greška! Niste unjeli dobar Id liste?");
            }
        }

        public void DeleteUsersListsById(int id)
        {
            var _list = _context.DbUsersLists.FirstOrDefault(n => n.Id == id);
            if (_list != null)
            {
                _context.DbUsersLists.Remove(_list);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Brisanje liste nije uspjelo!");
            }
        }
    }
}
