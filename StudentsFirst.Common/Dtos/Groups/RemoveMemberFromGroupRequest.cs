namespace StudentsFirst.Common.Dtos.Groups
{
    public record RemoveMemberFromGroupRequest(
        string GroupId,
        string UserId
    );
}
