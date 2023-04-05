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
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Register : UserControl
    {

        public static Register register;
        public string uToken;
        public Register()
        {
            InitializeComponent();
            register = this;
        }

        public UserControl chatView { get; private set; }
        public UserControl loginView { get; private set; }

        public void RegisterUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string reguser;
            reguser = RegisterUser.Text;
        }

        public void RegisterPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            string regpass;
            regpass = RegisterPass.Text;
        }

        public void RegisterPassConf_TextChanged(object sender, TextChangedEventArgs e)
        {
            string regpassconf;
            regpassconf = RegisterPassConf.Text;
        }

        public async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var error = Color.FromRgb(255, 0, 0);
            var ok = Color.FromRgb(0, 255, 0);

            if (RegisterPass.Text == RegisterPassConf.Text)
            {
                var login = new { username = RegisterUser.Text, password = RegisterPass.Text };
                var content = JsonContent.Create(login);
                var _httpClient = new HttpClient();
                var result = await _httpClient.PostAsync("http://fi.dynamichost.cc:20100/register", content);

                var status = result.StatusCode;
                uToken = await result.Content.ReadAsStringAsync();

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    RStatus.Foreground = new System.Windows.Media.SolidColorBrush(ok);
                    RegisterUser.IsReadOnly = true;
                    RegisterPass.IsReadOnly = true;
                    RegisterPassConf.IsReadOnly = true;
                    RStatus.Text = status.ToString();
                    Chat.chat.Visibility = Visibility.Visible;
                    registerView.Visibility = Visibility.Hidden;
                }
                else
                {
                    RStatus.Foreground = new System.Windows.Media.SolidColorBrush(error);
                    RStatus.Text = status.ToString();
                }
            }
            else
            {
                RStatus.Foreground = new System.Windows.Media.SolidColorBrush(error);
                RStatus.Text = ("Passwords must\nmatch!");
            }
        }

        private void LoginBtn(object sender, MouseButtonEventArgs e)
        {
            try
            {
                registerView.Visibility = Visibility.Collapsed;
                Login.login.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                RStatus.Text = ex.Message;
            }
        }
    }
}
