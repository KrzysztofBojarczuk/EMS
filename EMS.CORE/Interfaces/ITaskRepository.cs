using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskEntity> AddTaskAsync(TaskEntity entity, List<Guid> employeeListIds, List<Guid> vehicleIds);
        Task<TaskEntity> GetTaskByIdAsync(Guid id);
        Task<PaginatedList<TaskEntity>> GetUserTasksAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, List<StatusOfTask> statusOfTask, string sortOrder);
        Task<PaginatedList<TaskEntity>> GetAllTasksAsync(int pageNumber, int pageSize, string searchTerm);
        Task<TaskEntity> UpdateTaskAsync(Guid taskId, string appUserId, TaskEntity entity);
        Task<bool> UpdateTaskStatusAsync(Guid taskId, string appUserId, StatusOfTask Status);
        Task<bool> DeleteTaskAsync(Guid taskId, string appUserId);
    }
}