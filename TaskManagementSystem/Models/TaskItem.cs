using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Models
{
    [Table("TaskItem")]
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        // Foreign Key
        [Required]
        public int EmployeeId { get; set; }

        // Navigation Property
        [JsonIgnore]
        public Employee? Employee { get; set; }

        [JsonIgnore]
        public List<TaskNotes> TaskNotes { get; set; } = new List<TaskNotes>();
    }
}
