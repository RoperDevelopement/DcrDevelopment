using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Scm = AlanoClubInventory.SqlServices;


namespace AlanoClubInventory.ViewModels
{
    public class PrintVolunteerHrsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ItemListModel selectedItem;
        private ObservableCollection<ItemListModel> itemsVol;
        private ObservableCollection<VolunteerHoursModel> volunteers;
        private DateTime sDate;
        private DateTime eDate;
        private ObservableCollection<VolunteerHoursModel> volunteerHours;
        private VolunteerHoursModel selectedVol;
        private ICommand upDateVolHrs;
        private ICommand getVolHrs;
        private bool isViewing;
        private int dataGridRowHrs;
        private int buttonDataGridRow;
        private ICommand printVolHrs;
        private ObservableCollection<VolunteerHoursModel> volunteerTotalHours;
        public PrintVolunteerHrsViewModel()
        {
            GetConnectionStr();
            GetVolNames();
            SDate = DateTime.Now.AddMonths(-1);
            EDate = DateTime.Now;
            IsViewing = false;
            DataGridRowHrs = 10;
            ButtonDataGridRow = 4;
        }
        private int UserID { get; set; }
        public int ButtonDataGridRow
        {
            get => buttonDataGridRow;
            set
            {
                buttonDataGridRow = value;
                OnPropertyChanged(nameof(ButtonDataGridRow));
            }
        }
        public int DataGridRowHrs
        {
            get => dataGridRowHrs;
            set
            {
                dataGridRowHrs = value;
                OnPropertyChanged(nameof(DataGridRowHrs));
            }
        }
        public bool IsViewing
        {
            get => isViewing;
            set
            {
                isViewing = value;
                OnPropertyChanged(nameof(IsViewing));
            }
        }
        private IList<StoredParValuesModel> ParModel { get; set; }

