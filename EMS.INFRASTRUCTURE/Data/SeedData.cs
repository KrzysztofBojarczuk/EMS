using EMS.CORE.Entities;
using EMS.CORE.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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
                    new EmployeeEntity { Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-789", Salary = 5000, Age = 30, EmploymentDate = new DateTime(2018, 5, 1), MedicalCheckValidUntil = new DateTime(2025, 5, 1), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "987-654-321", Salary = 5500, Age = 28, EmploymentDate = new DateTime(2019, 3, 15), MedicalCheckValidUntil = new DateTime(2025, 3, 15), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "Michael Johnson", Email = "michael.johnson@example.com", Phone = "456-789-123", Salary = 6000, Age = 35, EmploymentDate = new DateTime(2017, 7, 20), MedicalCheckValidUntil = new DateTime(2024, 7, 20), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "Sarah Lee", Email = "sarah.lee@example.com", Phone = "321-654-987", Salary = 6200, Age = 32, EmploymentDate = new DateTime(2020, 1, 10), MedicalCheckValidUntil = new DateTime(2025, 1, 10), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "David Brown", Email = "david.brown@example.com", Phone = "789-123-456", Salary = 6500, Age = 40, EmploymentDate = new DateTime(2016, 9, 30), MedicalCheckValidUntil = new DateTime(2024, 9, 30), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "Emily White", Email = "emily.white@example.com", Phone = "159-357-468", Salary = 6800, Age = 27, EmploymentDate = new DateTime(2021, 2, 12), MedicalCheckValidUntil = new DateTime(2026, 2, 12), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "Daniel Green", Email = "daniel.green@example.com", Phone = "753-159-246", Salary = 7000, Age = 38, EmploymentDate = new DateTime(2015, 11, 5), MedicalCheckValidUntil = new DateTime(2024, 11, 5), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "Laura Black", Email = "laura.black@example.com", Phone = "246-813-579", Salary = 7200, Age = 33, EmploymentDate = new DateTime(2017, 6, 22), MedicalCheckValidUntil = new DateTime(2024, 6, 22), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "James Clark", Email = "james.clark@example.com", Phone = "963-258-741", Salary = 7400, Age = 36, EmploymentDate = new DateTime(2016, 12, 18), MedicalCheckValidUntil = new DateTime(2024, 12, 18), AppUserId = johnUser.Id },
                    new EmployeeEntity { Name = "Olivia Harris", Email = "olivia.harris@example.com", Phone = "369-741-258", Salary = 7600, Age = 29, EmploymentDate = new DateTime(2019, 9, 30), MedicalCheckValidUntil = new DateTime(2025, 9, 30), AppUserId = johnUser.Id }
                };

                var vehicles = new List<VehicleEntity>
                {
                    new VehicleEntity { Brand = "Toyota", Model = "Corolla", Name = "Car 1", RegistrationNumber = "ABC-001", Mileage = 12000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2018, 5, 1), InsuranceOcValidUntil = new DateTime(2025,5,1), InsuranceOcCost = 500, TechnicalInspectionValidUntil = new DateTime(2025,5,1), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Honda", Model = "Civic", Name = "Car 2", RegistrationNumber = "ABC-002", Mileage = 15000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2019, 3, 15), InsuranceOcValidUntil = new DateTime(2025,3,15), InsuranceOcCost = 550, TechnicalInspectionValidUntil = new DateTime(2025,3,15), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Ford", Model = "Focus", Name = "Car 3", RegistrationNumber = "ABC-003", Mileage = 20000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2017, 7, 20), InsuranceOcValidUntil = new DateTime(2024,7,20), InsuranceOcCost = 600, TechnicalInspectionValidUntil = new DateTime(2024,7,20), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "BMW", Model = "320i", Name = "Car 4", RegistrationNumber = "ABC-004", Mileage = 18000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2020, 1, 10), InsuranceOcValidUntil = new DateTime(2025,1,10), InsuranceOcCost = 650, TechnicalInspectionValidUntil = new DateTime(2025,1,10), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Audi", Model = "A4", Name = "Car 5", RegistrationNumber = "ABC-005", Mileage = 10000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2021, 6, 5), InsuranceOcValidUntil = new DateTime(2026,6,5), InsuranceOcCost = 700, TechnicalInspectionValidUntil = new DateTime(2026,6,5), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Fiat", Model = "500", Name = "Car 6", RegistrationNumber = "ABC-006", Mileage = 8000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2022, 2, 20), InsuranceOcValidUntil = new DateTime(2027,2,20), InsuranceOcCost = 550, TechnicalInspectionValidUntil = new DateTime(2027,2,20), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Mercedes", Model = "C200", Name = "Car 7", RegistrationNumber = "ABC-007", Mileage = 25000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2016, 9, 30), InsuranceOcValidUntil = new DateTime(2024,9,30), InsuranceOcCost = 800, TechnicalInspectionValidUntil = new DateTime(2024,9,30), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Nissan", Model = "Leaf", Name = "Car 8", RegistrationNumber = "ABC-008", Mileage = 5000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2021, 11, 12), InsuranceOcValidUntil = new DateTime(2026,11,12), InsuranceOcCost = 600, TechnicalInspectionValidUntil = new DateTime(2026,11,12), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Volkswagen", Model = "Golf", Name = "Car 9", RegistrationNumber = "ABC-009", Mileage = 22000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2018, 8, 18), InsuranceOcValidUntil = new DateTime(2025,8,18), InsuranceOcCost = 650, TechnicalInspectionValidUntil = new DateTime(2025,8,18), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Kia", Model = "Rio", Name = "Car 10", RegistrationNumber = "ABC-010", Mileage = 12000, VehicleType = VehicleType.Car, DateOfProduction = new DateTime(2019, 12, 25), InsuranceOcValidUntil = new DateTime(2025,12,25), InsuranceOcCost = 550, TechnicalInspectionValidUntil = new DateTime(2025,12,25), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Ford", Model = "Transit", Name = "Van 1", RegistrationNumber = "VAN-001", Mileage = 30000, VehicleType = VehicleType.Van, DateOfProduction = new DateTime(2017, 4, 10), InsuranceOcValidUntil = new DateTime(2024,4,10), InsuranceOcCost = 700, TechnicalInspectionValidUntil = new DateTime(2024,4,10), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Mercedes", Model = "Sprinter", Name = "Van 2", RegistrationNumber = "VAN-002", Mileage = 40000, VehicleType = VehicleType.Van, DateOfProduction = new DateTime(2018, 6, 15), InsuranceOcValidUntil = new DateTime(2025,6,15), InsuranceOcCost = 750, TechnicalInspectionValidUntil = new DateTime(2025,6,15), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Volkswagen", Model = "Crafter", Name = "Van 3", RegistrationNumber = "VAN-003", Mileage = 35000, VehicleType = VehicleType.Van, DateOfProduction = new DateTime(2019, 8, 20), InsuranceOcValidUntil = new DateTime(2025,8,20), InsuranceOcCost = 700, TechnicalInspectionValidUntil = new DateTime(2025,8,20), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Renault", Model = "Master", Name = "Van 4", RegistrationNumber = "VAN-004", Mileage = 45000, VehicleType = VehicleType.Van, DateOfProduction = new DateTime(2020, 2, 5), InsuranceOcValidUntil = new DateTime(2026,2,5), InsuranceOcCost = 720, TechnicalInspectionValidUntil = new DateTime(2026,2,5), IsAvailable = true, AppUserId = johnUser.Id },
                    new VehicleEntity { Brand = "Iveco", Model = "Daily", Name = "Van 5", RegistrationNumber = "VAN-005", Mileage = 50000, VehicleType = VehicleType.Van, DateOfProduction = new DateTime(2016, 11, 11), InsuranceOcValidUntil = new DateTime(2024,11,11), InsuranceOcCost = 800, TechnicalInspectionValidUntil = new DateTime(2024,11,11), IsAvailable = true, AppUserId = johnUser.Id }
                };

                var addresses = new List<AddressEntity>
                {
                    new AddressEntity { Id = Guid.NewGuid(), City = "New York", Street = "5th Avenue", Number = "101", ZipCode = "10001", AppUserId = johnUser.Id },
                    new AddressEntity { Id = Guid.NewGuid(), City = "Los Angeles", Street = "Sunset Boulevard", Number = "202", ZipCode = "90001", AppUserId = johnUser.Id },
                    new AddressEntity { Id = Guid.NewGuid(), City = "Chicago", Street = "Michigan Avenue", Number = "303", ZipCode = "60601", AppUserId = johnUser.Id },
                    new AddressEntity { Id = Guid.NewGuid(), City = "Houston", Street = "Main Street", Number = "404", ZipCode = "77001", AppUserId = johnUser.Id },
                    new AddressEntity { Id = Guid.NewGuid(), City = "San Francisco", Street = "Market Street", Number = "505", ZipCode = "94101", AppUserId = johnUser.Id }
                };

                await context.Employees.AddRangeAsync(employees);
                await context.Vehicles.AddRangeAsync(vehicles);
                await context.Address.AddRangeAsync(addresses);
                await context.SaveChangesAsync(); // Commit changes to the database
            }
        }
    }
}