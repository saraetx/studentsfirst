namespace StudentsFirst.Common.Dtos.Groups
{
    public record FindAllGroupsRequest(
        string? NameIncludes,
        bool OwnOnly,
        int Skip,
        int Take
    );
}
