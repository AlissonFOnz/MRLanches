using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MRLanches.Users.API.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=mrlanches_users;Username=postgres;Password=your_password");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
