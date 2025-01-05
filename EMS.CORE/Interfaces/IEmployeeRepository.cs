using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<int> GetNumberOfEmployeesAsync();
        Task<int> GetUserNumberOfEmployeesAsync(string appUserId);
        Task<IEnumerable<EmployeeEntity>> GetUserEmployeesAsync(string appUserId, string searchTerm);
        Task<IEnumerable<EmployeeEntity>> GetEmployeesAsync(string searchTerm);
        Task<EmployeeEntity> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity entity);
        Task<EmployeeEntity> UpdateEmployeeAsync(Guid employId, EmployeeEntity entity);
        Task<bool> DeleteEmployeeAsync(Guid taskId);
    }
}
