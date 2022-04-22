using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insti.API.Test.Helpers
{
    public static class UserManagerMocker
    {
        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            return mgr;
        }

        public static void InitUserManagerMock(
            ref Mock<UserManager<IdentityUser>> mock,
            Func<ClaimsPrincipal, Task<IdentityUser>> getUserAsyncReturn,
            Func<IdentityUser, IEnumerable<string>, Task<IdentityResult>> removeFromRolesAsyncReturn,
            Func<IdentityUser, IEnumerable<string>, Task<IdentityResult>> addToRolesAsyncReturn)
        {
            mock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(getUserAsyncReturn);

            mock.Setup(x => x.RemoveFromRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(removeFromRolesAsyncReturn);

            mock.Setup(x => x.AddToRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(addToRolesAsyncReturn);
        }

    }
}
