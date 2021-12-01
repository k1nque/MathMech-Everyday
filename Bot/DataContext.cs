using Bot.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bot
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<AppUsers> Users { get; set; }
    }
}