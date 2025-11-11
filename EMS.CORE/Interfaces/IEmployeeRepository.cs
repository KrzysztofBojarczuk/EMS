using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<int> GetNumberOfEmployeesAsync();
        Task<int> GetUserNumberOfEmployeesAsync(string appUserId);
        Task<PaginatedList<EmployeeEntity>> GetUserEmployeesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, string sortOrderSalary);
        Task<PaginatedList<EmployeeEntity>> GetEmployeesAsync(int pageNumber, int pageSize, string searchTerm);
        Task<EmployeeEntity> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity entity);
        Task<EmployeeEntity> UpdateEmployeeAsync(Guid employId, string appUserId, EmployeeEntity entity);
        Task<bool> DeleteEmployeeAsync(Guid employeeId, string appUserId);
        Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeListsEntity>>  GetUserEmployeeListsForTaskAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeEntity>> GetUserEmployeesForListAsync(string appUserId, string searchTerm);
        Task<EmployeeListsEntity> AddEmployeeListsAsync(EmployeeListsEntity entity, List<Guid> employeeIds);
        Task<bool> EmployeeListExistsAsync(string name, string appUserId);
        Task<bool> DeleteEmployeeListsAsync(Guid employeeListId, string appUserId);
    }
}