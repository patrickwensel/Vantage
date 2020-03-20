using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vantage.WPF.Interfaces;
using Vantage.WPF.Services;
using Vantage.WPF.ViewModels;
using Vantage.WPF.Views;

namespace Vantage.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (MenuItem)sender;
            string cmd = string.Empty;

            // The Tag property contains a command 
            // or the name of a user control to load
            if (mnu.Tag != null)
            {
                cmd = mnu.Tag.ToString();
                if (cmd.Contains("."))
                {
                    // Display a user control
                    LoadUserControl(cmd);
                }
                else
                {
                    // Process special commands
                    ProcessMenuCommands(cmd);
                }
            }
        }

        private bool ShouldLoadUserControl(string controlName)
        {
            bool ret = true;

            // Make sure you don't reload a control already loaded.
            if (contentArea.Children.Count > 0)
            {
                if (((UserControl)contentArea.Children[0]).GetType().Name ==
                    controlName.Substring(controlName.LastIndexOf(".") + 1))
                {
                    ret = false;
                }
            }

            return ret;
        }

        private void LoadUserControl(string controlName)
        {
            Type ucType = null;
            UserControl uc = null;

            if (ShouldLoadUserControl(controlName))
            {
                // Create a Type from controlName parameter
                ucType = Type.GetType(controlName);
                if (ucType == null)
                {
                    MessageBox.Show("The Control: " + controlName
                                     + " does not exist.");
                }
                else
                {
                    // Close current user control in content area
                    // NOTE: Optionally add current user control to a list 
                    //     so you can restore it when you close the newly added one
                    CloseUserControl();

                    // Create an instance of this control
                    uc = (UserControl)Activator.CreateInstance(ucType);
                    if (uc != null)
                    {
                        // Display control in content area
                        DisplayUserControl(uc);
                    }
                }
            }
        }

        private void ProcessMenuCommands(string command)
        {
            switch (command.ToLower())
            {
                case "exit":
                    this.Close();
                    break;

                case "login":
                    //if (_viewModel.UserEntity.IsLoggedIn)
                    //{
                    //    // Logging out, so close any open windows
                    //    CloseUserControl();
                    //    // Reset the user object
                    //    _viewModel.UserEntity = new User();
                    //    // Make menu display Login
                    //    _viewModel.LoginMenuHeader = "Login";
                    //}
                    //else
                    //{
                    // Display the login screen
                    //  LoadUserControl("Vantage.WPF.Views.LoginWindow");
                    //}

                    //Show the login view
                    AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
                    IView loginWindow = new LoginWindow(viewModel);
                    loginWindow.Show();
                    break;

                default:
                    break;
            }
        }


        private void CloseUserControl()
        {
            // Remove current user control
            contentArea.Children.Clear();

            // Restore the original status message
          //  _viewModel.StatusMessage = _originalMessage;
        }

        public void DisplayUserControl(UserControl uc)
        {
            // Add new user control to content area
            contentArea.Children.Add(uc);
        }
    }
}
