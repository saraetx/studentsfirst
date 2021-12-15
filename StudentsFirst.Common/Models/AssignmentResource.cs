namespace StudentsFirst.Common.Models
{
    public class AssignmentResource
    {
        public AssignmentResource(string id, string name, string downloadUrl, string assignmentId)
        {
            Id = id;
            Name = name;
            DownloadUrl = downloadUrl;
            AssignmentId = assignmentId;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public string DownloadUrl { get; set; }

        public string AssignmentId { get; set; }
    }
}
