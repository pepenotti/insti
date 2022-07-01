namespace Insti.Data.Models
{
    public class Module : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public StudyPlan StudyPlan { get; set; }

        public List<Professor> Professor { get; set; }
    }
}