        public ICommand PrintVolHrs
        {
            get
            {
                if (printVolHrs == null)
                    printVolHrs = new RelayCommd(VolHrsPrint, parma => true);
                //        //delCategory.Execute(true);
                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return printVolHrs;
            }
        }
        public ICommand GetVolHrs
        {
            get
            {
                if (getVolHrs == null)
                    getVolHrs = new RelayCommd(VolHrs, parma => true);
                //        //delCategory.Execute(true);
                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return getVolHrs;
            }
        }
        public ICommand UpDateVolHrs
        {
            get
            {
                if (upDateVolHrs == null)
                    upDateVolHrs = new RelayCommd(VolHrsUpdate, parma => true);
                //        //delCategory.Execute(true);
                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return upDateVolHrs;

            }

        }
        public VolunteerHoursModel SelectedVol
        {
            get
            {
                if (selectedVol == null)
                    selectedVol = new VolunteerHoursModel();
                return selectedVol;
            }
            set
            {
                selectedVol = value;
                OnPropertyChanged(nameof(SelectedVol));
            }
        }
        public ObservableCollection<VolunteerHoursModel> VolunteerHours
        {
            get
            {
                if (volunteerHours == null)
                {
                    volunteerHours = new ObservableCollection<VolunteerHoursModel>();
                }
                return volunteerHours;
            }
            set
            {
                volunteerHours = value;
                OnPropertyChanged(nameof(VolunteerHours));
            }

        }
        public ObservableCollection<VolunteerHoursModel> VolunteerTotalHours
        {
            get
            {

                if (volunteerTotalHours == null)
                {
                    volunteerTotalHours = new ObservableCollection<VolunteerHoursModel>();
                }
                return volunteerTotalHours;
            }
            set
            {
                volunteerTotalHours = value;
                OnPropertyChanged(nameof(VolunteerTotalHours));
            }
        }
        public DateTime EDate
        {
            get => eDate;
            set
            {
                eDate = value;
                OnPropertyChanged(nameof(EDate));
            }
        }
        public DateTime SDate
        {
            get => sDate;
            set
            {
                sDate = value;
                OnPropertyChanged(nameof(SDate));
            }
        }
        public ObservableCollection<ItemListModel> ItemsVol
        {
            get
            {
                if (itemsVol == null)
                    itemsVol = new ObservableCollection<ItemListModel>();
                return itemsVol;
            }
            set
            {
                itemsVol = value;
                OnPropertyChanged(nameof(ItemsVol));
            }
        }
        private string SqlConnectionStr { get; set; }
        private async void GetConnectionStr()
        {


            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        public ItemListModel SelectedItem
        {
            get
            {
                if (selectedItem == null)
                    selectedItem = new ItemListModel();
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private async void GetVolNames()
        {
            var volNames = Scm.AlClubSqlCommands.SqlCmdInstance.GetACProductsListNonAsc<VolunteerModel>(SqlConnectionStr, Scm.SqlConstProp.SPGetVolunteers);
            if ((volNames != null) && (volNames.Count > 0))
            {
                ItemsVol.Add(new ItemListModel { Label = "Select Volunteer To View Hrs", Value = "0" });
                ItemsVol.Add(new ItemListModel { Label = "View all Volunteer Hrs ", Value = "-1" });
                foreach (var volName in volNames)
                {
                    ItemsVol.Add(new ItemListModel { Label = $"{volName.UserName}-{volName.UserFirstName}-{volName.UserLastName}", Value = volName.ID.ToString() });
                }
                var setlected = ItemsVol.FirstOrDefault(p => p.Value == "-1");
                if (setlected != null)
                    SelectedItem = setlected;
            }
        }
        public async void VolHrsUpdate(object objVolHrs)
        {
            var vhs = objVolHrs as DataGrid;
            vhs.CommitEdit();
            if (vhs != null)
            {
                var hrsVol = vhs.CurrentItem as VolunteerHoursModel;
                if (hrsVol != null)
                {
                    UserID = hrsVol.ID;
                    var sNewdate = ALanoClubUtilites.IsValidDateTime(hrsVol.DateTimeClockedIn.ToString());
                    if (string.IsNullOrEmpty(sNewdate))
                    {
                        ALanoClubUtilites.ShowMessageBox($"In valid Clock In Date Time {hrsVol.DateTimeClockedIn}", "Clock In Time", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var eNewdate = ALanoClubUtilites.IsValidDateTime(hrsVol.DateTimeClockedOut.ToString());
                    if (string.IsNullOrEmpty(eNewdate))
                    {
                        ALanoClubUtilites.ShowMessageBox($"In valid Clock Out Date Time {hrsVol.DateTimeClockedOut}", "Clock In Time", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(hrsVol.UserName))
                    {
                        ALanoClubUtilites.ShowMessageBox($"Need a UserName", "User Name", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var mess = ALanoClubUtilites.IsValidDateRange(hrsVol.DateTimeClockedIn, hrsVol.DateTimeClockedOut);
                    if (!(string.IsNullOrWhiteSpace(mess)))
                    {
                        ALanoClubUtilites.ShowMessageBox($"Invalid Date Time Message {mess}", "Invalid Date Time", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    
                        GetUserId(hrsVol.UserName.Trim());
                        if ((UserID == 0) || (UserID == int.MaxValue))
                        {

                            ALanoClubUtilites.ShowMessageBox($"User Name Not Found {hrsVol.UserName}", "Invalid User Name", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }
                    
                   
                    //if (CheckDupDatesTimes(hrsVol.UserName.Trim(), hrsVol.DateTimeClockedIn, hrsVol.DateTimeClockedOut))
                    //{
                    //    ALanoClubUtilites.ShowMessageBox($"User Name {hrsVol.UserName} found with same clock in date {hrsVol.DateTimeClockedIn} and clock out date {hrsVol.DateTimeClockedOut}", "Duplicate Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                    if(await ALanoClubUtilites.CalcVolTotalHours(hrsVol.DateTimeClockedIn, hrsVol.DateTimeClockedOut) == 0.00)
                    {
                        ALanoClubUtilites.ShowMessageBox($"Clock In Date Time {hrsVol.DateTimeClockedOut} same as Clock Out Date Time {hrsVol.DateTimeClockedOut}", "Same Time", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    AddUpdateTimeParams(hrsVol.DateTimeClockedIn.ToString(), hrsVol.DateTimeClockedOut.ToString(),hrsVol.ID.ToString());
                    await Scm.AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(SqlConnectionStr, Scm.SqlConstProp.SPUpDatVolunteerHours, ParModel);
                    UpdateVolGrid();



                }
                else
                {
                    ALanoClubUtilites.ShowMessageBox($"No Imformation to add", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
        private bool CheckDupDatesTimes(string userName, DateTime dateS, DateTime dateE)
        {
            //var duplicates = VolunteerHours.Where(x => x.TotalHours > 0 && x.DateTimeClockedIn == dateS && x.DateTimeClockedOut == dateE && x.UserName.ToLower().StartsWith(userName.ToLower())).ToList();
            var duplicates = VolunteerHours.Where(x=>x.DateTimeClockedIn == dateS && x.DateTimeClockedOut == dateE && x.UserName.ToLower().StartsWith(userName.ToLower())).ToList();
            if (duplicates.Count > 0)
            {
                return true;
            }
            return false;

        }
        private async void GetUserId(string userName)
        {
            UserID = 0;
            var id = ItemsVol.FirstOrDefault(p => p.Label.ToLower().StartsWith(userName.ToLower()));
            if (id != null)
            {
                UserID = await ALanoClubUtilites.ConvertToInt(id.Value);

            }

        }
        private bool CHeckDate(string userName, DateTime checkSDate, DateTime endDate)
        {
            UserID = 0;
            var id = ItemsVol.FirstOrDefault(p => p.Label.ToLower().StartsWith(userName.ToLower()));

            return false;
        }
        private async void VolHrs(object objVolHrs)
        {
            if (SelectedItem.Value == "0")
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Need to select Volunteer", "VolunteerH", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            UpdateVolGrid();

        }

        private async void UpdateVolGrid()
        {
            AddPSPParams();
            var hrs = await Scm.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<VolunteerHoursModel>(SqlConnectionStr, Scm.SqlConstProp.SPGetPrintVolunteerHours, ParModel);
            if ((hrs != null) && (hrs.Count > 0))
            {
                VolunteerHours = new ObservableCollection<VolunteerHoursModel>();
                foreach (var volHours in hrs)
                {
                    var totHrs = await Utilites.ALanoClubUtilites.CalcVolTotalHours(volHours.DateTimeClockedIn, volHours.DateTimeClockedOut, volHours.TotalHours);
                    VolunteerHours.Add(new VolunteerHoursModel
                    {
                        ID = volHours.ID,
                        UserName = volHours.UserName,
                        UserEmailAddress = volHours.UserEmailAddress,
                        UserPhoneNumber = volHours.UserPhoneNumber,
                        UserFirstName = volHours.UserFirstName,
                        UserLastName = volHours.UserLastName,
                        DateTimeClockedIn = volHours.DateTimeClockedIn,
                        DateTimeClockedOut = volHours.DateTimeClockedOut,
                        TotalHours = totHrs

                    });


                }
                IsViewing = true;
                DataGridRowHrs = 5;
                ButtonDataGridRow = 7;
                var vHrs = await GenerateMonthlyInventorySalesAsync(VolunteerHours.ToList());
                if ((vHrs != null) && vHrs.Count > 0)
                {
                    VolunteerTotalHours = new ObservableCollection<VolunteerHoursModel>(vHrs);
                }


            }
        }

        public async Task<List<VolunteerHoursModel>> GenerateMonthlyInventorySalesAsync(IList<VolunteerHoursModel> alanoClubVol)
        {
            //  var listItems = alanoClubVol.Where(p => p.TotalHours > 0).ToList();
            //var totalHours = alanoClubVol.Where(x => x.DateTimeClockedIn.Date >= SDate.Date && x.DateTimeClockedOut <= EDate.Date && x.TotalHours > 0).Sum(x => (x.TotalHours));
            // var result = alanoClubVol.Where(x => x.DateTimeClockedIn.Date >= SDate.Date && x.DateTimeClockedOut <= EDate.Date && x.TotalHours > 0).GroupBy(x => x.UserName).Select(g => new { UserName = g.Key,TotalHours = g.Sum(p=>p.TotalHours) }).ToList();
            //var result = alanoClubVol.Where(x => x.TotalHours > 0 && x.DateTimeClockedIn.Date >= SDate.Date && x.DateTimeClockedOut.Date <= EDate.Date)
            //   .GroupBy(x => x.UserName).Select(g => new { UserName = g.Key, MinDate = g.Min(x => x.DateTimeClockedIn), MaxDate = g.Max(x => x.DateTimeClockedOut), TotalHours = g.Sum(p=>p.TotalHours), Items = g.ToList() }).ToList();
            var result = alanoClubVol.Where(x => x.TotalHours > 0 && x.DateTimeClockedIn.Date >= SDate.Date && x.DateTimeClockedOut.Date <= EDate.Date)
               .GroupBy(x => x.UserName).Select(g => new VolunteerHoursModel
               {
                   UserName = g.Key,
                   DateTimeClockedIn = g.Min(x => x.DateTimeClockedIn),
                   DateTimeClockedOut = g.Max(x => x.DateTimeClockedOut),
                   ID = g.First().ID,
                   UserFirstName = g.First().UserFirstName,
                   UserLastName = g.First().UserLastName,
                   UserPhoneNumber = g.First().UserPhoneNumber,
                   UserEmailAddress = g.First().UserEmailAddress,
                   TotalHours = g.Sum(p => p.TotalHours)
               }).ToList();
            return result;
        }
        private async void VolHrsPrint(object parma)
        {

        }
        private async void AddPSPParams()
        {
            ParModel = new List<StoredParValuesModel>();
            ParModel.Add(new StoredParValuesModel { ParmaName = Scm.SqlConstProp.SPParmaID, ParmaValue = SelectedItem.Value });
            ParModel.Add(new StoredParValuesModel { ParmaName = Scm.SqlConstProp.SPDateTimeVolClockedIn, ParmaValue = SDate.ToString("MM-dd-yyyy") });
            ParModel.Add(new StoredParValuesModel { ParmaName = Scm.SqlConstProp.SPDateTimeVolClockedOut, ParmaValue = EDate.ToString("MM-dd-yyyy") });

        }
        private async void AddUpdateTimeParams(string cIn,string cOut,string id)
        {
            ParModel = new List<StoredParValuesModel>();
            ParModel.Add(new StoredParValuesModel { ParmaName = Scm.SqlConstProp.SPParmaID, ParmaValue=id.ToString()});
            ParModel.Add(new StoredParValuesModel { ParmaName = Scm.SqlConstProp.SPParmaMemberID, ParmaValue = UserID.ToString() });
            ParModel.Add(new StoredParValuesModel { ParmaName = Scm.SqlConstProp.SPDateTimeVolClockedIn, ParmaValue = cIn });
            ParModel.Add(new StoredParValuesModel { ParmaName = Scm.SqlConstProp.SPDateTimeVolClockedOut, ParmaValue = cOut });

        }
        protected void OnPropertyChanged(string propertyName = null) =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
