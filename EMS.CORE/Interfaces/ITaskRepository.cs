using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetUserTasksAsync(string appUserId);
        Task<TaskEntity> GetTaskByIdAsync(Guid id);
        Task<TaskEntity> AddTaskAsync(TaskEntity entity);
        Task<TaskEntity> UpdateTaskAsync(Guid taskId, TaskEntity entity);
        Task<bool> DeleteTaskAsync(Guid taskId);
    }
}
