using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    [Table("Team")]
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(255)]
        public string TeamName { get; set; }

        // Navigation Property
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
