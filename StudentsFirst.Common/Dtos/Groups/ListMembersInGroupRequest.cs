namespace StudentsFirst.Common.Dtos.Groups
{
    public record ListMembersInGroupRequest(
        string GroupId,
        int Skip,
        int Take
    );
}
