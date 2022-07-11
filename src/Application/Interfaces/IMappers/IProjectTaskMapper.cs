using Domain.Entities;
using Application.DTO;

namespace Application.Interfaces.IMappers
{
    public interface IProjectTaskMapper
    {
        ProjectTask MapDtoToProjectTask(ProjectTaskDto projectTaskDto);
        ProjectTaskDto MapProjectTaskToDto(ProjectTask projectTask);
    }
}
