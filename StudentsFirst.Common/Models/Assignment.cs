using System;
using System.Collections.Generic;

namespace StudentsFirst.Common.Models
{
    public class Assignment
    {
        public Assignment(string id, string name, bool allowsOverdueCompletions, DateTime dueAt)
        {
            Id = id;
            Name = name;
            AllowsOverdueCompletions = allowsOverdueCompletions;
            DueAt = dueAt;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public IList<AssignmentResource>? Resources { get; set; }

        public bool AllowsOverdueCompletions { get; set; }
        public DateTime DueAt { get; set; }

        public IList<AssignmentSubmission>? Submissions { get; set; }
    }
}
