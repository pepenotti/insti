namespace Insti.Core.Constants
{
    public static class UserRoles
    {
        public const string Admin = "admin";
        public const string Student = "student";
        public const string Professor = "professor";
        public const string Monitor = "monitor";

        public readonly static List<string> Roles = new() { Admin, Student, Professor, Monitor }; 
    }
}