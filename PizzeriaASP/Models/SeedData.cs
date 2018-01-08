using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            TomasosContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if (!context.Matratt.Any())
            {
                context.Matratt.AddRange(
                    new Matratt() {MatrattNamn = "Hawaii", Beskrivning = "Fruktig pizza med ananas", Pris = 76});
            }
        }
    }
}