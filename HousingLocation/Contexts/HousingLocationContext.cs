using HousingLocation.Models;
using Microsoft.EntityFrameworkCore;

namespace HousingLocation.Contexts
{
    public class HousingLocationContext:DbContext
    {
        public DbSet<House> Houses { get; set; }

        public DbSet<Photo> Images { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserCard> UserCards { get; set; }

        public HousingLocationContext(DbContextOptions<HousingLocationContext> options) : base(options)
        { }

       


    }
}
