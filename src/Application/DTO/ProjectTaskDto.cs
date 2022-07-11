using Domain.Entities;

namespace Application.DTO
{
    public class ProjectTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Statuses Status { get; set; }
        public int CreatorId { get; set; }
        public int? PerformerId { get; set; }
    }
}
