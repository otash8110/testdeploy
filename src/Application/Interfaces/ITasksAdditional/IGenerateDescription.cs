using Domain.Entities;

namespace Application.Interfaces.ITasksAdditional
{
    public interface IGenerateDescription
    {
        DateTime currentDateTime { get; }
        Task<string> GenerateDescription(ProjectTask task);
    }
}
