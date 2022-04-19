namespace Insti.Data.Models
{
    public abstract class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId{ get; set; }
        public Gender Gender { get; set; }
    }
}
