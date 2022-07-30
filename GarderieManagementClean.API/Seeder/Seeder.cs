using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Seeder
{
    public class Seeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            // context.Database.EnsureCreated() does not use migrations to create the database and therefore the database that is created cannot be later updated using migrations 
            // use context.Database.Migrate() instead
            context.Database.Migrate();

            var users = await context.Users.ToListAsync();
            foreach (var user in users)
            {
                user.isOnline = false;
            }

            await context.SaveChangesAsync();
        }


    }
}
