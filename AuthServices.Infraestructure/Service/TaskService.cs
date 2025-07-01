using AuthServices.Application.DTOs;
using AuthServices.Application.Exceptions;
using AuthServices.Application.Interface;
using AuthServices.Application.Model;
using AuthServices.Application.Model.Task;
using AuthServices.Application.Model.User;
using AuthServices.Application.Utils;
using AuthServices.Domain.Entities;
using AuthServices.Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Infraestructure.Service
{
    public class TaskService:ITaskService
    {
        public readonly AuthDbContext _context;
        public readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TaskService> _logger;


        public TaskService(AuthDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<TaskService> logger)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }



        public async Task<TaskResponse> CreatedTask(TaskRequest request)
        {
            _logger.LogInformation("{Proceso} {Descripcion}", "Task", "Created");



            if (string.IsNullOrWhiteSpace(request.Title))
                throw new AuthenticationException(ResponseMessage.TasktitleRequired);


            var Taskxist = await _context.Task
                .FirstOrDefaultAsync(u => u.Title == request.Title);

            if (Taskxist != null)
                throw new RequestException(ResponseMessage.TaskExist);



            var TaskID = Guid.NewGuid();

            await _context.Task.AddAsync(new Domain.Entities.Task
            {
                TaskId = TaskID,
                Title = request.Title,
                Description  = request.Description,
                Status = request.Status,
                DueDate= request.DueDate,
                Priority=request.Priority,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy
            });

            await _context.SaveChangesAsync();

            return new TaskResponse
            {
                TaskID = TaskID
            };
        }


        public async Task<TaskResponse> UpdateTask(UpdateTaskRequest request)
        {
            _logger.LogInformation("{Proceso} {Descripcion}", "Task", "Update");

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new AuthenticationException(ResponseMessage.TasktitleRequired);


            var task = await _context.Task
                .FirstOrDefaultAsync(u => u.TaskId == request.TaskId);

            if (task == null)
                throw new RequestException(ResponseMessage.TaskNotFound);



            // Actualizar campos
            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;
            task.DueDate = request.DueDate;
            task.Priority = request.Priority;
            task.AssignedTo = request.AssignedTo;
            task.UpdatedAt = DateTime.Now;
            task.UpdatedBy = request.UpdatedBy;
            

            _context.Task.Update(task);
            await _context.SaveChangesAsync();

            return new TaskResponse
            {
                TaskID = task.TaskId,
            };
        }


        public async Task<PageResult<TaskListResponse>> GetTasksAsync(ListDtos queryParams)
        {
            try
            {
                var query = _context.Task
                    .Include(u => u.user)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchLower = queryParams.Search.ToLower();
                    query = query.Where(u =>
                        u.Title.ToLower().Contains(searchLower) ||
                        (u.Status != null && u.Status.ToLower().Contains(searchLower)) ||
                        u.Priority.Contains(searchLower)
                    );
                }

                if (!string.IsNullOrEmpty(queryParams.SortBy))
                {
                    query = queryParams.SortBy.ToLower() switch
                    {
                        "priority" => queryParams.SortDescending ? query.OrderByDescending(u => u.Priority) : query.OrderBy(u => u.Priority),
                        "title" => queryParams.SortDescending ? query.OrderByDescending(u => u.Title) : query.OrderBy(u => u.Title),
                        _ => query.OrderBy(u => u.Priority)
                    };
                }
                else
                {
                    query = query.OrderBy(u => u.Priority);
                }

                var totalRecords = await query.CountAsync();

                var tasks = await query
                    .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize)
                    .Select(u => new TaskListResponse
                    {
                        TaskId = u.TaskId,
                        Title = u.Title,
                        Description = u.Description,
                        AssignedTo = u.AssignedTo,
                        FullName=u.user.FirstName +" "+ u.user.LastName,
                        Status = u.Status,
                        DueDate = u.DueDate,
                        Priority = u.Priority,
                        CreatedAt = u.CreatedAt,
                        CreatedBy = u.CreatedBy,
                        UpdatedAt = u.UpdatedAt,
                        UpdatedBy = u.UpdatedBy,
                    })
                    .ToListAsync();

                return new PageResult<TaskListResponse>
                {
                    TotalRecords = totalRecords,
                    PageNumber = queryParams.PageNumber,
                    PageSize = queryParams.PageSize,
                    Data = tasks
                };
            }
            catch (Exception ex)
            {
                

                throw new RequestException("Error retrieving tasks");
            }
        }


        public async Task<PageResult<TaskListResponse>> GetTaskByUserIdAsync(ListUsetDtos queryParams)
        {
            try
            {
                var query = _context.Task
                    .Where(u=>u.AssignedTo== queryParams.UserId)
                    .Include(u=>u.user)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchLower = queryParams.Search.ToLower();
                    query = query.Where(u =>
                        u.Title.ToLower().Contains(searchLower) ||
                        (u.Status != null && u.Status.ToLower().Contains(searchLower)) ||
                        u.Priority.Contains(searchLower)
                    );
                }

                if (!string.IsNullOrEmpty(queryParams.SortBy))
                {
                    query = queryParams.SortBy.ToLower() switch
                    {
                        "priority" => queryParams.SortDescending ? query.OrderByDescending(u => u.Priority) : query.OrderBy(u => u.Priority),
                        "title" => queryParams.SortDescending ? query.OrderByDescending(u => u.Title) : query.OrderBy(u => u.Title),
                        _ => query.OrderBy(u => u.Priority)
                    };
                }
                else
                {
                    query = query.OrderBy(u => u.Priority);
                }

                var totalRecords = await query.CountAsync();

                var tasks = await query
                    .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize)
                    .Select(u => new TaskListResponse
                    {
                        TaskId = u.TaskId,
                        Title = u.Title,
                        Description = u.Description,
                        AssignedTo = u.AssignedTo,
                        Status = u.Status,
                        FullName = u.user.FirstName + " " + u.user.LastName,
                        DueDate = u.DueDate,
                        Priority = u.Priority,
                        CreatedAt = u.CreatedAt,
                        CreatedBy = u.CreatedBy,
                        UpdatedAt = u.UpdatedAt,
                        UpdatedBy = u.UpdatedBy,
                    })
                    .ToListAsync();

                return new PageResult<TaskListResponse>
                {
                    TotalRecords = totalRecords,
                    PageNumber = queryParams.PageNumber,
                    PageSize = queryParams.PageSize,
                    Data = tasks
                };
            }
            catch (Exception ex)
            {


                throw new RequestException("Error retrieving tasks");
            }
        }



        public async Task<TaskListResponse> GetTaskByIdAsync(Guid TaskId)
        {
            try
            {
                var Task = await _context.Task
                    .Where(u => u.TaskId == TaskId)
                    .Include(u => u.user)
                    .Select(u => new TaskListResponse
                    {
                        TaskId = u.TaskId,
                        Title = u.Title,
                        Description = u.Description,
                        AssignedTo = u.AssignedTo,
                        FullName = u.user.FirstName + " " + u.user.LastName,
                        Status = u.Status,
                        DueDate = u.DueDate,
                        Priority = u.Priority,
                        CreatedAt = u.CreatedAt,
                        CreatedBy = u.CreatedBy,
                        UpdatedAt = u.UpdatedAt,
                        UpdatedBy = u.UpdatedBy,
                    })
                    .FirstOrDefaultAsync();

                if (Task == null)
                    throw new RequestException(ResponseMessage.TaskNotFound);

                return Task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving the task by ID.");
                throw new RequestException(ex.Message);

            }
        }

        public async Task<bool> DeleteTaskAsync(Guid TaskId)
        {
            var task = await _context.Task.FirstOrDefaultAsync(u => u.TaskId == TaskId);

            if (task == null)
                throw new RequestException(ResponseMessage.UserNotFound);
            // Desactivación del usuario (eliminación lógica)
            task.Status = "Deleted";
            task.UpdatedAt = DateTime.UtcNow;

          
           
            _context.Task.Update(task);
            await _context.SaveChangesAsync();

            return true;
        }

        
    }
}
