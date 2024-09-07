using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Models
{
    [Table("TaskNotes")]
    public class TaskNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Key
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        [JsonIgnore]
        public TaskItem? TaskItem { get; set; }
    }
}
