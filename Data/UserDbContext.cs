using Microsoft.EntityFrameworkCore;
using WebApplicationDemo.Entities;

namespace WebApplicationDemo.Data
{
    public class UserDbContext : DbContext
    {
        /// <summary>
        /// The user db context
        /// </summary>
        /// <param name="options">The options</param>
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        
        /// <summary>
        /// Get or Set the use in Db
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
