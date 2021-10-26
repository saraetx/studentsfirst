using System.Collections.Generic;
using StudentsFirst.Common.Dtos.Users;

namespace StudentsFirst.Common.Dtos.Groups
{
    public record GroupMembersResponse(
        GroupResponse Group,
        IList<UserResponse> Members,
        int Total,
        int Skipping,
        int Taking
    );
}
