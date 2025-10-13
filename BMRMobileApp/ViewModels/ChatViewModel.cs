
using BMRMobileApp.Models;
using BMRMobileApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
 
namespace BMRMobileApp.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string sentMessage;
        public bool noChatHub = true;
        string cPText;
        Color cPBC;
        
        public ChatViewModel()
        {
            GetMood();
        }
        private ObservableCollection<ChatMessageModel> chatMess = new ObservableCollection<ChatMessageModel>();
        //    ObservableCollection<ChatMessageModel> PSMessages = new ObservableCollection<ChatMessageModel>();
        //  public ObservableCollection<ChatMessageModel> PSMessages { get; set; }
        [ObservableProperty]
        public ObservableCollection<ChatMessageModel> PSMessages { get=> chatMess; set { chatMess = value; OnPropertyChanged(nameof(PSMessages)); }   
            }

        //   public ObservableCollection<List> Messages { get; set; } = new();

        public string OutgoingText { get; set; }
   
        
        public string InputText
        {
            get => sentMessage;
            set
            {
                sentMessage = value;
                OnPropertyChanged(nameof(InputText));
            }
        }
        public string CPText
        {
            get => cPText;
            set
            {
                cPText = value;
                OnPropertyChanged(nameof(CPText));
            }
        }
        public Color CPBC
        {
            get => cPBC;
            set
            {
                cPBC = value;
                OnPropertyChanged(nameof(CPBC));
            }
        }

        private async void GetMood()
        {
            
            SQLiteDBCommands.SQLiteService sQLiteDB = new SQLiteDBCommands.SQLiteService();
            var mood = sQLiteDB.GetUserCurrentMoodNonAsync();
            CPText = "Chat";
            CPBC=Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
            if (mood != null)
            {
                CPText = $"{CPText} {mood.Mood} {mood.MoodTag}";
                CPBC = Color.FromArgb(mood.BackgroundColor);
            }
        }


        private HubConnection _hubConnection;
        // public ICommand SendCommand => new Command(SendMessage);
        public ICommand ExitCommand => new Command(ExitChat);
        public ICommand SendCommand => new Command(async () => await SendMessage());
        //  [RelayCommand]

        //private  async Task ConnectHub()
        //{
        //    _hubConnection = new HubConnectionBuilder()
        //        .WithUrl($"{ConfigurationManager.ChatHubModel.SingularAppUrl}{ConfigurationManager.ChatHubModel.SingularChatHub}", options =>
        //        {
        //            options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
        //        }) // Replace with your actual hub URL
        //        .WithServerTimeout(TimeSpan.FromSeconds(30))
        //        .WithAutomaticReconnect()
        //        .Build();

        //    _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        //    {
        //        // Update UI with received message
        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            var newMessage = new ChatMessageModel { Sender = user, Message = message,TimesMessageSent =DateTime.Now };
        //            Sender = newMessage.Sender;
        //            Messge = newMessage.Message;
        //            MessageTime = DateTime.Now.ToString();
        //            Messages.Add(newMessage);
        //          //  MainThread.BeginInvokeOnMainThread(() => Messages.Add(newMessage));
        //            //   Messages.Add(new ChatMessageModel
        //            // {
        //            //   Sender = user,
        //            //  Message = message,
        //            // TimesMessageSent = DateTime.Now
        //            //});

        //       });
        //    });

        //   await ConnectToHub();
        //}
        private async Task ConnectHubAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{ConfigurationManager.ChatHubModel.SingularAppUrl}{ConfigurationManager.ChatHubModel.SingularChatHub}", options =>
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                })
                .WithServerTimeout(TimeSpan.FromSeconds(30))
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var newMessage = new ChatMessageModel { Sender = user, SenderMessage = message, TimesMessageSent = DateTime.Now.ToString() };
                    PSMessages.Add(newMessage);
                    //  Messages.Add(new ChatMessageModel
                    // {
                    //    Sender = user,
                    //    Message = message,
                    //    TimesMessageSent = DateTime.Now
                    // });
                });
            });

            _hubConnection.StartAsync().GetAwaiter().GetResult();
        }
       public async Task EnsureHubConnectedAsync()
        {
            if (_hubConnection == null)
                await ConnectHubAsync();

            if (_hubConnection.State == HubConnectionState.Disconnected)
                _hubConnection.StartAsync().GetAwaiter().GetResult();
        }

        private async Task ConnectToHub()
        {
            try
            {
                _hubConnection.StartAsync().GetAwaiter().GetResult();
                // Console.WriteLine("");
            }
            catch (Exception ex)
            {

                // ("Error:",$"Errror connecting to chat hub {ex.Message}", "OK").Wait();
                // if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
                //  {
                //     window.Page = new AppShell();
                // }
            }
        }


        private void AddMessaged()
        {
            PSMessages.Add(new ChatMessageModel
            {
                SenderMessage = "Hello",
                Sender = "Me",
                TimesMessageSent = DateTime.Now.ToString()
            });
            InputText = string.Empty;
        }
        private async Task SendMessageHub(string user, string message)
        {
            // if(_hubConnection == null)
            //    await ConnectHub();
            await EnsureHubConnectedAsync();
            //if (_hubConnection.State == HubConnectionState.Disconnected)

            //  if (_hubConnection.State == HubConnectionState.Connected)
            //  {
            _hubConnection.InvokeAsync("SendMessage", user, message).GetAwaiter().GetResult();
            // }
        }
        void ExitChat()
        {
            if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
            {
                window.Page = new AppShell();
            }
        }
        async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(InputText)) return;
            try
            {
                if (!(noChatHub))
                {
                    SendMessageHub(PsUserLoginModel.instance.Value.PSUserDisplayName, InputText).Wait();
                }
                else
                {
                    ReceiveMessage(InputText);
                    PSMessages.Add(new ChatMessageModel
                    {
                        SenderMessage = "Hub not connected",
                        Sender = "System",
                        TimesMessageSent = DateTime.Now.ToString()
                    });
                }
                InputText = string.Empty;
                //  if (_hubConnection.State == HubConnectionState.Disconnected)
                //  {
                //  if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
                //   {
                //       window.Page = new AppShell();
                //  }
                // }









                // Simulate response
                //  ReceiveMessage(Messages.FirstOrDefault());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error:", $"Error connecting to hub: {ex.Message}", "OK");
                if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
                {
                    window.Page = new AppShell();
                }
            }
        }
        public void Onloading()
        {
            PSMessages.Add(new ChatMessageModel
            {
                Sender = "dan",
                SenderMessage = "test",
                TimesMessageSent = DateTime.Now.ToString()
            }
            );



            }
        void ReceiveMessage(ChatMessageModel chatMessageModel)
        {

            if (chatMessageModel == null) return;


            PSMessages.Add(chatMessageModel);

        }
        void ReceiveMessage(string text)
        {
            PSMessages.Add(new ChatMessageModel
            {
                SenderMessage = text,
                Sender = PsUserLoginModel.instance.Value.PSUserDisplayName,
                TimesMessageSent = DateTime.Now.ToString()
            });
        }

        protected void OnPropertyChanged(string name) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
    }
