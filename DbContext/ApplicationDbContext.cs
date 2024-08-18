using Microsoft.EntityFrameworkCore;
using mini_crm.Models;

namespace mini_crm.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PhysicalPerson> PhysicalPersons { get; set; }
        public DbSet<LegalPerson> LegalPersons { get; set; }
        public DbSet<Contract> Contracts { get; set; }

    }
}
