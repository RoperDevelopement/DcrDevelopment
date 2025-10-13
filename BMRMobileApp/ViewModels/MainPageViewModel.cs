using BMRMobileApp.Models;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Syncfusion.Maui.Toolkit.NavigationDrawer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
 

namespace BMRMobileApp.ViewModels
{
    public class MainPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SQLiteDBCommands.SQLiteService sqLCmd = new SQLiteDBCommands.SQLiteService();
        private int currentIndex;
        private bool isVisibLeLeftArrow;
        private bool isVisibeRIghtArrow;
        private bool headerVisable;
        private bool isExpanded;
        private string stopStartAS;
        private Color borderColor;
        private string taskMood;
        private bool headerButtonsVisable;
        private bool isToggled;
        private string totalTask;
        private bool autoScroll;
        ObservableCollection<PSTaskTags> taskTags = new ObservableCollection<PSTaskTags>();
        //private ObservableCollection<PSUserTask> userTask;
        private TaskProjectsModel pSUserTask;
        private CancellationTokenSource _scrollTokenSource;
        public ICommand NextImageCommand { get; }
        public ICommand ExpanderChangedCommand { get; }
        //   public ICommand ExpanderChangedCommand { get; }
        public ICommand StartStopScrollingCommand { get; }
        public ICommand CloseTask { get; }
        // private IDispatcherTimer _timer;
        //  public ICommand NextTask { get; }
        //  public ICommand ItemChangedCommand { get; set; }
        //    public ICommand PositionChangedCommand { get; set; }
        public MainPageViewModel()
        {
            GetTask();
           
            // NextTask = new Command(StopAutoScrollLoop);
            NextImageCommand = new RelayCommand<string>(NextImage);
            StartStopScrollingCommand = new RelayCommand(OnClickStartStopScrollingCommand);
            CloseTask = new Command<int>(OnCLickCloseTask);
            ExpanderChangedCommand = new Command<bool>(OnExpanded);
            // ExpanderChangedCommand = new Command<object>(OnExpanded);
            //  ItemChangedCommand= new Command<PSUserTask>(ItemChanged);
            //  ICommand PositionChangedCommand = new Command<PositionChangedEventArgs>(PositionChanged);
            // StartAutoScroll();
            ExitMain = false;
            


            //// Initialize and start the timer
            //_timer = Application.Current.Dispatcher.CreateTimer();
            //_timer.Interval = TimeSpan.FromSeconds(3); // Change image every 3 seconds
            //_timer.Tick += OnTimerTick;
            //_timer.Start();

            //  StartAutoScrollLoop();
            //   ExecuteAutoScrollOnce();
        }
        //    public PSUserTask PreviousMonkey { get; set; }
        //   public PSUserTask CurrentMonkey { get; set; }
        /// <summary>
        ///  public PSUserTask CurrentItem { get; set; }
        /// </summary>
        /// 
        private async void StartScroll()
        {
           
            CurrentPosition = 0;
            isVisibeRIghtArrow = true;
            HeaderVisable = false;
            HeaderButtonsVisable = false;
            if ((LTask != null) && (LTask.Count > 0))
            {
                GetSettings();
                SetBoarderColor(Utilites.Consts.DefaultBackgroundColor);
                HeaderVisable = true;
                GetPsTask();
                isVisibeRIghtArrow = false;
                isVisibLeLeftArrow = false;
                IsExpanded = true;
                HeaderButtonsVisable = true;
               
                IsToggled = false;
                StopStartAS = "Stop Scrolling ⏹";
                StartAutoScroll();


            }
            else
            {
                if (AutoScroll)
                    AutoScroll = false;
            }

        }
        //  public ICommand ExpanderChangedCommand => new Command(() =>
        //  {
        // Handle expansion logic here
        //     IsExpanded = !IsExpanded;
        // });
        private IList<PSEmotionsTag> EmotionsTags { get; set; }
        public int AutoScrollDelay { get; set; } = 3000;
        public bool ExitMain { get; set; }
        public bool AutoScroll 
        {
            get => autoScroll;
            set
            {
                autoScroll = value;
                OnPropertyChanged(nameof(AutoScroll));
            }
            } 

