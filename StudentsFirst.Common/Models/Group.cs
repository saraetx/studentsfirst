namespace StudentsFirst.Common.Models
{
    public class Group
    {
        public Group(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
