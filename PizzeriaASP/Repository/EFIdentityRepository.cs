using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class EFIdentityRepository : IIdentityRepository
    {
        private readonly ApplicationDbContext _context;

        public EFIdentityRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public DbSet<ApplicationUser> Users => _context.Users;

        public DbSet<IdentityRole> Roles => _context.Roles;


        public ApplicationUser GetSingleUser(string username)
        {
            return _context.Users.Single(u => u.UserName == username);
        }

        public IdentityRole GetSingleRole(string role)
        {
            return _context.Roles.Find(role);
        }
    }
}