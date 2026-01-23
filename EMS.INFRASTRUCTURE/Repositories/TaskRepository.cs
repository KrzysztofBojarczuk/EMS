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
        public async Task<TaskEntity> AddTaskAsync(TaskEntity entity, List<Guid> EmployeeListIds, List<Guid> VehicleIds)
        {
            entity.Id = Guid.NewGuid();
            entity.StartDate = entity.StartDate.ToLocalTime();
            entity.EndDate = entity.EndDate.ToLocalTime();

            var employeeLists = await dbContext.EmployeeLists.Where(x => EmployeeListIds.Contains(x.Id)).ToListAsync();

            foreach (var item in employeeLists)
            {
                item.TaskId = entity.Id;
            }

            var vehicles = await dbContext.Vehicles.Where(x => VehicleIds.Contains(x.Id)).ToListAsync();

            foreach (var item in vehicles)
            {
                item.TaskId = entity.Id;
            }


            dbContext.Tasks.Add(entity);

            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TaskEntity> GetTaskByIdAsync(Guid id)
        {
            return await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<TaskEntity>> GetUserTasksAsync(string appUserId, int pageNumber, int pageSize, string searchTerm, List<StatusOfTask> statusOfTask, string sortOrder)
        {
            var query = dbContext.Tasks.Include(x => x.AddressEntity).Include(x => x.EmployeeListsEntities).ThenInclude(x => x.EmployeesEntities).Include(x => x.VehicleEntities)
                                       .Where(x => x.AppUserId == appUserId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())
                                      || x.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            if (statusOfTask != null && statusOfTask.Any())
            {
                query = query.Where(x => statusOfTask.Contains(x.Status));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "start_asc":
                        query = query.OrderBy(x => x.StartDate);
                        break;
                    case "start_desc":
                        query = query.OrderByDescending(x => x.StartDate);
                        break;
                    case "end_asc":
                        query = query.OrderBy(x => x.EndDate);
                        break;
                    case "end_desc":
                        query = query.OrderByDescending(x => x.EndDate);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.EndDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.EndDate);
            }

            return await PaginatedList<TaskEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PaginatedList<TaskEntity>> GetAllTasksAsync(int pageNumber, int pageSize, string searchTerm, List<StatusOfTask> statusOfTask, string sortOrder)
        {
            var query = dbContext.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (statusOfTask != null && statusOfTask.Any())
            {
                query = query.Where(x => statusOfTask.Contains(x.Status));
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "start_asc":
                        query = query.OrderBy(x => x.StartDate);
                        break;
                    case "start_desc":
                        query = query.OrderByDescending(x => x.StartDate);
                        break;
                    case "end_asc":
                        query = query.OrderBy(x => x.EndDate);
                        break;
                    case "end_desc":
                        query = query.OrderByDescending(x => x.EndDate);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.EndDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.EndDate);
            }

            return await PaginatedList<TaskEntity>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<TaskEntity> UpdateTaskAsync(Guid id, string appUserId, TaskEntity entity)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id && x.AppUserId == appUserId);

            if (task is not null)
            {
                task.Name = entity.Name;
                task.Description = entity.Description;

                await dbContext.SaveChangesAsync();

                return task;
            }

            return entity;
        }

        public async Task<bool> UpdateTaskStatusAsync(Guid taskId, string appUserId, StatusOfTask newStatus)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId && x.AppUserId == appUserId);

            if (task is not null)
            {
                var employeeListsEntities = await dbContext.EmployeeLists.Where(x => x.TaskId == taskId && x.AppUserId == appUserId).ToListAsync();

                foreach (var item in employeeListsEntities)
                {
                    item.TaskId = null;
                }

                var vehiclesEntities = await dbContext.Vehicles.Where(x => x.TaskId == taskId && x.AppUserId == appUserId).ToListAsync();

                foreach (var item in vehiclesEntities)
                {
                    item.TaskId = null;
                }

                task.Status = newStatus;

                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId, string appUserId)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId && x.AppUserId == appUserId);

            if (task is not null)
            {
                var employeeListsEntities = await dbContext.EmployeeLists.Where(x => x.TaskId == taskId && x.AppUserId == appUserId).ToListAsync();

                foreach (var item in employeeListsEntities)
                {
                    item.TaskId = null;
                }

                var vehiclesEntities = await dbContext.Vehicles.Where(x => x.TaskId == taskId && x.AppUserId == appUserId).ToListAsync();

                foreach (var item in vehiclesEntities)
                {
                    item.TaskId = null;
                }

                dbContext.Tasks.Remove(task);
                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }
    }
}