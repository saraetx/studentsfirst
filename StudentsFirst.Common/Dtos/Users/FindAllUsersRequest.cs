namespace StudentsFirst.Common.Dtos.Users
{
    public record FindAllUsersRequest(
        string? NameIncludes,
        string? Role,
        int Skip,
        int Take
    );
}
