using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
    {
        public async Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.DateOfBirth = entity.DateOfBirth.ToLocalTime();
            entity.EmploymentDate = entity.EmploymentDate.ToLocalTime();
            entity.MedicalCheckValidUntil = entity.MedicalCheckValidUntil.ToLocalTime();
            dbContext.Employees.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<EmployeeListsEntity> AddEmployeeListsAsync(EmployeeListsEntity entity, List<Guid> employeeIds)
        {
            entity.Id = Guid.NewGuid();
            dbContext.EmployeeLists.Add(entity);

            var employees = await dbContext.Employees.Where(x => employeeIds.Contains(x.Id)).ToListAsync();

            entity.EmployeesEntities = employees;

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<EmployeeEntity> GetEmployeeByIdAsync(Guid id)
        {
            return await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<EmployeeEntity>> GetUserEmployeesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, string sortOrder)
        {
            var query = dbContext.Employees.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "salary_asc":
                        query = query.OrderBy(x => x.Salary);
                        break;
                    case "salary_desc":
                        query = query.OrderByDescending(x => x.Salary);
                        break;
                    case "birthDate_asc":
                        query = query.OrderBy(x => x.DateOfBirth);
                        break;
                    case "birthDate_desc":
                        query = query.OrderByDescending(x => x.DateOfBirth);
                        break;
                    case "employmentDate_asc":
                        query = query.OrderBy(x => x.EmploymentDate);
                        break;
                    case "employmentDate_desc":
                        query = query.OrderByDescending(x => x.EmploymentDate);
                        break;
                    case "medicalCheckValidUntil_asc":
                        query = query.OrderBy(x => x.MedicalCheckValidUntil);
                        break;
                    case "medicalCheckValidUntil_desc":
                        query = query.OrderByDescending(x => x.MedicalCheckValidUntil);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.EmploymentDate);
                        break;
                }
            }

            return await PaginatedList<EmployeeEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PaginatedList<EmployeeEntity>> GetAllEmployeesAsync(int pageNumber, int pageSize, string searchTerm, string sortOrder)
        {
            var query = dbContext.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "salary_asc":
                        query = query.OrderBy(x => x.Salary);
                        break;
                    case "salary_desc":
                        query = query.OrderByDescending(x => x.Salary);
                        break;
                    case "birthDate_asc":
                        query = query.OrderBy(x => x.DateOfBirth);
                        break;
                    case "birthDate_desc":
                        query = query.OrderByDescending(x => x.DateOfBirth);
                        break;
                    case "employmentDate_asc":
                        query = query.OrderBy(x => x.EmploymentDate);
                        break;
                    case "employmentDate_desc":
                        query = query.OrderByDescending(x => x.EmploymentDate);
                        break;
                    case "medicalCheckValidUntil_asc":
                        query = query.OrderBy(x => x.MedicalCheckValidUntil);
                        break;
                    case "medicalCheckValidUntil_desc":
                        query = query.OrderByDescending(x => x.MedicalCheckValidUntil);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.EmploymentDate);
                        break;
                }
            }

            return await PaginatedList<EmployeeEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.EmployeeLists.Include(x => x.EmployeesEntities).Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsForTaskAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.EmployeeLists.Include(x => x.EmployeesEntities).Where(x => x.AppUserId == appUserId && x.TaskId == null);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeEntity>> GetUserEmployeesForListAddAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.Employees.Where(x => x.AppUserId == appUserId && x.EmployeeListId == null);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeEntity>> GetUserEmployeesForListUpdateAsync(string appUserId, Guid employeeListId, string searchTerm)
        {
            var query = dbContext.Employees.Where(x => x.AppUserId == appUserId && (x.EmployeeListId == null || x.EmployeeListId == employeeListId));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> EmployeeListExistsForAddAsync(string name, string appUserId)
        {
            return await dbContext.EmployeeLists.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.AppUserId == appUserId);
        }

        public async Task<bool> EmployeeListExistsForUpdateAsync(string name, string appUserId, Guid employeeListId)
        {
            return await dbContext.EmployeeLists.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.AppUserId == appUserId && x.Id != employeeListId);
        }

        public async Task<int> GetUserNumberOfEmployeesAsync(string appUserId)
        {
            return await dbContext.Employees.Where(x => x.AppUserId == appUserId).CountAsync();
        }

        public async Task<int> GetNumberOfEmployeesAsync()
        {
            return await dbContext.Employees.CountAsync();
        }

        public async Task<EmployeeEntity> UpdateEmployeeAsync(Guid employeeId, string appUserId, EmployeeEntity entity)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId && x.AppUserId == appUserId);

            if (employee is not null)
            {
                employee.Name = entity.Name;
                employee.Email = entity.Email;
                employee.Phone = entity.Phone;
                employee.Salary = entity.Salary;
                employee.DateOfBirth = entity.DateOfBirth.ToLocalTime();
                employee.EmploymentDate = entity.EmploymentDate.ToLocalTime();
                employee.MedicalCheckValidUntil = entity.MedicalCheckValidUntil.ToLocalTime();

                await dbContext.SaveChangesAsync();

                return employee;
            }

            return entity;
        }

        public async Task<EmployeeListsEntity> UpdateEmployeeListAsync(Guid employeeListId, string appUserId, EmployeeListsEntity entity, List<Guid> employeeIds)
        {
            var employeeList = await dbContext.EmployeeLists.Include(x => x.EmployeesEntities).FirstOrDefaultAsync(x => x.Id == employeeListId && x.AppUserId == appUserId);

            if (employeeList is not null)
            {
                employeeList.Name = entity.Name;

                employeeList.EmployeesEntities.Clear();

                var employees = await dbContext.Employees.Where(x => employeeIds.Contains(x.Id)).ToListAsync();

                foreach (var employee in employees)
                {
                    employeeList.EmployeesEntities.Add(employee);
                }

                await dbContext.SaveChangesAsync();

                return employeeList;
            }

            return entity;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid employeeId, string appUserId)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId && x.AppUserId == appUserId);

            if (employee is not null)
            {
                dbContext.Employees.Remove(employee);

                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> DeleteEmployeeListsAsync(Guid employeeListId, string appUserId)
        {
            var employeeList = await dbContext.EmployeeLists.FirstOrDefaultAsync(x => x.Id == employeeListId && x.AppUserId == appUserId);

            if (employeeList is not null)
            {
                var employees = await dbContext.Employees.Where(x => x.EmployeeListId == employeeListId).ToListAsync();

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