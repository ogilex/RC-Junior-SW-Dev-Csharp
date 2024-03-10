using System.ComponentModel.DataAnnotations;

namespace Csharp_task.Models
{
    public class EmployeeModel
    {
        public required string Id { get; set; }
        [Required]
        public required string EmployeeName { get; set; }
        [Required]
        public required string StarTimeUtc { get; set; }
        [Required]
        public required string EndTimeUtc { get; set; }
        [Required]
        public string? EntryNotes { get; set; }

    }
}
