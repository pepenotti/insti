using System.ComponentModel.DataAnnotations.Schema;

namespace Insti.Data.Models
{
    public class Schedule : BaseEntity
    {
        public int ModuleId { get; set; }

        public Module Module { get; set; }

        public DateTime Start { get; set; }

        public TimeSpan Duration { get; set; }

        [NotMapped]
        public DateTime End => Start.Add(Duration);
    }
}
