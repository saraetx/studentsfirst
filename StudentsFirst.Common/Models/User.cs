using StudentsFirst.Common.Constants;

namespace StudentsFirst.Common.Models
{
    public class User
    {
        public User(string id, string name, string role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Possible values for role are enumerated in <see cref="RoleConstants">.
        /// </summary>
        public string Role { get; set; }

        public bool IsStudent => Role == RoleConstants.STUDENT;
        public bool IsTeacher => Role == RoleConstants.TEACHER;
    }
}
