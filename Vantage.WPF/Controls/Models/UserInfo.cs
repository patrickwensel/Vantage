using System;
using System.Collections.Generic;
using System.Text;
using Vantage.Common.Models;

namespace Vantage.WPF.Controls.Models
{
    public class UserInfo
    {
        public string Username { get; set; }

        public string Roles { get; set; }

        public string ImageUrl { get; set; }

        public UserReturnObject FullUserInfo { get; set; }
    }
}
