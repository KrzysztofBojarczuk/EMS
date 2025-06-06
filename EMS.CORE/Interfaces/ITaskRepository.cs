﻿using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.INFRASTRUCTURE.Extensions;

namespace EMS.CORE.Interfaces
{
    public interface ITaskRepository
    {
        Task<PaginatedList<TaskEntity>> GetUserTasksAsync(string appUserId, int pageNumber, int pageSize, string searchTerm);
        Task<TaskEntity> GetTaskByIdAsync(Guid id);
        Task<TaskEntity> AddTaskAsync(TaskEntity entity, List<Guid> employeeListIds);
        Task<TaskEntity> UpdateTaskAsync(Guid taskId, TaskEntity entity);
        Task<bool> UpdateTaskStatusAsync(Guid taskId, StatusOfTask Status); 
        Task<bool> DeleteTaskAsync(Guid taskId);
        Task<PaginatedList<TaskEntity>> GetAllTasksAsync(int pageNumber, int pageSize, string searchTerm);
    }
}
