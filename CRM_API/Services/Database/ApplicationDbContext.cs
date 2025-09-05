using CRM_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_API.Services.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> Client {  get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<LoginDetails> LoginDetails { get; set; }
        public DbSet<ClientType> ClientType { get; set; }
        public DbSet<Titles> Titles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


    }
}
