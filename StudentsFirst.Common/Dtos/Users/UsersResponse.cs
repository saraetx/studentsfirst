using System.Collections.Generic;

namespace StudentsFirst.Common.Dtos.Users
{
    public record UsersResponse(
        IList<UserResponse> Users,
        bool Filtering,
        int Skipping,
        int Taking
    );
}
