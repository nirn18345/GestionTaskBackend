using AuthServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Infraestructure.Data
{
    public class AuthDbContext:DbContext 
    {


        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<Account> Account => Set<Account>();
        public DbSet<User> Users => Set<User>();


        public DbSet<Role> Role => Set<Role>();
        public DbSet<Domain.Entities.Task> Task => Set<Domain.Entities.Task>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        

            modelBuilder.Entity<Account>().ToTable("Account","core");
            modelBuilder.Entity<User>().ToTable("User","core");
            modelBuilder.Entity<Role>().ToTable("Role", "core");
            modelBuilder.Entity<Domain.Entities.Task>().ToTable("Task", "core");
        }
    }
}
