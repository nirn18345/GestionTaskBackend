using AuthServices.Application.DTOs;
using AuthServices.Application.Model;
using AuthServices.Application.Model.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Interface
{
    public interface ITaskService
    {
        public Task<TaskResponse> CreatedTask(TaskRequest request);

       

        public Task<TaskResponse> UpdateTask(UpdateTaskRequest request);

        Task<PageResult<TaskListResponse>> GetTasksAsync(ListDtos queryParams);

        Task<TaskListResponse> GetTaskByIdAsync(Guid TaskID);
        Task<PageResult<TaskListResponse>> GetTaskByUserIdAsync(ListUsetDtos queryParams);

        Task<bool> DeleteTaskAsync(Guid TaskId);
    }
}
