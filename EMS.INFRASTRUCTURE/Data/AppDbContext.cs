using EMS.APPLICATION.Dtos;
using EMS.CORE.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<PlannedExpenseEntity> PlannedExpenses { get; set; }
        public DbSet<BudgetEntity> Budgets { get; set; }
        public DbSet<AddressEntity> Address { get; set; }
        public DbSet<EmployeeListsEntity> EmployeeLists { get; set; }
        public DbSet<LocalEntity> Locals { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<VehicleEntity> Vehicles { get; set; }
        public DbSet<LogEntity> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ReservationEntity>()
                .HasOne(x => x.LocalEntity)
                .WithMany(x => x.ReservationsEntities)
                .HasForeignKey(x => x.LocalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUserEntity>()
                .HasMany(x => x.ReservationsEntities)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired();

            builder.Entity<AppUserEntity>()
                .HasMany(x => x.LocalEntities)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired();

            builder.Entity<AppUserEntity>()
                .HasMany(x => x.TaskEntity)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired();

            builder.Entity<AppUserEntity>()
                .HasMany(x => x.EmployeeEntities)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired();

            builder.Entity<BudgetEntity>()
                .HasMany(x => x.TransactionEntity)
                .WithOne(x => x.BudgetEntity)
                .HasForeignKey(x => x.BudgetId)
                .IsRequired();

            builder.Entity<BudgetEntity>()
                .HasMany(x => x.PlannedExpenseEntity)
                .WithOne(x => x.BudgetEntity)
                .HasForeignKey(x => x.BudgetId)
                .IsRequired();

            builder.Entity<AppUserEntity>()
                .HasMany(x => x.AddressEntities)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired();

            builder.Entity<AppUserEntity>()
                .HasOne(x => x.BudgetEntity)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey<BudgetEntity>(x => x.AppUserId)
                .IsRequired();

            builder.Entity<TaskEntity>()
                .HasOne(x => x.AddressEntity)
                .WithMany(x => x.TaskEntities)
                .HasForeignKey(x => x.AddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.Entity<TaskEntity>()
                .HasMany(x => x.EmployeeListsEntities)
                .WithOne(x => x.TaskEntities)
                .HasForeignKey(x => x.TaskId);

            builder.Entity<AppUserEntity>()
                .HasMany(x => x.EmployeeListsEntities)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired();

            builder.Entity<EmployeeListsEntity>()
                .HasMany(x => x.EmployeesEntities)
                .WithOne(x => x.EmployeeListsEntity)
                .HasForeignKey(x => x.EmployeeListId);

            builder.Entity<AppUserEntity>()
                .HasMany(x => x.VehicleEntities)
                .WithOne(x => x.AppUserEntity)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired();

            builder.Entity<TaskEntity>()
                .HasMany(x => x.VehicleEntities)
                .WithOne(x => x.TaskEntities)
                .HasForeignKey(x => x.TaskId)
                .IsRequired(false);

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