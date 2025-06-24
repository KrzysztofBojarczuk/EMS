using EMS.CORE.Common;
using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<int> GetNumberOfEmployeesAsync();
        Task<int> GetUserNumberOfEmployeesAsync(string appUserId);
        Task<PaginatedList<EmployeeEntity>> GetUserEmployeesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm);
        Task<PaginatedList<EmployeeEntity>> GetEmployeesAsync(int pageNumber, int pageSize, string searchTerm);
        Task<EmployeeEntity> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity entity);
        Task<EmployeeEntity> UpdateEmployeeAsync(Guid employId, EmployeeEntity entity);
        Task<bool> DeleteEmployeeAsync(Guid employeeId);
        Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeListsEntity>>  GetUserEmployeeListsForTaskAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeEntity>> GetUserEmployeesForListAsync(string appUserId, string searchTerm);
        Task<Result<EmployeeListsEntity>> AddEmployeeListsAsync(EmployeeListsEntity entity, List<Guid> employeeIds);
        Task<bool> DeleteEmployeeListsAsync(Guid employeeListId);
    }
}
