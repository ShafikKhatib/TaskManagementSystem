using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]  // Marks this property as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-incrementing
        public int EmployeeId { get; set; }

        [Required]  // Marks this field as NOT NULL
        [MaxLength(255)]  // VARCHAR(255) equivalent
        public string Name { get; set; }

        [Required]  // Marks this field as NOT NULL
        [MaxLength(255)]  // VARCHAR(255)
        [EmailAddress]  // Ensures that the data is in valid email format
        public string Email { get; set; }

        [Required]  // Marks this field as NOT NULL
        [MaxLength(50)]  // VARCHAR(50)
        public string Role { get; set; }  // Can be "Employee", "Manager", "Admin"

        // Foreign Key - TeamId

        public int? TeamId { get; set; }  // Foreign key (can be nullable, so `int?`)

        [ForeignKey("TeamId")]
        [JsonIgnore]
        public Team? Team { get; set; }  // Navigation property to Team
        
        [JsonIgnore]
        [BindNever]
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
