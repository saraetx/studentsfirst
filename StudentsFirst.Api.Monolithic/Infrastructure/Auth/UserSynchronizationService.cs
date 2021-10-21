using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public class UserSynchronizationService : IUserSynchronizationService
    {
        public UserSynchronizationService(StudentsFirstContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly StudentsFirstContext _dbContext;

        public async Task<User> SynchronizeUser(ClaimsPrincipal userPrincipal)
        {
            string userId = userPrincipal.GetObjectId()
                ?? throw new ClaimsInvalidException(ClaimConstants.Oid, ClaimsInvalidException.CLAIM_NOT_FOUND);
            string name = userPrincipal.GetDisplayName()
                ?? throw new ClaimsInvalidException(ClaimConstants.Name, ClaimsInvalidException.CLAIM_NOT_FOUND);
            
            bool isStudent = userPrincipal.IsInRole(RoleConstants.STUDENT);
            bool isTeacher = userPrincipal.IsInRole(RoleConstants.TEACHER);

            if (!(isStudent || isTeacher) || (isStudent && isTeacher))
            {
                throw new ClaimsInvalidException(ClaimConstants.Roles, ClaimsInvalidException.CLAIM_UNPROCESSABLE);
            }

            string role = isStudent ? RoleConstants.STUDENT : RoleConstants.TEACHER;

            User? user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            bool saveRequired = false;

            if (user == null)
            {
                user = new User(userId, name, role);
                _dbContext.Users.Add(user);
                saveRequired = true;
            }
            else
            {
                if (user.Name != name)
                {
                    user.Name = name;
                    saveRequired = true;
                }

                if (user.Role != role)
                {
                    user.Role = role;
                    saveRequired = true;
                }
            }

            if (saveRequired) { await _dbContext.SaveChangesAsync(); }

            return user;
        }
    }
}
