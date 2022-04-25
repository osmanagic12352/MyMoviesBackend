using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        //public async Task Login([FromBody] Login login)
        //{
        //    var UserCheck = await _userManager.FindByNameAsync(login.Email);
        //    if (UserCheck != null && await _userManager.CheckPasswordAsync(UserCheck, login.Password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(UserCheck);
        //        IdentityOptions _options = new IdentityOptions();

        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, UserCheck.UserName),
        //            new Claim("UserID", UserCheck.Id.ToString()),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(_options.ClaimsIdentity.RoleClaimType,userRoles.FirstOrDefault())
        //        };


        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        var LoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CryptoBanking123"));

        //        var TokenSettings = new JwtSecurityToken(
        //            issuer: "https://localhost:5001",
        //            audience: "https://localhost:5001",
        //            expires: DateTime.Now.AddHours(5),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(LoginKey, SecurityAlgorithms.HmacSha256)
        //            );

        //        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(TokenSettings) });
        //    }
        //    else
        //    {
        //        throw new Exception("Neuspjela prijava! (Da li ste upisali dobro svoje korisničke podatke?)");
        //    }
        //}


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
                Admin = "NE",
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
                Admin = "DA",
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

        public AppUser GetUserById(int Id)
        {
            var GetUser = _context.DbUsers.FirstOrDefault(n => n.Id == Id);
            return GetUser;
        }

        public AppUser UpdateUserById(int Id, AppUserView userView)
        {
            var _user = _context.DbUsers.FirstOrDefault(n => n.Id == Id);
            if (_user != null)
            {
                _user.UserName = userView.UserName;
                _user.Email = userView.Email;
                _user.FullName = userView.FullName;
                _user.PasswordHash = userView.Password;

                _mapper.Map(userView, _user);
                _context.SaveChanges();
                return _user;
            }
            else
            {
                throw new Exception("Greška! Niste unjeli dobar Id usera?");
            }
        }

        public void DeleteUserById(int id)
        {
            var _user = _context.DbUsers.FirstOrDefault(n => n.Id == id);
            if (_user != null)
            {
                _context.DbUsers.Remove(_user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Brisanje korisnika nije uspjelo!");
            }
        }

        
    }
}
