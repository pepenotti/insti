namespace Insti.Data.Models
{
    public class StudyPlan : BaseEntity
    {
        public string Summary { get; set; }

        public string Goals { get; set; }

        public string TimeFrame { get; set; }

        public List<StudyPlanContent> Content { get; set; }
    }

    public class StudyPlanContent : BaseEntity
    {
        public string Title { get; set; }

        public List<StudyPlanContentData> Content { get; set; }
    }

    public class StudyPlanContentData : BaseEntity
    {
        public string Data { get; set; }
    }
}