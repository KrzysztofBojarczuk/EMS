using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
    {
        public async Task<IEnumerable<EmployeeEntity>> GetUserEmployeesAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.Employees.AsQueryable();

            query = query.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetUserNumberOfEmployeesAsync(string appUserId)
        {
            return await dbContext.Employees.Where(x => x.AppUserId == appUserId).CountAsync();
        }

        public async Task<int> GetNumberOfEmployeesAsync()
        {
            return await dbContext.Employees.CountAsync();
        }

        public async Task<IEnumerable<EmployeeEntity>> GetEmployeesAsync(string searchTerm)
        {
            var query = dbContext.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<EmployeeEntity> GetEmployeeByIdAsync(Guid id)
        {
            return await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity entity)
        {
            entity.Id = Guid.NewGuid(); //służy do przypisania nowego, unikalnego identyfikatora
            dbContext.Employees.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<EmployeeEntity> UpdateEmployeeAsync(Guid employeeId, EmployeeEntity entity)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee is not null)
            {
                employee.Name = entity.Name;
                employee.Email = entity.Email;
                employee.Phone = entity.Phone;
                employee.Salary = entity.Salary;

                await dbContext.SaveChangesAsync();

                return employee;
            }

            return entity;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid employeeId)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee is not null)
            {
                dbContext.Employees.Remove(employee);

                return await dbContext.SaveChangesAsync() > 0; //Jeśli usunięcie się powiodło: SaveChangesAsync() zwróci liczbę większą od 0, więc metoda zwróci true.
            }

            return false;
        }

        public async Task<EmployeeListsEntity> AddEmployeeListsAsync(EmployeeListsEntity entity, List<Guid> employeeIds)
        {
            entity.Id = Guid.NewGuid();
            dbContext.EmployeeLists.Add(entity);

            var employees = await dbContext.Employees
                  .Where(e => employeeIds.Contains(e.Id))
                  .ToListAsync();

            entity.EmployeesEntities = employees;

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.EmployeeLists.Include(x => x.EmployeesEntities).AsQueryable();

            query = query.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsForTaskAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.EmployeeLists.Include(x => x.EmployeesEntities).AsQueryable();

            query = query.Where(x => x.AppUserId == appUserId && x.TaskId == null);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeEntity>> GetUserEmployeesForListAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.Employees.AsQueryable();

            query = query.Where(x => x.AppUserId == appUserId && x.EmployeeListId == null);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> DeleteEmployeeListsAsync(Guid employeeListId)
        {
            var employeeList = await dbContext.EmployeeLists.FirstOrDefaultAsync(x => x.Id == employeeListId);

            if (employeeList is not null)
            {
                var employees = await dbContext.Employees.Where(e => e.EmployeeListId == employeeListId).ToListAsync();

                foreach (var employee in employees)
                {
                    employee.EmployeeListId = null;
                }

                dbContext.EmployeeLists.Remove(employeeList);

                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }
    }
}
