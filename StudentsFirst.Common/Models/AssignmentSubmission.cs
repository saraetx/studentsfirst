using System;
using System.Collections.Generic;

namespace StudentsFirst.Common.Models
{
    public class AssignmentSubmission
    {
        public AssignmentSubmission(string id, string assigneeId, string assignmentId, bool completed, DateTime? completedAt)
        {
            Id = id;
            AssigneeId = assigneeId;
            AssignmentId = assignmentId;
            Completed = completed;
            CompletedAt = completedAt;
        }

        public string Id { get; set; }

        public string AssigneeId { get; set; }
        public string AssignmentId { get; set; }

        public IList<AssignmentAttachment>? Attachments { get; set; }

        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
