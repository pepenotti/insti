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
        public static Mock<UserManager<IdentityUser>> MockUserManager<IdentityUser>() where IdentityUser : class
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<IdentityUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

            return mgr;
        }

        public static void InitUserManagerMock(
            ref Mock<UserManager<IdentityUser>> mock,
            Func<ClaimsPrincipal, Task<IdentityUser>> geIdentityUserAsyncReturn,
            Func<IdentityUser, IEnumerable<string>, Task<IdentityResult>> removeFromRolesAsyncReturn,
            Func<IdentityUser, IEnumerable<string>, Task<IdentityResult>> addToRolesAsyncReturn)
        {
            mock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(geIdentityUserAsyncReturn);

            mock.Setup(x => x.RemoveFromRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(removeFromRolesAsyncReturn);

            mock.Setup(x => x.AddToRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(addToRolesAsyncReturn);
        }

    }
}
