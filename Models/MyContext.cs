using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        public DbSet<User> Users {get;set;}
        public DbSet<Jubilee> Jubilees {get;set;}
        public DbSet<Outing> Outings {get;set;}
    }
}