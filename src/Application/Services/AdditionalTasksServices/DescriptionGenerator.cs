using Application.Interfaces.ITasksAdditional;
using Application.Interfaces.IServices;
using Domain.Entities;

namespace Application.Services.AdditionalTasksServices
{
    public class DescriptionGenerator : IGenerateDescription
    {
        private readonly IUsersService _usersService;
        public DescriptionGenerator(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public DateTime currentDateTime => DateTime.Now;

        public async Task<string> GenerateDescription(ProjectTask task)
        {
            string descriptionEnd = "";
            descriptionEnd = $" Creator: { await _usersService.GetUserFullName(task.CreatorId)}." +
                    $" Created: {currentDateTime}. ";

            if ((task.PerformerId != 0) && task.PerformerId != null)
            {
                descriptionEnd += $"Performer: { await _usersService.GetUserFullName((int) task.PerformerId)}";
            } else
            {
                descriptionEnd += "No performer";
            }
            string finalDescription = task.Description + descriptionEnd;
            return finalDescription;
        }
    }
}