        ////{
        ////    get => isAutoScroll;

        ////    set
        ////    {
        ////        isAutoScroll = value;
        ////        //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
        ////        OnPropertyChanged(nameof(AutoScroll));
        ////    }
        ////}
        ////public bool AutoScroll

        ////{
        ////    get => isAutoScroll;

        ////    set
        ////    {
        ////        isAutoScroll = value;
        ////        //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
        ////        OnPropertyChanged(nameof(AutoScroll));
        ////    }
        ////}

        public int PreviousPosition { get; set; }
        public int CurrentPosition { get; set; }

        private IList<PSTaskTags> LTaskTags { get; set; }
        [ObservableProperty]
        public ObservableCollection<PSTaskTags> TaskTags
        {
            get => taskTags;
            set
            {
                taskTags = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(TaskTags));
            }
        }

        public string TotalTask

        {
            get => totalTask
;
            set
            {
                totalTask = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(TotalTask));
            }
        }
        public string TaskMood
        {
            get => taskMood;
            set
            {
                taskMood = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(TaskMood));
            }
        }
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(BorderColor));
            }
        }
        [ObservableProperty]
        public string StopStartAS
        {
            get => stopStartAS;
            set
            {
                stopStartAS = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(StopStartAS));
            }
        }

        [ObservableProperty]
        public bool IsToggled
        {
            get => isToggled;
            set
            {
                isToggled = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(IsToggled));
            }
        }
        [ObservableProperty]
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(IsExpanded));
            }
        }
        [ObservableProperty]
        public TaskProjectsModel PSUserTask
        {
            get => pSUserTask;
            set
            {
                pSUserTask = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(PSUserTask));
            }
        }
        private async void GetSettings()
        {
            AutoScrollDelay = Utilites.Consts.DefautAS;
           var psUserSetting  =   sqLCmd.GetPsSettingsNonAsync();
           
            if(psUserSetting != null)
            {
                if(psUserSetting.AutoScroll == 1)
                {
                    AutoScroll = true;
                }
                AutoScrollDelay = sqLCmd.GetDelyTime(psUserSetting.ScrollWaits);
            }
            
            await Task.CompletedTask;
        }
        //[ObservableProperty]
        ////   public ObservableCollection<PSUserTask> UserTask { get; set; }
        //public ObservableCollection<PSUserTask> UserTask
        //{
        //    get => userTask;
        //    set
        //    {
        //        userTask = value;
        //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
        //        OnPropertyChanged(nameof(UserTask));
        //    }

        //}
        [ObservableProperty]
        public bool IsVisibLeLeftArrow
        {

            get => isVisibLeLeftArrow;
            set
            {
                isVisibLeLeftArrow = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(IsVisibLeLeftArrow));
            }
        }
        [ObservableProperty]
        public bool HeaderButtonsVisable
        {

            get => headerButtonsVisable;
            set
            {
                headerButtonsVisable = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(HeaderButtonsVisable));
            }
        }

        [ObservableProperty]
        public bool HeaderVisable
        {

            get => headerVisable;
            set
            {
                headerVisable = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(HeaderVisable));
            }
        }
        [ObservableProperty]
        public bool IsVisibeRIghtArrow
        {

            get => isVisibeRIghtArrow;
            set
            {
                isVisibeRIghtArrow = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(IsVisibeRIghtArrow));
            }
        }

        private async void SetBoarderColor(string colorBoarder)
        {
            BorderColor = Color.FromArgb(colorBoarder);
        }
        public IList<TaskProjectsModel> LTask { get; set; }

        public void RefreshCollObj(string propName)
        {
            OnPropertyChanged(nameof(propName)); // Forces UI to rebind
        }
        private async void GetPsTask()
        {
            if ((LTask == null) || (LTask.Count == 0))
            {
                AutoScroll = false;
                return;
            }

            IsToggled = false;
            var dueDate = GetDaysLeftTask(LTask[CurrentIndex].TaskDateAdded, LTask[CurrentIndex].TaskDueDate);
            PSUserTask = new TaskProjectsModel
            {
                ID = LTask[CurrentIndex].ID,
                TaskName = LTask[CurrentIndex].TaskName,
                TaskDescription = LTask[CurrentIndex].TaskDescription,
                TaskCategoryName = LTask[CurrentIndex].TaskCategoryName,
                TaskDueDate = LTask[CurrentIndex].TaskDueDate,
                TaskDateDone = dueDate.ToString(),
                TaskDateAdded = LTask[CurrentIndex].TaskDateAdded
            };
            if (LTask[CurrentIndex].TaskNotfication == 1)
            {
                IsToggled = true;

            }

            AddTaslTags(LTask[CurrentIndex].ID);
            AddEmoTags(LTask[CurrentIndex].ID);
            RefreshCollObj(nameof(PSUserTask));
            RefreshCollObj(nameof(IsToggled));
        }
        private async void GetTask()
        {
            LTask = sqLCmd.GetPSUserTaskNonTasl<TaskProjectsModel>(SQLiteDBCommands.SqlLiteSelectStatements.SelectOpenTask);
            LTaskTags = sqLCmd.GetPSUserTaskNonTasl<PSTaskTags>(SQLiteDBCommands.SqlLiteSelectStatements.SelectTagsIDs);
            EmotionsTags = sqLCmd.GetPSUserTaskNonTasl<PSEmotionsTag>(SQLiteDBCommands.SqlLiteSelectStatements.SelectEmotions);
            TotalTask = $"Task ({LTask.Count.ToString()})";
            StartScroll();
            //   UserTask = Utilites.EnumerableExtensions.ToObservableCollectionNonAsync<PSUserTask>(LTask);
            //  RefreshCollObj(nameof(UserTask));
        }
        private async void AddTaslTags(int id)
        {
            if ((LTask != null) && (LTask.Count > 0))
            {
                TaskTags.Clear();
                var tags = LTaskTags.Where(p => p.ID == id);
                if (tags.Any())
                {
                    foreach (var tag in tags)
                    {
                        TaskTags.Add(tag);
                    }
                    RefreshCollObj(nameof(TaskTags));
                }
            }
        }
        private async void AddEmoTags(int id)
        {
            if ((EmotionsTags != null) && (EmotionsTags.Count > 0))
            {

                var tags = EmotionsTags.Where(p => p.ID == id);
                if (tags.Any())
                {
                    foreach (var tag in tags)
                    {
                        SetBoarderColor(tag.EmotionsColor);
                        TaskMood = $" {tag.Emotion} {tag.EmotionIcon}";
                    }

                }
                else
                {
                    TaskMood = $" Uncertain 👀";
                    SetBoarderColor(Utilites.Consts.DefaultBackgroundColor);
                }

            }
        }

        private int GetDaysLeftTask(string sDate, string edDate)
        {
            //   DateTime startDate = DateTime.Parse(sDate);
            // DateTime endDate = DateTime.Parse(edDate);
            //   DateTime startDate = DateTime.Parse(sDate);
            DateTime endDate = DateTime.Parse(edDate);
            DateTime startDate = DateTime.Now;
            // Calculate the difference
            TimeSpan difference = endDate - startDate;

            // Get the total days
            return difference.Days;
        }
        public int CurrentIndex { get; set; }


        private async void StartAutoScroll()
        {
            try
            {
                while (AutoScroll)
                {
                    if (!(AutoScroll))
                        break;
                    if (ExitMain)
                        break;
                    // Thread.Sleep(3000);
                    await Task.Delay(AutoScrollDelay); // scroll every 3 seconds
                                            //CurrentIndex = (CurrentIndex + 1) % LTask.Count();
                                            //await Task.Delay(3000); // scroll every 3 seconds
                    CurrentIndex++;
                    //if (CurrentIndex < LTask.Count() - 1)
                    //  CurrentIndex++;
                    if (CurrentIndex > LTask.Count - 1)
                        CurrentIndex = 0;

                    GetPsTask();
                  
                        
                    //  RefreshCollObj(nameof(CurrentIndex));

                    // RefreshCollObj(nameof(UserTask));
                    // await Task.Delay(30000); // scroll every 3 seconds
                    // break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //public void StopTimer()
        //{
        //    _timer?.Stop();
        //}
        public async void NextImage(string rightLeft)
        {
            CurrentIndex++;
            if (CurrentIndex > LTask.Count - 1)
                CurrentIndex = 0; // 
            else
            {
                IsVisibeRIghtArrow = true;
                IsVisibLeLeftArrow = true;
                RefreshCollObj(nameof(IsVisibeRIghtArrow));
                RefreshCollObj(nameof(IsVisibLeLeftArrow));
            }
            GetPsTask();





            //  if (CurrentIndex < UserTask.Count() - 1)
            //       CurrentIndex++;
            // else
            //     CurrentIndex = 0; //
            //MainThread.BeginInvokeOnMainThread(() =>
            //{
            //    var dispatcher = Application.Current?.MainPage?.Dispatcher;
            //    dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            //    {
            //        CurrentIndex = (CurrentIndex + 1) % UserTask.Count;
            //        // ⛔ Return false to stop after one scroll
            //        return false;
            //    });
            //});
        }
        public async void OnClickStartStopScrollingCommand()
        {
            if (AutoScroll)
            {
                AutoScroll = false;
                StopStartAS = "Start Scrolling 📜";
                IsVisibeRIghtArrow = true;
                IsVisibLeLeftArrow = false;
                IsExpanded = false;
                RefreshCollObj(nameof(IsVisibeRIghtArrow));
                RefreshCollObj(nameof(IsVisibLeLeftArrow));
                RefreshCollObj(nameof(IsExpanded));
            }
            else
            {
                AutoScroll = true;
                IsVisibeRIghtArrow = false;
                IsVisibLeLeftArrow = false;
                IsExpanded = true;
                RefreshCollObj(nameof(IsVisibeRIghtArrow));
                RefreshCollObj(nameof(IsVisibLeLeftArrow));
                RefreshCollObj(nameof(IsExpanded));
                // CurrentIndex = 0;
                StartScroll();
            }

        }

        protected void OnPropertyChanged(string name = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Change the OnExpanded method signature from:
        // public async void OnExpanded(ExpandedChangedEventArgs e)

        // To this:


        //  public async void OnExpanded(object? e)
        // {
        //   if (e == null) return;
        //var j = e.IsExpanded;
        // }
        public async void OnExpanded(bool isExpanded)
        {


            HeaderButtonsVisable = isExpanded;

            RefreshCollObj(nameof(HeaderButtonsVisable));
        }

        public async void OnCLickCloseTask(int id)
        {
            var l = LTask.Where(p => p.ID == id).ToList();
            if (l.Any())
            {
                bool answerv = await Application.Current.MainPage.DisplayAlert("Confrim", $"Close Task {l[0].TaskName}", "Yes", "No");
                if (!(answerv))
                {
                    // GetConfirm().Wait();
                    return;
                }
                var st = string.Format(SQLiteDBCommands.SqlLiteSelectStatements.SqlCloseTask, id);
                await sqLCmd.UpDateTable(st);
                CurrentIndex = 0;
                GetTask();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error:", $"Could not get information for id {id}", "OK");
            }

            //  public async void OnExpanderChanged(object sender)
            //{
            //    Console.WriteLine();
            // Call your ViewModel command here if needed, e.g.:
            // (BindingContext as MainPageViewModel)?.ExpanderChangedCommand.Execute((sender as Expander)?.BindingContext);
            //  }
            // Change this line:
            // ExpanderChangedCommand = new RelayCommand<ExpandedChangedEventArgs>(OnExpanded);

            // To this:

        }
    }
}