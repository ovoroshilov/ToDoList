using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Extensions;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Service.Interfaces;
using ToDoListDAL.Intertfaces;

namespace ToDoList.Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _baseRepository;
        private ILogger<TaskService> _logger;

        public TaskService(IBaseRepository<TaskEntity> baseRepository, ILogger<TaskService> logger)
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }

        public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel taskViewModel)
        {
            try
            {
                taskViewModel.Validate();

                _logger.LogInformation("Task create query - " + taskViewModel.Name);

                var task = await _baseRepository.GetAll()
                    .Where(x => x.Created.Date == DateTime.Today)
                    .FirstOrDefaultAsync(x => x.Name == taskViewModel.Name);
                if (task != null)
                {
                    return new BaseResponse<TaskEntity>
                    {
                        Description = "Task with this name already exists",
                        StatusCode = StatusCode.TaskAlredyExists,

                    };
                }

                task = new TaskEntity()
                {
                    Name = taskViewModel.Name,
                    Description = taskViewModel.Description,
                    Priority = taskViewModel.Priority,
                    Created = DateTime.Now,
                    IsDone = false
                };
                await _baseRepository.Create(task);

                _logger.LogInformation("Task has been created: " + task.Name + task.Created);
                return new BaseResponse<TaskEntity>
                {
                    Description = "Task has been created",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");

                return new BaseResponse<TaskEntity>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<bool>> EndDay()
        {
            try
            {
                foreach (var task in await _baseRepository.GetAll().ToListAsync())
                {
                    if(task.IsDone) await _baseRepository.Delete(task);
                }
 
                return new BaseResponse<bool>()
                {
                    Description = "You successfully finished the day",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"[TaskService.EndDay]: {ex.Message}");

                return new BaseResponse<bool>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> EndTask(int taskId)
        {
            try
            {
                var task = await _baseRepository.GetAll().FirstOrDefaultAsync(x => x.Id == taskId);
                if (task == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Task is not found",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }

                task.IsDone = true;

                await _baseRepository.Update(task);

                return new BaseResponse<bool>()
                {
                    Description = "Task ended",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"[TaskService.EndTask]: {ex.Message}");

                return new BaseResponse<bool>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetCompletedTasks()
        {
            try
            {
                var tasks = await _baseRepository.GetAll()
                    .Where(x => x.IsDone)
                    .Where(x => x.Created.Date == DateTime.Today)
                    .Select(x => new TaskViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,  
                        Description = x.Description.Substring(0, x.Description.Length),

                    }).ToListAsync();

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Data = tasks,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.GetCompletedTasks]: {ex.Message}");

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    StatusCode = StatusCode.InternalServerError
                };

            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks()
        {
            try
            {
                var tasks = await _baseRepository.GetAll()
                    .Where(x => !x.IsDone)
                    .Select(x => new TaskViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsDone = x.IsDone == true ? "Is done" : "Is not done",
                        Priority = x.Priority.GetDisplayName(),
                        Created = x.Created.ToLongDateString()
                    })
                    .ToListAsync();
                return new BaseResponse<IEnumerable<TaskViewModel>>()
                {
                    Data = tasks,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"[TaskService.GetAllTasks]: {ex.Message}");

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}


