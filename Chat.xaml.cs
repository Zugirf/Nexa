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
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        public class Message
        {

            [JsonProperty("author")]
            public string author { get; set; }

            [JsonProperty("content")]
            public string content { get; set; }

            [JsonProperty("time")]
            public string time { get; set; }
        }

        public class Data
        {

            [JsonProperty("messages")]
            public IList<Message> messages { get; set; }
        }

        public static Chat chat;
        public string token;
        public Chat()
        {
            InitializeComponent();
            chat = this;
        }

        public void ChatBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string send1;
            send1 = ChatBox.Text;
        }

        private async void SendEvent(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(Login.login.token))
            {
                CStatus.Text = ("You need to be logged in to do that!");
                return;
            }
            else
            {
                if (ChatBox.Text == null)
                {
                    CStatus.Text = ("You must include a message!");
                }
                else
                {
                    var sendcontent = new { content = ChatBox.Text };
                    var content = JsonContent.Create(sendcontent);
                    var _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Add("Authorization", Login.login.token);
                    var result = await _httpClient.PostAsync("http://fi.zugirf.xyz:20012/messages/send", content);

                    var status = result.StatusCode;
                    var message = await result.Content.ReadAsStringAsync();

                    ChatBox.Text = ("");

                    //then refresh chatbox

                    var _httpClient2 = new HttpClient();
                    _httpClient2.DefaultRequestHeaders.Add("Authorization", Login.login.token);
                    var result2 = await _httpClient.GetStringAsync("http://fi.zugirf.xyz:20012/messages/get");
                    Data data = JsonConvert.DeserializeObject<Data>(result2);

                    foreach (Message messages in data.messages)
                    {
                        MsgBox.Text += messages.author + ": " + messages.content + "\n" + messages.time + "\n";
                    }

                    MsgBox.ScrollToEnd();
                }
            }
        }
    }
}
