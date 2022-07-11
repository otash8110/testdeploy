namespace Domain.Entities
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Statuses Status { get; set; }
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public int? PerformerId { get; set; }
        public virtual User Performer { get; set; }
    }
}
