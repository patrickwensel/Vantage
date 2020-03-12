using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

//Source
//https://social.technet.microsoft.com/wiki/contents/articles/25726.wpf-implementing-custom-authentication-and-authorization.aspx

namespace Vantage.WPF
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, List<string> roles)
        {
            Name = name;
            Roles = roles;
        }

        public string Name { get; private set; }
        public List<string> Roles { get; private set; }


        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
    }
}
