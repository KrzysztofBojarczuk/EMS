using EMS.CORE.Entities;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity entity);
        Task<EmployeeListsEntity> AddEmployeeListsAsync(EmployeeListsEntity entity, List<Guid> employeeIds);
        Task<EmployeeEntity> GetEmployeeByIdAsync(Guid id);
        Task<PaginatedList<EmployeeEntity>> GetUserEmployeesAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, string sortOrder);
        Task<PaginatedList<EmployeeEntity>> GetAllEmployeesAsync(int pageNumber, int pageSize, string searchTerm, string sortOrder);
        Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsForTaskAddAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeListsEntity>> GetUserEmployeeListsForTaskUpdateAsync(string appUserId, Guid taskId, string searchTerm);
        Task<IEnumerable<EmployeeEntity>> GetUserEmployeesForListAddAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeEntity>> GetUserEmployeesForListUpdateAsync(string appUserId, Guid employeeListId, string searchTerm);
        Task<bool> EmployeeListExistsForAddAsync(string name, string appUserId);
        Task<bool> EmployeeListExistsForUpdateAsync(string name, string appUserId, Guid employeeListId);
        Task<int> GetUserNumberOfEmployeesAsync(string appUserId);
        Task<int> GetNumberOfEmployeesAsync();
        Task<EmployeeEntity> UpdateEmployeeAsync(Guid employId, string appUserId, EmployeeEntity entity);
        Task<EmployeeListsEntity> UpdateEmployeeListAsync(Guid employeeListId, string appUserId, EmployeeListsEntity entity, List<Guid> employeeIds);
        Task<bool> DeleteEmployeeAsync(Guid employeeId, string appUserId);
        Task<bool> DeleteEmployeeListsAsync(Guid employeeListId, string appUserId);
    }
}