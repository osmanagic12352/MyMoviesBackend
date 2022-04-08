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
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IMapper _mapper;
        private AppDbContext _context;


        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager, IMapper mapper, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }


        public async Task InsertUser(AppUserView userView)
        {

            var UserCheck = await _userManager.FindByNameAsync(userView.UserName);
            if (UserCheck != null)
            {
                throw new ApplicationException("Username već postoji!");
            }
            var EmailCheck = await _userManager.FindByEmailAsync(userView.Email);
            if (EmailCheck != null)
            {
                throw new ApplicationException("Email već postoji!");
            }


            var user = _mapper.Map<AppUser>(userView);
            user = new AppUser()
            {
                UserName = userView.UserName,
                Email = userView.Email,
                FullName = userView.FullName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, userView.Password);
            if (!result.Succeeded)
                throw new Exception("Greška u registraciji. Da li ste pripazili na dodavanje brojeva, jednog karaktera specifičnog i veliko slovo?");

            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Admin));

            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole<int>(Roles.User));

            if (await _roleManager.RoleExistsAsync(Roles.User))
                await _userManager.AddToRoleAsync(user, Roles.User);
        }

        public async Task InsertAdmin(AppUserView adminView)
        {
            var UserCheck = await _userManager.FindByNameAsync(adminView.UserName);
            if (UserCheck != null)
            {
                throw new ApplicationException("Username već postoji!");
            }
            var EmailCheck = await _userManager.FindByEmailAsync(adminView.Email);
            if (EmailCheck != null)
            {
                throw new ApplicationException("Email već postoji!");
            }

            var admin = _mapper.Map<AppUser>(adminView);
            admin = new AppUser()
            {
                UserName = adminView.UserName,
                Email = adminView.Email,
                FullName = adminView.FullName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(admin, adminView.Password);
            if (!result.Succeeded)
                throw new Exception("Greška u Bazi!");

            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new IdentityRole<int>(Roles.Admin));

            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole<int>(Roles.User));

            if (await _roleManager.RoleExistsAsync(Roles.Admin))
                await _userManager.AddToRoleAsync(admin, Roles.Admin);

        }
        public List<AppUser> GetAllUsers()
        {
            var allUsers = _context.DbUsers.ToList();
            return allUsers;
        }
    }
}
