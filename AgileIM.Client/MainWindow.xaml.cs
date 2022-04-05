using AgileIM.Client.Controls;
using AgileIM.IM.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
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

namespace AgileIM.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CustomWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private readonly ClientWebSocket _webSocket = new();
        private readonly CancellationToken _cancellation = new();
        public async Task WebSocket(string url)
        {
            try
            {
                url =
                    @"localhost:9659?Authorization=Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkNGNzFCQUE0RTZFOEVFM0M1RTczMDE3NENFMzhGN0VCIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2MzI0NzI5OTksImV4cCI6MTYzMjUxNjE5OSwiaXNzIjoiaHR0cDovL2hvc3QuZG9ja2VyLmludGVybmFsOjUyMDAiLCJhdWQiOlsiUHJlcGFyYXRpb25TZXJ2aWNlIiwiUmVzb3VyY2VTZXJ2aWNlIiwiVGVhY2hpbmdTZXJ2aWNlIiwiVXNlclNlcnZpY2UiXSwiY2xpZW50X2lkIjoidXNlcl9tYW5hZ2VySWQiLCJzdWIiOiJZVFhXMFQyMDIxMDAxNzYiLCJhdXRoX3RpbWUiOjE2MzI0NzI5OTksImlkcCI6Imhvc3QuZG9ja2VyLmludGVybmFsOjUyMDAiLCJkYl9uYW1lX3ByZWZpeCI6IndsanlfcnNjXyIsImNsb3VkX2Ric2VydmVyX2lwIjoiIiwiY2xvdWRfZGJzZXJ2ZXJfcG9ydCI6IjMzMDYiLCJ1c2VyVWlkIjoiMDA0YjlmMDgtZjc1Yy00M2E0LWFjNzgtNjNmOTAzYzQyNjFjIiwibmlja05hbWUiOiIiLCJ1c2VyQWNjb3VudCI6IllUWFcwVDIwMjEwMDE3NiIsInVuaXRVSWQiOiJhZjg0NThjMi05ODcyLTExZWEtOGE5OC1mYTE2M2UzZjAyZTciLCJ1bml0TmFtZSI6IuS6muWkqui1hOa6kOS4reW_gyIsInVzZXJUeXBlQ29kZSI6IjIwIiwid2VDaGF0T3BlbmlkIjoiIiwic2NodGVybVVpZCI6IjBmZmFjZTBlLTEzOTUtMTFlYy1iZDk5LWZhMTYzZTEwMzMyMSIsInVuaXRJZCI6IjIiLCJ0b2tlbkV4cGlyZVRpbWUiOiIyMDIxLTA5LTI0IDIwOjQzOjE5IiwianRpIjoiQUNFOUJCQ0M2ODczMTM5NDkxRUFDNTM3NzY4QTdGQ0MiLCJpYXQiOjE2MzI0NzI5OTksInNjb3BlIjpbIm9wZW5pZCIsIlByZXBhcmF0aW9uU2VydmljZSIsInByb2ZpbGUiLCJSZXNvdXJjZVNlcnZpY2UiLCJUZWFjaGluZ1NlcnZpY2UiLCJVc2VyU2VydmljZSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJwd2QiXX0.oX0Tv9mYQOR_sXlXgNeiPVh3ojdAnwsCYYJ_ECljYEL4lk2m_8gMCZfxj7kg1_elIERl4VkWNjVZhkaXru3ExPv9nXjRntYjgQlO6l-E7azkyeAJz7eQbHesNch1lsUviiDhpW7PPysbzpLDnbtwYV9ruF3TK0eOQg-BadoYHN1nJfR2xVcFr50U1rYs624pddtwKJhEl8UwRzNqNqWQ0bEWCvaWdNmEwuLnxhtPIeSMo3a4-qcn0Mr4_wZ3XlUr7oG-bVN_DqnSFKfkpOIyQqJGqCgfMagMsm6uIs3_Fox1vHP2NEvuD6qA9W70r4W8nBpYuD1ObE3YRcXTjEXBEA";

                await _webSocket.ConnectAsync(new Uri(url), _cancellation);

                var result = new byte[4096];
                while (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.ReceiveAsync(result, default);
                    try
                    {
                        // 接收数据
                        var str = Encoding.UTF8.GetString(result);
                        if (string.IsNullOrEmpty(str)) continue;
                        var msg = JsonConvert.DeserializeObject<Message>(str);


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
