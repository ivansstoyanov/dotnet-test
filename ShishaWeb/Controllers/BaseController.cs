using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShishaWeb.Config.Enumerations;

namespace ShishaWeb.Controllers
{    
    public class BaseController : Controller
    {
        protected string GetCurrentUserClaim(string claim)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            foreach (Claim c in claimsIdentity.Claims)
            {
                if (c.Type == claim)
                {
                    return c.Value;
                }
            }

            return null;
        }

        protected bool IsCurrentUserAdmin(MongoModels.User user)
        {
            if (user?.UserGroup == UserGroupEnum.SystemAdmin || user?.UserGroup == UserGroupEnum.Admin)
            {
                return true;
            }

            return false;
        }
    }
}
