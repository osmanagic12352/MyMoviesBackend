using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMoviesBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers.Service
{
    public class RolesService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly RoleManager<IdentityUserRole<int>> _userRoleManager;
        private AppDbContext _context;

        public RolesService(RoleManager<IdentityRole<int>> roleManager, AppDbContext context, RoleManager<IdentityUserRole<int>> userRoleManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userRoleManager = userRoleManager;
        }


        public async Task AddRole(string name)
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<int>(name));
                if (!roleResult.Succeeded)
                    throw new Exception($"Dodavanje uloge {name} nije uspjelo!");

            }
            else
            {
                throw new Exception($"Uloga sa imenom '{name}' već postoji!");
            }
        }

        public List<IdentityRole<int>> GetAllRoles()
        {
            var allRoles = _roleManager.Roles.ToList();
            return allRoles;
        }

        public List<IdentityUserRole<int>> GetAllUserRoles()
        {
            var allRole = _userRoleManager.Roles.ToList();
            return allRole;
        }

        public void DeleteRole(string name)
        {
            var _user = _context.Roles.FirstOrDefault(n => n.Name == name);
            if (_user != null)
            {
                _context.Roles.Remove(_user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Brisanje korisnika nije uspjelo! Da li ste upisali dobro naziv korisnika?");
            }
        }
    }
}
