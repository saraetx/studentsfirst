namespace StudentsFirst.Common.Dtos.Groups
{
    public record CreateGroupRequest(
        string Name,
        bool AddSelf
    );
}
