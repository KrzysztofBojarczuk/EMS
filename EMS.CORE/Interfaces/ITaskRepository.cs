using EMS.CORE.Entities;
using EMS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetUserTasksAsync(string appUserId, string searchTerm);
        Task<TaskEntity> GetTaskByIdAsync(Guid id);
        Task<TaskEntity> AddTaskAsync(TaskEntity entity, List<Guid> employeeListIds);
        Task<TaskEntity> UpdateTaskAsync(Guid taskId, TaskEntity entity);
        Task<bool> UpdateTaskStatusAsync(Guid taskId, StatusOfTask Status); 
        Task<bool> DeleteTaskAsync(Guid taskId);
    }
}
