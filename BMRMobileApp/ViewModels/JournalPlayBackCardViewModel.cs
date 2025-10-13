
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using Microsoft.Graph.TermStore;
 
using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


//bar charts https://devblogs.microsoft.com/xamarin/microcharts-elegant-cross-platform-charts-for-any-app/
namespace BMRMobileApp.ViewModels
{
    public class JournalPlayBackCardView : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int currentPosition;
        private bool isPlaying;
        //private Microsoft.Maui.Timer playbackTimer;
        private System.Threading.Timer playbackTimer;
        private SQLiteService cmd = new SQLiteService();
        string cPText;
        Color cPBC;
        private readonly BarCharts barChart = new BarCharts();
        private Chart _moodChart;
        private string sDateJPB;
        private string eDateJPB;
        private string selectedSite;
        public ICommand StartStopAsCmd { get; }
        private string stopStartAS;
        private string timeSpan;
        private string journalEntriesNumberDays;
        private bool isVisPB;
      
        public JournalPlayBackCardView()
        {
            
            
            //  StartAutoScroll();
           
         Init();
             
            StartStopAsCmd = new Command(StopPlayback);
            SelectedSite = ChartTypes.BarChart.ToString();
        }
        private IList<EmotionsCountModel> EmotionsCountModel { get; set; }
        public async void Init()
        {
            EDateJPB = DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd");
            SDateJPB = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            GetJouranlEntries();
            StopStartAS = "Stop Scrolling ⏹";
            IsVisPB = true;
            GetMood();
            GetSettings();
            GetDateDIff();
            StartPlayback();
            

        }
        [ObservableProperty]
        public string StopStartAS
        {
            get => stopStartAS;
            set
            {
                if (stopStartAS != value)
                { 
                    stopStartAS = value;
                //        //            Console.WriteLine($"Scrolling to position: {currentIndex}");
                OnPropertyChanged(nameof(StopStartAS));
                }
            }
        }
        public IList<string> PickerChartTypes { get; set; } 
        public IList<EmotionsCountModel> MoodEntry { get; set; } = new List<EmotionsCountModel>();
        public string EDateJPB
        {
            get => eDateJPB;
            set
            {
                eDateJPB = value;
                OnPropertyChanged(nameof(EDateJPB));
            }
        }

        
            public string JournalEntriesNumberDays
        {
            get => journalEntriesNumberDays;
            set
            {
                journalEntriesNumberDays = value;
                OnPropertyChanged(nameof(JournalEntriesNumberDays));
            }
        }
        public string SDateJPB
        {
            get => sDateJPB;
            set
            {
                sDateJPB = value;
                OnPropertyChanged(nameof(SDateJPB));
            }
        }
        public Chart MoodChart
        {
            get => _moodChart;
            set
            {
                _moodChart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoodChart)));
            }
        }
        private bool AutoScroll { get; set; }
        public ObservableCollection<JournalEntryCardPlayBack> EntryCardPlayBacks { get; set; }
        public ObservableCollection<JournalEntriesModel> JournalEntries { get; set; }
        public string CPText
        {
            get => cPText;
            set
            {
                cPText = value;
                OnPropertyChanged(nameof(CPText));
            }
        }
        private int AutoScrollDelay { get; set; }
        public Color CPBC
        {
            get => cPBC;
            set
            {
                cPBC = value;
                OnPropertyChanged(nameof(CPBC));
            }
        }
        public bool IsVisPB
        {
            get => isVisPB;
            set
            {
                isVisPB = value;
                OnPropertyChanged(nameof(IsVisPB));
            }
        }
        protected void OnPropertyChanged(string name) =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async void GetDateDIff()
        {
             TimeSpan difference = DateTime.Parse(EDateJPB) - DateTime.Parse(SDateJPB);
            JournalEntriesNumberDays = difference.Days.ToString();
            
            if (JournalEntries.Count == 0)
            {
                JournalEntriesNumberDays = $"No Journals for the last{JournalEntriesNumberDays} Days";
                
                AutoScroll = false;
                IsVisPB =false;
            }
            else
            {



                StartPlayback();
                JournalEntriesNumberDays = $"Journal Play Back for the last {JournalEntriesNumberDays} Days";
                GetMoodEntries();
            }
            if(!AutoScroll)
                StopStartAS = "Start Scrolling 📜";



        }
        public string SelectedSite
        {
            get => selectedSite;
            set
            {
                if (selectedSite != value)
                {
                    selectedSite = value;
                    OnPropertyChanged(nameof(SelectedSite));
                    GetBarChart();

                    //Debug.WriteLine($"Selected mood value: {_selectedMood?.Value}");
                }
            }
        }
        private async void GetBarChart()
        {
            //var me = cmd.GetPSUserTaskNonTasl<EmotionsCountModel>(sel);
            if (MoodEntry.Count > 0)
            {

               // if (SelectedSite == null)
                 


                PickerChartTypes = await barChart.GetChartTypes();
                MoodChart = await barChart.CreateChart(MoodEntry, System.Enum.Parse<ChartTypes>(SelectedSite));
               
                //var entries = me.Select(e => new ChartEntry(me.Count)
                //{
                //    Label = e.Label,
                //    ValueLabel = e.ValueLabel,
                //    Color = GetColorForEmotion(e.EmotionsColor)
                //});
                //foreach (EmotionsCountModel item in me)
                //    MoodEntry.Add(new EmotionsCountModel { Label = item.Label, ValueLabel = item.ValueLabel, EmotionsColor = $"{SKColor.Parse(item.EmotionsColor)}" });
                //MoodChart = new RadialGaugeChart { Entries = (IEnumerable<ChartEntry>)MoodEntry };
                //MoodChart = new Microcharts.RadialGaugeChart { Entries = entries };
            }
        }
        private async void GetMoodEntries()
        {
            MoodEntry.Clear();
            string sel = string.Format(SqlLiteSelectStatements.SelectEmotionsCount, SDateJPB, EDateJPB);
            MoodEntry = cmd.GetPSUserTaskNonTasl<EmotionsCountModel>(sel);
           
            


        }
        private SKColor GetColorForEmotion(string emotion)
        {
          //  return SKColors.Red;
             return SKColor.Parse(emotion);
        }
        public int CurrentPosition
        {
            get => currentPosition;
            set => SetProperty(ref currentPosition, value);
        }
        private async void GetMood()
        {

            SQLiteDBCommands.SQLiteService sQLiteDB = new SQLiteDBCommands.SQLiteService();
            var mood = sQLiteDB.GetUserCurrentMoodNonAsync();
            CPText = "Journal Play Back";
            CPBC = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
            if (mood != null)
            {
                CPText = $"{CPText} {mood.Mood} {mood.MoodTag}";
                CPBC = Color.FromArgb(mood.BackgroundColor);
            }
        }

        private async void StartPlayback()
        {
            if (AutoScroll)
            {
                playbackTimer = new System.Threading.Timer(_ =>
                {
                    if (CurrentPosition < JournalEntries.Count - 1)
                        CurrentPosition++;
                    else
                        CurrentPosition = 0; // Loop back to start
                    OnPropertyChanged(nameof(CurrentPosition));

                }, null, TimeSpan.Zero, TimeSpan.FromSeconds(AutoScrollDelay)); // Adjust interval as needed
            }
        }
        private async void GetSettings()
        {
            AutoScrollDelay = Utilites.Consts.DefautAS;
            var psUserSetting = cmd.GetPsSettingsNonAsync();

            if (psUserSetting != null)
            {
                if (psUserSetting.AutoScroll == 1)
                {
                    AutoScroll = true;
                }
                AutoScrollDelay = cmd.GetDelyTimeHrSec(psUserSetting.ScrollWaits);
            }

            await Task.CompletedTask;
        }
        private async void StartAutoScroll()
        {
            try
            {
                CurrentPosition = 0;
                while (AutoScroll)
                {
                    if (!(AutoScroll))
                        break;

                    // Thread.Sleep(3000);
                    await Task.Delay(3000); // scroll every 3 seconds
                                            //CurrentIndex = (CurrentIndex + 1) % LTask.Count();
                                            //await Task.Delay(3000); // scroll every 3 seconds

                    //if (CurrentIndex < LTask.Count() - 1)
                    CurrentPosition++;
                    if (CurrentPosition > JournalEntries.Count - 1)
                        CurrentPosition = 0;




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
        public async void StopPlayback()
        {
            if (!(AutoScroll))
            {
                AutoScroll = true;
                StartPlayback();
                 StopStartAS = "⏹Stop Scrolling";

            }
            else
            {
                
                playbackTimer?.Dispose();
                AutoScroll = false;
                playbackTimer = null;
                StopStartAS = "📜Start Scrolling";

            }
            OnPropertyChanged(nameof(StopStartAS));
        }
        private async void GetJouranlEntries()
        {
            CurrentPosition = 0;
            string sel = string.Format(SqlLiteSelectStatements.SelectJournalEntries, SDateJPB, EDateJPB);
            var je = cmd.GetPSUserTaskNonTasl<JournalEntriesModel>(sel);
            if ((je == null) || (je.Count == 0))
            {
              //  await   Application.Current.MainPage.DisplayAlert("NO Entries", $"No Journal Entries found", "OK");
                //  Application.Current.MainPage = new AppShell();
             if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
                {

                    window.Page = new AppShell();
                }
           }

         
        //   JournalEntries = await Utilites.EnumerableExtensions.ToObservableCollection<JournalEntriesModel>(je);
        JournalEntries = new ObservableCollection<JournalEntriesModel>();
            sel = string.Format(SqlLiteSelectStatements.SelectJournalEntriesGoals, SDateJPB, EDateJPB);
            var jeg = cmd.GetPSUserTaskNonTasl<PSJounalEntryGoals>(sel);
            foreach (var entry in je)
            {
                if (jeg.Count > 0)
                {
                    var idExist = jeg.FirstOrDefault(p => p.ID == entry.ID);
                    if (idExist != null)
                        entry.JournalEntryGoals = idExist.JournalEntry;
                    else
                        entry.JournalEntryGoals = "No Goals Set";
                }
                else
                    entry.JournalEntryGoals = "No Goals Set";
                JournalEntries.Add(new JournalEntriesModel
                {
                    ID = entry.ID,
                    JournalEntry=entry.JournalEntry,
                    PSUserID = entry.PSUserID,
                    DateAdded = entry.DateAdded,
                    Emotion = entry.Emotion,
                    EmotionIcon = entry.EmotionIcon,
                    EmotionsColor = entry.EmotionsColor,
                    JournalEntryGoals = entry.JournalEntryGoals
                });

            }

        }

    }

}
