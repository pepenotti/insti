namespace Insti.Data.Models
{
    public class Attendance : BaseEntity
    {
        public DateTime Date { get; set; }

        public int AttendanceStatusId { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
