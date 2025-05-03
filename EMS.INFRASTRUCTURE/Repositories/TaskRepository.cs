using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using EMS.INFRASTRUCTURE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class TaskRepository(AppDbContext dbContext) : ITaskRepository
    {
        public async Task<PaginatedList<TaskEntity>> GetUserTasksAsync(string appUserId, int pageNumber, int pageSize, string searchTerm)
        {
            var query = dbContext.Tasks.Include(x => x.AddressEntity).Include(x => x.EmployeeListsEntities).ThenInclude(x => x.EmployeesEntities).AsQueryable();

            query = query.OrderByDescending(x => x.EndDate).Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())
                                      || x.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            return await PaginatedList<TaskEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<TaskEntity> GetTaskByIdAsync(Guid id)
        {
            return await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskEntity> AddTaskAsync(TaskEntity entity, List<Guid> EmployeeListIds)
        { 
            entity.Id = Guid.NewGuid(); // Przypisanie nowego identyfikatora

            var employeeLists = await dbContext.EmployeeLists.Where(x => EmployeeListIds.Contains(x.Id)).ToListAsync();

            foreach (var item in employeeLists)
            {
                item.TaskId = entity.Id;
            }
            entity.StartDate = entity.StartDate.ToLocalTime();
            entity.EndDate = entity.EndDate.ToLocalTime();

            dbContext.Tasks.Add(entity);

            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            var employeeList = await dbContext.EmployeeLists.Where(x =>x.TaskId == taskId).ToListAsync();

            if (task is not null)
            {
                foreach (var item in employeeList) 
                {
                    item.TaskId = null;
                }

                dbContext.Tasks.Remove(task);
                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> UpdateTaskStatusAsync(Guid taskId, StatusOfTask newStatus)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (task is not null)
            {
                var employeeListsEntities = await dbContext.EmployeeLists.Where(x => x.TaskId == taskId).ToListAsync();

                foreach (var item in employeeListsEntities)
                {
                    item.TaskId = null;
                }

                task.Status = newStatus;

                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<TaskEntity> UpdateTaskAsync(Guid id, TaskEntity entity)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task is not null)
            {
                task.Name = entity.Name;
                task.Description = entity.Description;

                await dbContext.SaveChangesAsync();

                return task;
            }

            return entity;
        }

        public async Task<PaginatedList<TaskEntity>> GetAllTasksAsync(int pageNumber, int pageSize, string searchTerm)
        {
            var query = dbContext.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return await PaginatedList<TaskEntity>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
