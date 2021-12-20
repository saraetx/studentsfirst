using System.Collections.Generic;
using System.Threading.Tasks;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Common.Services
{
    public interface IGroupsService
    {
        public Task<Group?> FindByGroupIdOrDefaultAsync(string groupId, string? withUserId = null);
        public Task<(IList<Group>, int)> FindAllAsync(
            string? withUserId = null,
            string? nameIncludes = null,
            int skip = 0,
            int take = 100
        );

        public Task<IList<UserGroupMembership>> GetMembershipsByGroupId(string groupId);
    }
}
