using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DMS
{
    public class CustomProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string login)
        {
            DMSContext db = new DMSContext();
            string[] role = { };
            try
            {
                User user = (from u in db.Users
                             where u.Login == login
                             select u).FirstOrDefault();
                if (user != null)
                {
                    Role userRole = db.Roles.FirstOrDefault(item => item.Id == user.RoleId);

                    if (userRole != null)
                    {
                        role = new[] { userRole.Name };
                    }
                }
            }
            catch
            {
                role = new string[] { };
            }
            return role;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            DMSContext db = new DMSContext();
            bool outputResult = false;
            try
            {
                User user = (from u in db.Users
                             where u.Login == username
                             select u).FirstOrDefault();
                if (user != null)
                {
                    Role userRole = db.Roles.FirstOrDefault(item => item.Id == user.RoleId);

                    if (userRole != null && userRole.Name == roleName)
                    {
                        outputResult = true;
                    }
                }
            }
            catch
            {
                outputResult = false;
            }
            return outputResult;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}