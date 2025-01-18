using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using EMS.INFRASTRUCTURE.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.INFRASTRUCTURE.Repositories
{
    public class TaskRepository(AppDbContext dbContext) : ITaskRepository
    {
        public async Task<IEnumerable<TaskEntity>> GetUserTasksAsync(string appUserId, string searchTerm)
        {
            var query = dbContext.Tasks.AsQueryable();

            query = query.Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())
                                      || x.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<TaskEntity> GetTaskByIdAsync(Guid id)
        {
            return await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskEntity> AddTaskAsync(TaskEntity entity)
        {
            entity.Id = Guid.NewGuid(); // Przypisanie nowego identyfikatora
            dbContext.Tasks.Add(entity);

            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

            if (task is not null)
            {
                dbContext.Tasks.Remove(task);
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
    }
}
