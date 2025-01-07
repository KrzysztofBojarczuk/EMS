using EMS.CORE.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.INFRASTRUCTURE.Data
{
    public static class SeedData
    {
        public static async Task SeedUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUserEntity>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            string adminUserName = "admin";
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@12345";
            string userRole = "Admin";

            // Ensure Administrator role exists
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Ensure User role exists
            if (await roleManager.FindByNameAsync(userRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(userRole));
            }

            // Seed admin user
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUserEntity
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Add user John with a specific password
            string johnUserName = "John";
            string johnEmail = "john@example.com";
            string johnPassword = "John18!";
            if (await userManager.FindByEmailAsync(johnEmail) == null)
            {
                var johnUser = new AppUserEntity
                {
                    UserName = johnUserName,
                    Email = johnEmail,
                    EmailConfirmed = true
                };

                var johnResult = await userManager.CreateAsync(johnUser, johnPassword);

                if (johnResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(johnUser, "User");
                }
            }

            // Add employees if not exist
            if (!context.Employees.Any())
            {
                var johnUser = await userManager.FindByEmailAsync(johnEmail); // Retrieve John user

                var employees = new List<EmployeeEntity>
        {
            new EmployeeEntity { Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-789", Salary = 5000, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "987-654-321", Salary = 5500, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "Michael Johnson", Email = "michael.johnson@example.com", Phone = "456-789-123", Salary = 6000, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "Sarah Lee", Email = "sarah.lee@example.com", Phone = "321-654-987", Salary = 6200, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "David Brown", Email = "david.brown@example.com", Phone = "789-123-456", Salary = 6500, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "Emily White", Email = "emily.white@example.com", Phone = "159-357-468", Salary = 6800, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "Daniel Green", Email = "daniel.green@example.com", Phone = "753-159-246", Salary = 7000, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "Laura Black", Email = "laura.black@example.com", Phone = "246-813-579", Salary = 7200, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "James Clark", Email = "james.clark@example.com", Phone = "963-258-741", Salary = 7400, AppUserId = johnUser.Id },
            new EmployeeEntity { Name = "Olivia Harris", Email = "olivia.harris@example.com", Phone = "369-741-258", Salary = 7600, AppUserId = johnUser.Id }
        };

                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync(); // Commit changes to the database
            }
        }

    }
}