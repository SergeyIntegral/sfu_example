using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Example.Core.Consts;
using Example.DAL.Entities;
using Example.Services.Context;

namespace Example.Web.Infrastructure.Extensions
{
    public static class PrincipalExtension
    {
        #region Section

        public static bool CanManageSection(this IPrincipal user)
        {
            return HttpContext.Current.Request.IsAuthenticated
                   && (user.IsInRole(UserRoles.Administrator)
                       || user.IsInRole(UserRoles.Moderator));
        }

        #endregion


        #region Topic

        public static bool CanCreateTopic(this IPrincipal user)
        {
            return HttpContext.Current.Request.IsAuthenticated
                   && (user.IsInRole(UserRoles.Administrator)
                       || user.IsInRole(UserRoles.Moderator)
                       || user.IsInRole(UserRoles.User));
        }

        public static bool CanManageTopic(this IPrincipal user, ExampleUser author)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return false;

            if (author != null && author.Id == ExampleContext.Current.User.Id)
            {
                return true;
            }

            return user.IsInRole(UserRoles.Administrator) || user.IsInRole(UserRoles.Moderator);
        }

        #endregion

        #region Message

        public static bool CanCreateMessage(this IPrincipal user)
        {
            // All authenticated users
            return HttpContext.Current.Request.IsAuthenticated;
        }

        public static bool CanManageMessage(this IPrincipal user, ExampleUser author)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return false;

            if (author != null && author.Id == ExampleContext.Current.User.Id)
            {
                return true;
            }

            return user.IsInRole(UserRoles.Administrator) || user.IsInRole(UserRoles.Moderator);
        }

        #endregion


    }
}