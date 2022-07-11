using Application.Interfaces.IMappers;
using Application.DTO;
using Domain.Entities;

namespace Application.Services.MappersServices
{
    public class ProjectTaskMapper : IProjectTaskMapper
    {
        public ProjectTask MapDtoToProjectTask(ProjectTaskDto projectTaskDto)
        {
            var performerId = projectTaskDto.PerformerId == 0 ? null : projectTaskDto.PerformerId;
            return new ProjectTask()
            {
            Id = projectTaskDto.Id,
                Name = projectTaskDto.Name,
                Description = projectTaskDto.Description,
                Status = projectTaskDto.Status,
                CreatorId = projectTaskDto.CreatorId,
                PerformerId = performerId,
            };
        }

        public ProjectTaskDto MapProjectTaskToDto(ProjectTask projectTask)
        {
            var performerId = projectTask.PerformerId == 0 ? null : projectTask.PerformerId;
            return new ProjectTaskDto()
            {
                Id = projectTask.Id,
                Name = projectTask.Name,
                Description = projectTask.Description,
                Status = projectTask.Status,
                CreatorId = projectTask.CreatorId,
                PerformerId = performerId,
            };
        }
    }
}
