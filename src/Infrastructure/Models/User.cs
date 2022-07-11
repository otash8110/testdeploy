using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        public Role Role { get; set; }

        [InverseProperty("Creator")]
        public ICollection<ProjectTask> CreatorTasks { get; set; }
        [InverseProperty("Performer")]
        public ICollection<ProjectTask> PerformerTasks { get; set; }
    }
}
