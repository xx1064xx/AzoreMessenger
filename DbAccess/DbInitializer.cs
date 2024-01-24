using AzoreMessanger.Data;
using AzoreMessanger.Models;

namespace AzoreMessanger.DbAccess
{
    public class DbInitializer
    {
        private readonly MessengerAppContext _context;
        public DbInitializer(MessengerAppContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (_context.Database.EnsureCreated())
            {
                string salt;
                string pwHash = HashGenerator.GenerateHash("admin", out salt);
                _context.Users.Add(
                    new User
                    {
                        username = "admin",
                        email = "admin@admin.com",
                        password = pwHash,
                        Salt = salt
                    });

                _context.Browsers.Add(
                    new Browser
                    {
                        browsername = "admin",
                        browsernumber = 2000,
                        userId = 1
                    });

                _context.SaveChanges();
            }

        }
    }
}
