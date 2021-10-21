namespace StudentsFirst.Common.Models
{
    public class UserGroupMembership
    {
        public UserGroupMembership(string userId, string groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }

        public string UserId { get; set; }
        public string GroupId { get; set; }
    }
}
