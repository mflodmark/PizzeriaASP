using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaASP.Models
{
    public interface IIdentityRepository
    {
        DbSet<ApplicationUser> Users { get; }

        DbSet<IdentityRole> Roles { get; }
        
        ApplicationUser GetSingleUser(string username);

        IdentityRole GetSingleRole(string role);


    }
}
