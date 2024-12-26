using EMS.CORE.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EMS.INFRASTRUCTURE.Data
{
    public class AppDbContext : IdentityDbContext<AppUserEntity>
    {
        public AppDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUserEntity>()
                .HasMany(e => e.TaskEntity)
                .WithOne(e => e.AppUserEntity)
                .HasForeignKey(e => e.AppUserId)
            .IsRequired();

            builder.Entity<AppUserEntity>()
               .HasMany(e => e.EmployeeEntities)
               .WithOne(e => e.AppUserEntity)
               .HasForeignKey(e => e.AppUserId)
               .IsRequired();

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
            
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
