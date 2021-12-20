using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Common.Models;
using StudentsFirst.Common.Services;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    public class GroupsService : IGroupsService
    {
        public GroupsService(StudentsFirstContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly StudentsFirstContext _dbContext;

        public async Task<(IList<Group>, int)> FindAllAsync(
            string? withUserId = null,
            string? nameIncludes = null,
            int skip = 0,
            int take = 100
        )
        {
            IQueryable<Group> groups = _dbContext.Groups;

            if (withUserId is not null)
            {
                groups = FilterGroupsWithUserId(groups, withUserId);
            }

            if (nameIncludes is not null)
            {
                groups = groups.Where(group => group.Name.ToLower().Contains(nameIncludes.ToLower()));
            }

            int total = await groups.CountAsync();

            groups = groups.OrderBy(group => group.Name).Skip(skip).Take(take);

            return (await groups.ToListAsync(), total);
        }

        public async Task<Group?> FindByGroupIdOrDefaultAsync(string groupId, string? withUserId = null)
        {
            IQueryable<Group> groups = _dbContext.Groups;

            if (withUserId is not null)
            {
                groups = FilterGroupsWithUserId(groups, withUserId);
            }

            return await groups.SingleOrDefaultAsync(group => group.Id == groupId);
        }

        public Task<IList<UserGroupMembership>> GetMembershipsByGroupId(string groupId)
        {
            throw new System.NotImplementedException();
        }

        private IQueryable<Group> FilterGroupsWithUserId(IQueryable<Group> groups, string withUserId) =>
            from @group in groups
            join userGroupMembership in _dbContext.UserGroupMemberships on @group.Id equals userGroupMembership.GroupId
            where userGroupMembership.UserId == withUserId
            select @group;
    }
}
