namespace Insti.Core.Constants
{
    public static class UserRoles
    {
        public const string Admin = "admin";
        public const string Student = "student";
        public const string Professor = "professor";
        public const string Monitor = "monitor";

        private readonly static List<string> roles = new() { Admin, Student, Professor, Monitor }; 

        // This way we return a new instance of the list, and the roles remain unmodifiable
        public static List<String> Roles => roles.ToList();
    }
}