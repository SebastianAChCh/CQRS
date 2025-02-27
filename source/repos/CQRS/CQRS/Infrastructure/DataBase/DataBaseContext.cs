using CQRS.Domains;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Infrastructure.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { 
        }

        public DbSet<Post> posts { get; set; }
    }
}
