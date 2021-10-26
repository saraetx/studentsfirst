using System.Collections.Generic;

namespace StudentsFirst.Common.Dtos.Groups
{
    public record GroupsResponse(
        IList<GroupResponse> Groups,
        bool Filtering,
        int Total,
        int Skipping,
        int Taking
    );
}
