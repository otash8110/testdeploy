using Domain.Entities;
using Application.DTO;

namespace Application.Interfaces.IServices
{
    public interface ITasksService
    {
        Task<List<ProjectTaskDto>> GetAllTasksAsync();
        Task CreateTaskAsync(string name, string description, Statuses status, int creatorId, int performerId = 0);
        Task CreateTaskAsync(ProjectTaskDto taskDto);
        Task<ProjectTaskDto> GetTaskByIdAsync(int id);
        Task UpdateTaskAsync(string name, string description, Statuses status, int creatorId, int performerId = 0);
        Task UpdateTaskAsync(ProjectTaskDto projectTaskDto);
        Task DeleteTaskAsync(int id);
        List<ProjectTaskDto> MapDtoToProjectTasksList(List<ProjectTask> tasks);
    }
}
