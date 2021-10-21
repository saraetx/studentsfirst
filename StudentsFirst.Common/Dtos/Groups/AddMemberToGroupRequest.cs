namespace StudentsFirst.Common.Dtos.Groups
{
    public record AddMemberToGroupRequest(
        string GroupId,
        string UserId
    );
}
