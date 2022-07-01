namespace Insti.Data.Models
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Student> Students { get; set; }

        public List<Module> Modules { get; set; }
    }
}
