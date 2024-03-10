namespace Csharp_task.Models
{
    public class EmployeeDto : IComparable<EmployeeDto>
    {
        public string EmployeeName { get; set; }
        public long HoursWorked { get; set; }

        public EmployeeDto(string empName, long hrsWorked) 
        {
            EmployeeName = empName;
            HoursWorked = hrsWorked;
        }

        public int CompareTo(EmployeeDto? other)

        {
            return other.HoursWorked.CompareTo(this.HoursWorked);
        }
    }
}
