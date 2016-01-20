using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace DietPlanner.Helpers
{
    public static class AccountHelper
    {
        public static string GetLoggedUserId()
        {
            return System.Web.HttpContext.Current.User.Identity.GetUserId();
        }
        
        public static bool UserInRole(string roleName)
        {
            return System.Web.HttpContext.Current.User.IsInRole(roleName);
        }
    }
}