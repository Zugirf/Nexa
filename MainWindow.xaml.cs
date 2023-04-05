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
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace Nexa
{

    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        public class LoggedInUser
        {
            public string User { get; set; }
            public string Token { get; set; }
        }

        private void appExit(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MStatus.Text = ex.Message;
            }
        }

        private void appMove(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                MStatus.Text = ex.Message;
            }
        }

        private void appMinimize(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MStatus.Text = ex.Message;
            }
        }

        private void Darkmode(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var black = Color.FromRgb(0, 0, 0);
                var dark = Color.FromRgb(44, 47, 51);
                var darkerdark = Color.FromRgb(35, 39, 42);
                var white = Color.FromRgb(255, 255, 255);


                this.MainBorder.Background = new System.Windows.Media.SolidColorBrush(darkerdark);
                this.VersionTxt.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.loginView.LoginBox.Background = new System.Windows.Media.SolidColorBrush(dark);
                this.loginView.LoginBox.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.loginView.PasswordBox.Background = new System.Windows.Media.SolidColorBrush(dark);
                this.loginView.PasswordBox.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.loginView.LoginPlaceholder.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.loginView.PasswordPlaceholder.Foreground = new System.Windows.Media.SolidColorBrush(white);

                this.registerView.RegisterUser.Background = new System.Windows.Media.SolidColorBrush(dark);
                this.registerView.RegisterPass.Background = new System.Windows.Media.SolidColorBrush(dark);
                this.registerView.RegisterPassConf.Background = new System.Windows.Media.SolidColorBrush(dark);

                this.registerView.RegisterUser.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.registerView.RegisterPass.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.registerView.RegisterPassConf.Foreground = new System.Windows.Media.SolidColorBrush(white);

                this.devView.DevText.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.devView.DevText2.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.devView.DevText3.Foreground = new System.Windows.Media.SolidColorBrush(white);

                this.devView.PingBtn.Background = new System.Windows.Media.SolidColorBrush(darkerdark);
                this.devView.WSCBtn.Background = new System.Windows.Media.SolidColorBrush(darkerdark);
                this.devView.WSPBtn.Background = new System.Windows.Media.SolidColorBrush(darkerdark);

                this.devView.PingBtn.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.devView.WSCBtn.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.devView.WSPBtn.Foreground = new System.Windows.Media.SolidColorBrush(white);


                //start darkmode for chat userctrl

                this.chatView.MsgBox.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.chatView.ChatBox.Foreground = new System.Windows.Media.SolidColorBrush(white);
                this.chatView.SendBorder.Background = new System.Windows.Media.SolidColorBrush(dark);
                this.chatView.ChatBox.Background = new System.Windows.Media.SolidColorBrush(dark);


                this.chatView.DarkSendImg.Visibility = Visibility.Hidden;
                this.chatView.WhiteSendImg.Visibility = Visibility.Visible;
                this.DarkImg.Visibility = Visibility.Hidden;
                this.LightImg.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MStatus.Text = ex.Message;
            }
        }
        private void Lightmode(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var black = Color.FromRgb(0, 0, 0);
                var dark = Color.FromRgb(44, 47, 51);
                var darkerdark = Color.FromRgb(35, 39, 42);
                var white = Color.FromRgb(255, 255, 255);
                var darkwhite = Color.FromRgb(236, 236, 236);
                var darkerwhite = Color.FromRgb(216, 216, 216);

                var loginback = Color.FromRgb(246, 246, 246);
                var loginfore = Color.FromRgb(169, 169, 169);

                this.MainBorder.Background = new System.Windows.Media.SolidColorBrush(white);
                this.VersionTxt.Foreground = new System.Windows.Media.SolidColorBrush(black);
                this.loginView.LoginBox.Background = new System.Windows.Media.SolidColorBrush(loginback);
                this.loginView.LoginBox.Foreground = new System.Windows.Media.SolidColorBrush(loginfore);
                this.loginView.PasswordBox.Background = new System.Windows.Media.SolidColorBrush(loginback);
                this.loginView.PasswordBox.Foreground = new System.Windows.Media.SolidColorBrush(loginfore);
                this.loginView.LoginPlaceholder.Foreground = new System.Windows.Media.SolidColorBrush(loginfore);
                this.loginView.PasswordPlaceholder.Foreground = new System.Windows.Media.SolidColorBrush(loginfore);

                this.registerView.RegisterUser.Background = new System.Windows.Media.SolidColorBrush(loginback);
                this.registerView.RegisterPass.Background = new System.Windows.Media.SolidColorBrush(loginback);
                this.registerView.RegisterPassConf.Background = new System.Windows.Media.SolidColorBrush(loginback);

                this.registerView.RegisterUser.Foreground = new System.Windows.Media.SolidColorBrush(loginfore);
                this.registerView.RegisterPass.Foreground = new System.Windows.Media.SolidColorBrush(loginfore);
                this.registerView.RegisterPassConf.Foreground = new System.Windows.Media.SolidColorBrush(loginfore);

                this.devView.DevText.Foreground = new System.Windows.Media.SolidColorBrush(black);
                this.devView.DevText2.Foreground = new System.Windows.Media.SolidColorBrush(black);
                this.devView.DevText3.Foreground = new System.Windows.Media.SolidColorBrush(black);

                this.devView.PingBtn.Background = new System.Windows.Media.SolidColorBrush(white);
                this.devView.WSCBtn.Background = new System.Windows.Media.SolidColorBrush(white);
                this.devView.WSPBtn.Background = new System.Windows.Media.SolidColorBrush(white);

                this.devView.PingBtn.Foreground = new System.Windows.Media.SolidColorBrush(black);
                this.devView.WSCBtn.Foreground = new System.Windows.Media.SolidColorBrush(black);
                this.devView.WSPBtn.Foreground = new System.Windows.Media.SolidColorBrush(black);


                //start lightmode for chat userctrl

                this.chatView.MsgBox.Foreground = new System.Windows.Media.SolidColorBrush(black);
                this.chatView.ChatBox.Foreground = new System.Windows.Media.SolidColorBrush(black);
                this.chatView.SendBorder.Background = new System.Windows.Media.SolidColorBrush(darkerwhite);
                this.chatView.ChatBox.Background = new System.Windows.Media.SolidColorBrush(darkerwhite);


                this.chatView.DarkSendImg.Visibility = Visibility.Visible;
                this.chatView.WhiteSendImg.Visibility = Visibility.Hidden;
                this.DarkImg.Visibility = Visibility.Visible;
                this.LightImg.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MStatus.Text = ex.Message;
            }
        }
    }
}