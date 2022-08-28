using Microsoft.EntityFrameworkCore;
using aspnet_core_dotnet_core.Model;

namespace aspnet_core_dotnet_core.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
