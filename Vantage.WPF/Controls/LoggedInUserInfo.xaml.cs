using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vantage.WPF.Controls.Models;

namespace Vantage.WPF.Controls
{
    /// <summary>
    /// Interaction logic for LoggedInUserInfo.xaml
    /// </summary>
    public partial class LoggedInUserInfo : UserControl
    {
        public static readonly DependencyProperty UserInfoProperty = DependencyProperty.Register(nameof(UserInfo), typeof(UserInfo), typeof(LoggedInUserInfo), new UIPropertyMetadata(null, OnUserInfoChanged));

        public UserInfo UserInfo 
        {
            get { return (UserInfo)GetValue(UserInfoProperty); }
            set { SetValue(UserInfoProperty, value); }
        }

        public LoggedInUserInfo()
        {
            InitializeComponent();
        }

        private static void OnUserInfoChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LoggedInUserInfo loggedInUserInfo = obj as LoggedInUserInfo;
            UserInfo userInfo = e.NewValue as UserInfo;
            loggedInUserInfo.TBUsername.Text = userInfo.Username;
            loggedInUserInfo.TBRoles.Text = userInfo.Roles;
        }
    }
}
