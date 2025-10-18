using EntityFramworkPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramworkPractice.Data
{

    public class ApplicationDBContext :DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teachers> Teachers { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-SEUURFE;Database=EFDB;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;");
        }
    }
}
