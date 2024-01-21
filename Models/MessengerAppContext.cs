using AzoreMessanger.Models;
using Microsoft.EntityFrameworkCore;

namespace AzoreMessanger.Models
{
    public class MessengerAppContext : DbContext
    {
        public MessengerAppContext(DbContextOptions<MessengerAppContext> options)
            : base(options) { }
        public DbSet<User> Users { get; set; } = null!;

    }
}