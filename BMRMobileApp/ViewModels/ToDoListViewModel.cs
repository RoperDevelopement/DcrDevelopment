using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
 

namespace BMRMobileApp.ViewModels
{
    public partial class ToDoListViewModel : INotifyPropertyChanged
    {
        public readonly SpeechToTextUtility speechUtility = new();
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SQLiteDBCommands.SQLiteService sqLCmd = new SQLiteDBCommands.SQLiteService();
        ObservableCollection<PSIconTask> pSIconTasks = new ObservableCollection<PSIconTask>();
        ObservableCollection<PSTaskTags> taskTags = new ObservableCollection<PSTaskTags>();
        string cPText;
        Color cPBC;


        private readonly RandomBackGroundColor backGroundColor = new RandomBackGroundColor();
        public ICommand ExitToDoCommand { get; }
        public ICommand SaveToDoCommand { get; }
        public ICommand StartVoiceRecordingCommand { get; }
        public ICommand StopVoiceRecordingCommand { get; }
        public ICommand ClearTextCommand { get; }
        public ICommand TaskTagsCommand { get; set; }
        private bool sendNotications;
        public ICommand TagChangeTagColor { get; set; }
        public ICommand EmoTag { get; set; }
        private bool isSpeaking = false;
        private bool isNotSpeaking = false;
        private Color iconBackGround;
        private PSTaskTags selectedItem;
        private string taskDueDate;
        private string moodTask;
        private Color randomColor;
        private Color randomColorTT;
        public ToDoListViewModel()
        {
         //    PSUserTaskProjects = new PSUserTask();
            ExitToDoCommand = new Command(OnExitCommand);
            SaveToDoCommand = new Command(OnTaskSave);
            ClearTextCommand = new Command(OnClearTextClicked);
            TaskTagsCommand = new Command<PSIconTask>(OnTaskTagClicked);
            TagChangeTagColor = new Command<PSTaskTags>(OnChangeTageColor);
            IsNotSpeaking = false;
            IsSpeaking = true;
            StartVoiceRecordingCommand = new Command(OnStartRecordingClicked);
            StopVoiceRecordingCommand = new Command(OnSStopRecordingClicked);
            EmoTag = new Command<PSEmotionsTag>(OnClickEmoTag);
            speechUtility.PartialResultReceived += OnPartialResult;
            speechUtility.FinalResultReceived += OnFinalResult;
            IconBackGround = Color.FromArgb("#696969");
            SendNotications = false;
            TaskDueDate = $"Date Task Due {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
            MoodTask = "Select an emotion";
            _ = Init();
            GetMood();
        }
        private async void GetMood()
        {
            CPText = "Task ";
            CPBC = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
            var mood = sqLCmd.GetUserCurrentMoodNonAsync();
            if (mood != null)
            {
                CPText = $"{CPText} {mood.Mood} {mood.MoodTag}";
                CPBC = Color.FromArgb(mood.BackgroundColor);
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

        public async Task Init()
        {

            //var proj = await sqLCmd.GetTAsync<PSUserProjects>();
            //Projects = await Utilites.EnumerableExtensions.ToObservableCollection<PSUserProjects>(proj);
         //   var icons = await sqLCmd.GetTAsync<PSIconTask>();
            //TaskIcons = await Utilites.EnumerableExtensions.ToObservableCollection<PSIconTask>(icons);

            var cat = await sqLCmd.GetTAsync<PSTaskCatgory>();
            Catgories = await Utilites.EnumerableExtensions.ToObservableCollection<PSTaskCatgory>(cat);
            PSTasksTagColors = await sqLCmd.GetTAsync<PSTaskTags>();
            //  TaskTags = await Utilites.EnumerableExtensions.ToObservableCollection<PSTaskTags>(PSTasksTagColors);
            //  await ChangeToDefaultColor();
            PSEmotionsTagsList = await sqLCmd.GetTAsync<PSEmotionsTag>();
            EmotionsTag = await Utilites.EnumerableExtensions.ToObservableCollection<PSEmotionsTag>(PSEmotionsTagsList);
            await SetTaskTagColor();
            await CreateEmColl();
            PSEmotionsTagsList = await sqLCmd.GetTAsync<PSEmotionsTag>();
            RandomColorEditor = await backGroundColor.RandomColor();
            RefreshCollObj(nameof(RandomColorEditor));
             


        }
      
        private async Task CreateEmColl()
        {
             foreach(var item in EmotionsTag)
            {
                item.EmotionsColor = Utilites.EmojiTags.DefaultTagColor;
            }
            RefreshCollObj(nameof(EmotionsTag));


        }
        
        #region ObservableCollection
        [ObservableProperty]
        public ObservableCollection<PSEmotionsTag> EmotionsTag { get; set; }
        [ObservableProperty]
        public ObservableCollection<PSTaskTags> TaskTags { get; set; } = new();
       [ObservableProperty]
        private IList<PSTaskTags> PSTasksTagColors { get; set; }
        [ObservableProperty]
        public ObservableCollection<PSTaskCatgory> Catgories { get; set; }
        #endregion
        #region ObservableCollection Prop
        private int PrevEmTag { get; set; } = -1;

        public Color RandomColorTT
        {
            get => randomColorTT;
            set
            {
                randomColorTT = value;
                OnPropertyChanged(nameof(RandomColorTT));
            }
        }
        public string MoodTask
        {
            get => moodTask;
            set
            {
                moodTask = value;
                OnPropertyChanged(nameof(MoodTask));
            }
        }
      
        public string TaskDueDate
        {

            get => taskDueDate;
            set
            {
                taskDueDate = value;
                OnPropertyChanged(nameof(TaskDueDate));
            }
        }
        private string OldTaskColorTagName { get; set; }=string.Empty;
        
            public Color RandomColor
        {
            get => randomColor;
            set
            {
                randomColor = value;
                OnPropertyChanged(nameof(RandomColor));
            }
        }
        public Color IconBackGround
        {
            get => iconBackGround;
            set
            {
                iconBackGround = value;
                OnPropertyChanged(nameof(IconBackGround));
            }
        }
        public bool SendNotications
        {
            get => sendNotications;
            set
            {
                sendNotications = value;
                OnPropertyChanged(nameof(SendNotications));
            }
        }
        #endregion
        private PSUserTask PSUserTaskProjects { get; set; }   = new PSUserTask();
        public IList<PSEmotionsTag> PSEmotionsTagsList { get; set; }


        public string SelectedTime { get; set; }
        public string SelectedDate { get; set; }
        public bool TaskDescriptionFocused { get; set; }
        public bool IsNotSpeaking
        {
            get => isNotSpeaking;
            set
            {
                isNotSpeaking = value;
                OnPropertyChanged(nameof(IsNotSpeaking));
            }
        }
        private string TagDefaultColor { get; set; }
        public bool IsSpeaking
        {
            get => isSpeaking;
            set
            {
                isSpeaking = value;
                OnPropertyChanged(nameof(IsSpeaking));
            }
        }

        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;

        //public ObservableCollection<PSTaskTags> TaskTags
        //{
        //    get => taskTags;
        //    set
        //    {
        //        taskTags = value;
        //        OnPropertyChanged(nameof(TaskTags));
        //    }
        //}
        //public PSTaskTags SelectedItem
        //{
        //    get => selectedItem;
        //    set
        //    {
        //        selectedItem = value;
        //        OnPropertyChanged();
        //        var index = TaskTags.IndexOf(value);
        //       // SelectedIndex = index;
        //    }
        //}

        //[ObservableProperty]
        //public ObservableCollection<PSIconTask> TaskIcons
        //{
        //    get => pSIconTasks;
        //    set
        //    {

        //        pSIconTasks = value;
        //        OnPropertyChanged(nameof(TaskIcons));
        //    }
        //}

        [ObservableProperty]
        public PSTaskCatgory SelectedCatagory { get; set; } = new PSTaskCatgory();

       
        private async Task ChangeToDefaultColor()
        {
            //   TagDefaultColor = PSTasksTagColors.Select(p => p.TagColor == "DefaultColor").ToString();

        }
        private IList<int> AddedTagColor { get; set; } = new List<int>();
        private async Task SetTaskTagColor(int index = 0)
        {
            if ((TaskTags == null) || (TaskTags.Count() == 0))

            {
                TaskTags = new ObservableCollection<PSTaskTags>();
                for (int i = 0; i < PSTasksTagColors.Count(); i++)
                {
                    TaskTags.Add(new PSTaskTags { ID = PSTasksTagColors[i].ID, TagName = PSTasksTagColors[i].TagName, TagColor = Utilites.EmojiTags.DefaultTagColor });
                }
            }
            else
            {
                // TaskTags[index].TagColor = PSTasksTagColors[index].TagColor;
                TaskTags[0].TagColor = "#848884";
                OnPropertyChanged(nameof(TaskTags));
            }
        }
        //private int _selectedIndex;
        //public int SelectedIndex
        //{
        //    get => _selectedIndex;
        //    set
        //    {
        //        _selectedIndex = value;
        //        OnPropertyChanged();
        //    }
        //}
       
        private async void OnTaskSave()
        {
      


            InitTaskProj();
            if (string.IsNullOrEmpty(TaskName))
            {
              await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Need a Task Name", "Missing Task Name", "OK");
                return;
            }
            if(SelectedCatagory.ID == 0)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Need a Category", "Missing Category", "OK");
                return;
            }

            PSUserTaskProjects.TaskDueDate = $"{SelectedDate} {SelectedTime}";
            PSUserTaskProjects.TaskDateAdded = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
            PSUserTaskProjects.IsTaskComplete = 0;
            PSUserTaskProjects.TaskCategoryID = SelectedCatagory.ID;
            
                PSUserTaskProjects.TaskName = TaskName;
            if (!string.IsNullOrEmpty(TaskDescription))
                PSUserTaskProjects.TaskDescription = TaskDescription;
            if (SendNotications)
                PSUserTaskProjects.TaskNotfication = 1;
            await sqLCmd.InsetRec<PSUserTask>(PSUserTaskProjects);
            AddTags();
            if (Microsoft.Maui.Controls.Application.Current?.Windows.Count > 0 && Microsoft.Maui.Controls.Application.Current.Windows[0] is Window window)
            {

                window.Page = new ToDoListPage();
            }
        }
        private async void AddTags()
        {
            if((AddedTagColor != null) && (AddedTagColor.Count > 0))
            {
                var maxId = await sqLCmd.GetMaxId(nameof(PSUserTask));
                foreach(int i in AddedTagColor)
                {
                    PSTaskTagIDs pSTaskTagIDs = new PSTaskTagIDs{ ID = maxId, TagID = i };
                   await sqLCmd.InsetRec<PSTaskTagIDs>(pSTaskTagIDs);
                }
                await sqLCmd.UpdatePSUserTaskTagID(maxId);
                
            }
        }
        private async void InitTaskProj()
        {
            PSUserTaskProjects.TaskName = Utilites.Consts.NA;
            PSUserTaskProjects.TaskDescription =  Utilites.Consts.NA;
            PSUserTaskProjects.TaskDateDone = Utilites.Consts.NA;
            

        }
        private async void OnExitCommand()
        {
            if (Microsoft.Maui.Controls.Application.Current?.Windows.Count > 0 && Microsoft.Maui.Controls.Application.Current.Windows[0] is Window window)
            {
                window.Page = new AppShell();
            }


        }
        [RelayCommand]
        private async void OnTaskTagClicked(PSIconTask pSIcon)
        {
            if (Microsoft.Maui.Controls.Application.Current?.Windows.Count > 0 && Microsoft.Maui.Controls.Application.Current.Windows[0] is Window window)
            {
                window.Page = new ToDoListPage();
            }

        }
        private async void OnClearTextClicked()
        {

        }
        // public async Task<ObservableCollection<T>> ToObservableCollection<T>(this IEnumerable<T> source)
        // {
        //   return new ObservableCollection<T>(source);
        // }
        protected void OnPropertyChanged(string name = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async void OnStartRecordingClicked()
        {
            
            //ButtonStartListing.IsVisible = false;
            //ButtonStopListing.IsVisible = true;
            //  await ListenAndFillTextBox(null);

            IsSpeaking = false;
            IsNotSpeaking = true;

            await StartVoiceJournalAsync();

            //  await SpeakText(JournalEditor.Text);
        }
        public async Task StartVoiceJournalAsync()
        {
            var ready = await speechUtility.InitializeAsync(CancellationToken.None);
            if (!ready) return;


            //  speechUtility.PartialResultReceived += (s, text) => UpdateLiveTranscript(text);
            //  speechUtility.FinalResultReceived += (s, text) => SaveFinalTranscript(text);

            await speechUtility.StartListeningAsync();
        }
        public async Task StopVoiceJournalAsync()
        {
            await speechUtility.StopListeningAsync();
        }
        private void OnPartialResult(object? sender, string text)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (string.IsNullOrEmpty(text)) return;

                    TaskName = text;

                });
                // Live updates while speaking

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void OnSStopRecordingClicked()
        {

            //ButtonStartListing.IsVisible = true;
            //ButtonStopListing.IsVisible = false;
            //  await ListenAndFillTextBox(null);
            StopRecording();
            //  await SpeakText(JournalEditor.Text);
        }
        public Color RandomColorEditor { get; set; }
        public async void StopRecording()
        {
            IsSpeaking = true;
            IsNotSpeaking = false;


            await StopVoiceJournalAsync();
        }
        private void OnFinalResult(object? sender, string text)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!(string.IsNullOrEmpty(text)))
                    {

                        TaskName = text;

                    }

                });   // Final transcript after stop

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Optional: Save to journal, tag emotion, trigger animation
            //  SaveTranscript(text);
        }
        public void UnRegisterEvents()
        {
            speechUtility.PartialResultReceived -= OnPartialResult;
            speechUtility.FinalResultReceived -= OnFinalResult;
        }
        private async void OnClickEmoTag(PSEmotionsTag tag)
        {
            var  index = EmotionsTag.IndexOf(tag);
            var newTag = PSEmotionsTagsList.FirstOrDefault(x => x.ID == tag.ID);
            RandomColor = await backGroundColor.RandomColor();
            if (PrevEmTag >= 0)
            {

                var prevTagInfo = EmotionsTag[PrevEmTag];
                EmotionsTag.RemoveAt(PrevEmTag);
                EmotionsTag.Insert(PrevEmTag,new PSEmotionsTag { Emotion = prevTagInfo.Emotion, ID = prevTagInfo.ID, EmotionIcon = prevTagInfo.EmotionIcon, EmotionsColor = Utilites.EmojiTags.DefaultTagColor });
             }
            PSUserTaskProjects.TaskEmotionsID = tag.ID;
            MoodTask = $"Your emotion for this task is {newTag.Emotion} {newTag.EmotionIcon}";
            PrevEmTag = index;
            EmotionsTag.RemoveAt(PrevEmTag);
            EmotionsTag.Insert(PrevEmTag, new PSEmotionsTag { Emotion = newTag.Emotion, ID = newTag.ID, EmotionIcon = newTag.EmotionIcon, EmotionsColor = newTag.EmotionsColor });
            //    EmotionsTag[index].EmotionsColor = newTag.EmotionsColor;
            RefreshCollObj(nameof(EmotionsTag));


        }
        private async void OnChangeTageColor(PSTaskTags pSTask)
        {
            RandomColorTT = await backGroundColor.RandomColor();
            var index = TaskTags.IndexOf(pSTask);
            var psIndex = pSTask.ID;
            var newColor = PSTasksTagColors.FirstOrDefault(x => x.ID == pSTask.ID);
            var tt = TaskTags[index].TagColor;
            TaskTags.RemoveAt(index);
            if ( tt != newColor.TagColor)
            {
                TaskTags.Insert(index, new PSTaskTags { ID = newColor.ID, TagName = newColor.TagName, TagColor = newColor.TagColor });
                AddedTagColor.Add(newColor.ID);
            }

            else
            {
                TaskTags.Insert(index, new PSTaskTags { ID = newColor.ID, TagName = newColor.TagName, TagColor = Utilites.EmojiTags.DefaultTagColor });
                AddedTagColor.Remove(newColor.ID);
            }
            //OldTaskColorTagName = TaskTags[index].TagName;

            //  TaskTags[index].TagColor = "#848884";
            // SelectedItem = pSTask;
            //TaskTags[SelectedIndex].TagColor = "#848884";
            // Console.WriteLine(SelectedIndex);

            //for (int i = 0; i < TaskTags.Count; i++)
            //{
            //    if (TaskTags[i].ID == pSTask.ID)
            //    {
            //        TaskTags.RemoveAt(i);


            //        PSTasksTagColors = await sqLCmd.GetTAsync<PSTaskTags>();
            //        var tColorv = PSTasksTagColors.FirstOrDefault(x => x.ID == pSTask.ID);
            //        TaskTags.Insert(i, new PSTaskTags { ID =tColorv.ID,TagName=tColorv.TagName,TagColor=tColorv.TagColor});


            //        //TaskTags = await Utilites.EnumerableExtensions.ToObservableCollection<PSTaskTags>(PSTasksTagColors);

            //       // taskTags[i].TagColor = tColorv.TagColor;
            //       // OnPropertyChanged(nameof(TaskTags));
            //        break;
            //    }
            //}
            // OnPropertyChanged(nameof(TaskTags));
            RefreshCollObj(nameof(TaskTags));
            RefreshCollObj(nameof(RandomColorTT));
        }
        public void RefreshCollObj(string propName)
        {
            OnPropertyChanged(nameof(propName)); // Forces UI to rebind
        }

    }
}
