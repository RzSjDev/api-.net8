using api_net9.Application.Context;
using api_net9.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace api_net9.Infrastructure.context
{
    public class DataContext : DbContext,IDataContext
    {
 
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersContact>().HasOne(c => c.userAuth).WithMany(c => c.usersContacts).HasForeignKey(c=>c.UserAuthId);
        }

        public DbSet<UsersContact> usersContacts { get; set; }
        public DbSet<UserAuth> userAuths { get; set; }
    }
}

