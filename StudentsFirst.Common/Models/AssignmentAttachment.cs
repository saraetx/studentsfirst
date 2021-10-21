using System;

namespace StudentsFirst.Common.Models
{
    public class AssignmentAttachment
    {
        public AssignmentAttachment(string id, string name, string submissionId, DateTime uploadedAt, string downloadUrl)
        {
            Id = id;
            Name = name;
            SubmissionId = submissionId;
            UploadedAt = uploadedAt;
            DownloadUrl = downloadUrl;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public string SubmissionId { get; set; }

        public DateTime UploadedAt { get; set; }

        public string DownloadUrl { get; set; }
    }
}
