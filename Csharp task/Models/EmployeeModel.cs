using System.ComponentModel.DataAnnotations;

namespace Csharp_task.Models
{
    public class EmployeeModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string StarTimeUtc { get; set; }
        [Required]
        public string EndTimeUtc { get; set; }
        [Required]
        public string EntryNotes { get; set; }

    }
}
