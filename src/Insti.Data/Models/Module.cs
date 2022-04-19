namespace Insti.Data.Models
{
    public class Module : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public StudyPlan StudyPlan { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public List<Professor> Professor { get; set; }
    }
}
