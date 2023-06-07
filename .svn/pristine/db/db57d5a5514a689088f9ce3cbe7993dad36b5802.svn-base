using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SaleDXGui.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; }
        public CustomPrincipal(string orgCode)
        {
            Identity = new GenericIdentity(orgCode);
        }
        public string[] Roles { get; set; }
        public string Id => Identity.Name;
        public bool IsAuthenticated { get; set; }
        public bool IsInRole(string role)
        {
            var grantedRole = role.Split(',').ToList();
            return Roles.Intersect(grantedRole).Any();
        }
    }
}