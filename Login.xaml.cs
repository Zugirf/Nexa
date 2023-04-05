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
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using SocketIOClient;
using System.Net.Sockets;
using System.Diagnostics;
using static Nexa.Chat;
using Color = System.Windows.Media.Color;

namespace Nexa
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public static Login login;
        public UserControl registerView => new UserControl();
        public UserControl chatView => new Chat();

        public UserControl devView => new Dev();



        public Login()
        {
            InitializeComponent();
            login = this;
            LoginBox.GotFocus += HideLoginPlaceholder;
            LoginBox.LostFocus += ShowLoginPlaceholder;
            LoginBox.TextChanged += CheckLoginPlaceholder;
            PasswordBox.GotFocus += HidePasswordPlaceholder;
            PasswordBox.LostFocus += ShowPasswordPlaceholder;
            PasswordBox.TextChanged += CheckPasswordPlaceholder;

        }

        private void HideLoginPlaceholder(object sender, RoutedEventArgs e)
        {
            // Hide the login placeholder when LoginBox gets focus
            LoginPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void ShowLoginPlaceholder(object sender, RoutedEventArgs e)
        {
            // Show the login placeholder if LoginBox loses focus and has no text
            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                LoginPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void CheckLoginPlaceholder(object sender, TextChangedEventArgs e)
        {
            // Check if LoginBox has text; if so, hide the login placeholder
            if (!string.IsNullOrEmpty(LoginBox.Text))
            {
                LoginPlaceholder.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void HidePasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            // Hide the password placeholder when PasswordBox gets focus
            PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void ShowPasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            // Show the password placeholder if PasswordBox loses focus and has no text
            if (string.IsNullOrEmpty(PasswordBox.Text))
            {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void CheckPasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            // Check if PasswordBox has text; if so, hide the password placeholder
            if (!string.IsNullOrEmpty(PasswordBox.Text))
            {
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        public void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            string login1;
            login1 = LoginBox.Text;
            if (LoginBox.Text == null)
            {
                LoginPlaceholder.Visibility = Visibility.Visible;
            }
            if (LoginBox.Text != null)
            {
                LoginPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        public void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pass1;
            pass1 = PasswordBox.Text;
        }

        private SocketIO client;
        private string isLoggedIn = null;
        public string token;


        public async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var error = Color.FromRgb(255, 0, 0);
            var ok = Color.FromRgb(0, 255, 0);

            // developer menu access
            if (LoginBox.Text == "Dev" && PasswordBox.Text == "NexaDev69!")
            {
                loginView.Visibility = Visibility.Collapsed;
                Dev.dev.Visibility = Visibility.Visible;
                LoginBox.IsEnabled = false;
                PasswordBox.IsEnabled = false;
            }
            else
            {
                var client = new SocketIO("http://fi.dynamichost.cc:20100");

                client.OnConnected += async (sender, e) =>
                {
                    Console.WriteLine("Connected to server");
                    await Dispatcher.InvokeAsync(() =>
                    {
                        // Send login request
                        var requestData = new
                        {
                            username = LoginBox.Text,
                            password = PasswordBox.Text
                        };
                        if (isLoggedIn == null)
                        {
                            client.EmitAsync("login", requestData);
                        }
                        else
                        {
                            Debug.WriteLine("Already Logged in!");
                        }
                    });
                };

                var serializerSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

                client.On("loginResponse", response =>
                {
                    JToken json = JToken.Parse(response.ToString());
                    if (json.Type == JTokenType.Array)
                    {
                        foreach (JObject obj in json)
                        {
                            // Handle each object in the array
                            if (obj.GetValue("success").ToString() == "False")
                            {
                                Debug.WriteLine("Login failed: Wrong username or password");
                                client.DisconnectAsync();

                                // Modify the UI elements to display a red borderbrush for wrong password / username
                                Dispatcher.InvokeAsync(() =>
                                {
                                    LoginBox.BorderBrush.Opacity = 100;
                                    //Code here

                                });
                            }
                            else if (obj.GetValue("success").ToString() == "True")
                            {
                                string token = obj.GetValue("token").ToString();
                                Debug.WriteLine($"Successful login, the token is: {token}");
                                isLoggedIn = "True";
                                Dispatcher.InvokeAsync(() =>
                                {
                                    loginView.Visibility = Visibility.Collapsed;
                                    Chat.chat.Visibility = Visibility.Visible;
                                });
                            }
                        }
                    }
                });

                await client.ConnectAsync();
            };
        }










        private void RegisterBtn(object sender, MouseButtonEventArgs e)
        {
            try
            {
                loginView.Visibility = Visibility.Collapsed;
                Register.register.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                LStatus.Text = ex.Message;
            }
        }
    }
}
