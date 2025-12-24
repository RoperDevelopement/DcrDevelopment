using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using OpenTK.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Scmd = AlanoClubInventory.SqlServices;
using AlanoClubInventory.Views;
namespace AlanoClubInventory.ViewModels
{
    public class ReceiptsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private AlanoClubInformaitonModel aclInfor;
        private DateTime recepitDate;
        private ObservableCollection<ItemListModel> itemList;
        private ObservableCollection<ItemListModel> itemListDescPrices;

        private ItemListModel selectedMember;
        private ItemListModel selectedDescPrices;
        private ObservableCollection<PayDuesModel> payDues;
        private PayDuesModel selectedMemberID;
        private ICommand deleteItem;
        private ICommand addItem;
        private ICommand savePrintInv;
        private bool isAdding;
        private string recBY;
        private int rceiptNumber;
        private IList<AlanoClubPricesModel> pricesModel;
        private ObservableCollection<ItemListModel> numberMonths;
        private ItemListModel selectedMonths;
        private bool isMember = false;

        public ReceiptsViewModel()
        {
            Init();
            //  InitPayDues();
        }
        public bool IsMember
        {
            get => isMember;
            set
            {
                //  if(isMember != value)
                // { 
                isMember = value;
                OnPropertyChanged(nameof(IsMember));
                // }
            }
        }

        public bool IsAdding { get; set; }

        private IList<AlanoClubPricesModel> Prices
        {
            get
            {
                if (pricesModel == null)
                    pricesModel = new List<AlanoClubPricesModel>();
                return pricesModel;
            }
            set
            {
                pricesModel = value;
            }
        }

        public ObservableCollection<ItemListModel> NumberMonths
        {
            get
            {
                if (numberMonths == null)
                    numberMonths = new ObservableCollection<ItemListModel>();
                return numberMonths;
            }
            set
            {
                numberMonths = value;
                OnPropertyChanged(nameof(NumberMonths));
            }
        }
        public ItemListModel SelectedMonths
        {
            get
            {
                if (selectedMonths == null)
                    selectedMonths = new ItemListModel();
                return selectedMonths;

            }
            set
            {
                selectedMonths = value;
                AddDuesToPay();
                OnPropertyChanged(nameof(SelectedMonths));
            }
        }
        public ObservableCollection<ItemListModel> ItemListDescPrices
        {
            get
            {
                if (itemListDescPrices == null)
                    itemListDescPrices = new ObservableCollection<ItemListModel>();
                return itemListDescPrices;
            }
            set
            {
                if (itemListDescPrices != value)
                {
                    itemListDescPrices = value;
                    OnPropertyChanged(nameof(ItemList));
                }
            }
        }
        public ItemListModel SelectedDescPrices
        {
            get

            {
                if (selectedDescPrices == null)
                    selectedDescPrices = new ItemListModel();
                return selectedDescPrices;
            }
            set
            {
                selectedDescPrices = value;
                AddPriceDesc();
                OnPropertyChanged(nameof(SelectedDescPrices));
            }
        }
        public int RceiptNumber
        {
            get => rceiptNumber;
            set
            {
                if (rceiptNumber != value)
                {
                    rceiptNumber = value;
                    OnPropertyChanged(nameof(RceiptNumber));
                }
            }
        }

        public string RecBY
        {
            get => recBY;
            set
            {
                recBY = value;
                OnPropertyChanged(nameof(RecBY));
            }
        }
        public float AmountTotal { get; set; }
        public ICommand AddItem
        {
            get
            {
                if (addItem == null)
                    addItem = new RelayCommd(AddNewItem);
                //   addCategory = new AlanoClubInventory.Utilites.RelayCommand<DataGrid>(AddNewCategoryParm, CanExecuteAction);

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return addItem;
            }

        }

        public ICommand SavePrintRec
        {
            get
            {
                if (savePrintInv == null)
                    savePrintInv = new RelayCommand<ObservableCollection<PayDuesModel>>(PrintSaveRec);
                //   addCategory = new AlanoClubInventory.Utilites.RelayCommand<DataGrid>(AddNewCategoryParm, CanExecuteAction);

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return savePrintInv;
            }

        }
        public ICommand DeleteItem
        {
            get
            {
                if (deleteItem == null)
                    deleteItem = new RelayCommd(ItemDelete);
                //        //delCategory.Execute(true);
                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return deleteItem;
            }
            set
            {
                deleteItem = value;
                OnPropertyChanged(nameof(DeleteItem));
            }
        }
        private int IDInvoice { get; set; } = 0;
        public PayDuesModel SelectedMemberID
        {
            get
            {
                if (selectedMemberID == null)
                {
                    selectedMemberID = new PayDuesModel();
                }
                return selectedMemberID;
            }
            set
            {
                selectedMemberID = value;

                OnPropertyChanged(nameof(SelectedMemberID));
            }
        }
        private async void GetRecNumber()
        {
            RceiptNumber = Scmd.AlClubSqlCommands.SqlCmdInstance.GetMaxID(SqlConnectionStr, Scmd.SqlConstProp.SPGetNextReceiptNumber);
            if (RceiptNumber == 0)
            {
                RceiptNumber = 1000;
            }
            else
                RceiptNumber++;
        }
        public ObservableCollection<PayDuesModel> PayDues
        {
            get
            {
                if (payDues == null)
                {
                    payDues = new ObservableCollection<PayDuesModel>();
                }
                return payDues;
            }
            set
            {
                payDues = value;
                OnPropertyChanged(nameof(PayDues));
            }

        }
        public bool Signed { get; set; } = false;
        public DateTime RecepitDate
        {
            get => recepitDate;
            set
            {
                recepitDate = value;
                OnPropertyChanged(nameof(RecepitDate));
            }
        }
        public ItemListModel SelectedMember
        {
            get
            {
                if (selectedMember == null)
                    selectedMember = new ItemListModel();
                return selectedMember;
            }
            set
            {
                selectedMember = value;
                CheckIsMemnber();
                OnPropertyChanged(nameof(SelectedMember));

                //EditUserInfor();

            }
        }
        private IList<AlanoCLubMembersModel> MembersModel { get; set; }
        public string ACIconPath { get; set; }
        public AlanoClubInformaitonModel AcInfo
        {
            get => aclInfor;
            set { aclInfor = value; OnPropertyChanged(nameof(AcInfo)); }
        }
        public ObservableCollection<ItemListModel> ItemList
        {
            get => itemList;
            set
            {
                itemList = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }
        private async void AddDuesMonths()
        {
            if (NumberMonths.Count > 0)
                NumberMonths.Clear();
            NumberMonths.Add(new ItemListModel { Label = "Number Months Dues", Value = "0" });
            for (int i = 1; i < 13; i++)
                NumberMonths.Add(new ItemListModel { Label = i.ToString(), Value = i.ToString() });
            var ite = NumberMonths.FirstOrDefault(i => i.Value == "0");
            SelectedMonths = ite;
        }
        private async void GetMembersList()
        {
            MembersModel = Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsListNonAsc<AlanoCLubMembersModel>(SqlConnectionStr, Scmd.SqlConstProp.SPALanoCLubGetMembers);
            if ((MembersModel != null) && (MembersModel.Count > 0))
            {
                ItemList = new ObservableCollection<ItemListModel>();


                ItemList.Add(new ItemListModel { Label = "Select Member or Create a Receipt", Value = "0" });
                ItemList.Add(new ItemListModel { Label = "Create a Receipt", Value = "-1" });
                foreach (var item in MembersModel)
                {
                    ItemList.Add(new ItemListModel { Label = $"ID-{item.MemberID.ToString()} FirstName {item.MemberFirstName} Last Name {item.MemberLastName}", Value = item.MemberID.ToString() });
                }
                var i = ItemList.FirstOrDefault(i => i.Value == "0");
                SelectedMember = i;
            }

        }
        private async void GetItemsDescPrices()
        {
            Prices = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetAlanoCLubProducts(SqlConnectionStr, SqlServices.SqlConstProp.SPGetAlanoCLubProducts);
            if ((Prices != null) && (Prices.Count > 0))
            {

                if (ItemListDescPrices.Count > 0)
                {
                    ItemListDescPrices.Clear();
                }

                ItemListDescPrices.Add(new ItemListModel { Label = "Select Description", Value = "0" });

                foreach (var item in Prices)
                {
                    if (item.BarItem)
                        continue;
                    ItemListDescPrices.Add(new ItemListModel { Label = $"Description {item.ProductName}", Value = $"{item.ID.ToString()}" });
                }
                var i = ItemListDescPrices.FirstOrDefault(i => i.Value == "0");
                SelectedDescPrices = i;
            }

        }

        private async void AddPriceDesc()
        {
            if ((SelectedDescPrices.Value != null) && (SelectedDescPrices.Value != "0"))
            {
                var selItem = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedDescPrices.Value);
                if (selItem != int.MinValue)
                {
                    var desc = Prices.FirstOrDefault(p => p.ID == selItem);
                    if (desc != null)
                    {

                        PayDues.Add(new PayDuesModel { ID = IDInvoice++, Quanity = 1, Price = desc.ClubPrice, Description = desc.ProductName });
                    }
                }
            }
        }

        private async void GetAlnoClubInfo()
        {
            ACIconPath = "pack://application:,,,/Resources/Images/butteac.ico";
            ReadJsonFile readJson = new ReadJsonFile();

            var ac = readJson.GetJsonData<AlanoClubInformaitonModel>("AlanoClubInformaitonModel").Result;
            AcInfo = new AlanoClubInformaitonModel
            {
                ClubName = $"{ac.ClubName}",
                ClubAddress = $"{ac.ClubAddress}",
                ClubPOBox = $"{ac.ClubPOBox}",
                ClubCity = $"{ac.ClubCity} {ac.ClubSt} {ac.ClubZipCode}",
                ClubPhone = $"Phone: {ac.ClubPhone}",
                ClubEmail = $"Email: {ac.ClubEmail}",
                FacebookLink = $"FaceBook Page Butte Alano Club"

            };
        }
        private string SqlConnectionStr { get; set; }
        private async void GetSqlConn()
        {
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        private async void CheckIsMemnber()
        {
            IsMember = false;
            if (!(string.IsNullOrWhiteSpace(SelectedMember.Value)))

            {
                var mID = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedMember.Value);
                if ((mID != int.MaxValue) && (mID > 0))
                {
                    IsMember = true;


                }
            }
        }
        private async void PrintSaveRec(ObservableCollection<PayDuesModel> items)
        {

            if ((items != null) && (items.Count > 0))
            {

                if (!(Signed))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Have to Sign the recipt", "Not Signed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                PrintRec(items);
            }
        }
        private async void PrintRec(ObservableCollection<PayDuesModel> items)
        {

            var mID = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedMember.Value);
            if (mID == int.MaxValue)
            {

                Utilites.ALanoClubUtilites.ShowMessageBox("Have to Select Create a receipt or Member ID", "Select Receipt", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var duesM = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedMonths.Value);
            if (duesM == int.MaxValue)
                duesM = 0;
            AddReceiptNumber(mID);
            if ((duesM > 0) && (mID > 0))
            {
                AddPayDues(duesM, RceiptNumber, recepitDate);
            }


            AddSig(RceiptNumber);


            foreach (var rec in items)
            {
                var addNewRecNum = new List<StoredParValuesModel>();
                addNewRecNum.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaReceiptNumber, ParmaValue = RceiptNumber.ToString() });
                addNewRecNum.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaReceiptDesc, ParmaValue = rec.Description.ToString() });
                addNewRecNum.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaQuanitySold, ParmaValue = rec.Quanity.ToString() });
                addNewRecNum.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaPrice, ParmaValue = rec.Price.ToString("0.00") });
               
                await Scmd.AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(SqlConnectionStr, Scmd.SqlConstProp.SPAddAlanoClubReceipt,addNewRecNum);
            }
            PrintEmailRecipt(mID);
        }
        private async void AddReceiptNumber(int mID)
        {
            var addRecNum = new List<StoredParValuesModel>();
            addRecNum.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberID, ParmaValue = mID.ToString() });
            addRecNum.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaRecivedBY, ParmaValue = LoginUserModel.LoginInstance.UserIntils });
            addRecNum.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaDate, ParmaValue = RecepitDate.ToString() });

            await Scmd.AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(SqlConnectionStr, Scmd.SqlConstProp.SPAlanoCLubAddRecNumber, addRecNum);
        }
        private async void AddSig(int recN)
        {
            byte[] img = Utilites.ALanoClubUtilites.ResizeImage(System.IO.Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, "signature.png"), 100, 100);
            await Scmd.AlClubSqlCommands.SqlCmdInstance.AddSig(SqlConnectionStr,recN,img,Scmd.SqlConstProp.SPAlanoClubAddRecSig);
        }
        private async void AddPayDues(int duesM,int rec,DateTime recepitDate)
        {
            var addPayDues = new List<StoredParValuesModel>();
            addPayDues.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaReceiptNumber, ParmaValue = rec.ToString() });
            addPayDues.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaNumberMonthsDues, ParmaValue = duesM.ToString() });
            addPayDues.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaDate, ParmaValue = recepitDate.ToString() });


            await Scmd.AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(SqlConnectionStr, Scmd.SqlConstProp.SPAddPayDues, addPayDues);
        }
        private async void ItemDelete(object itemToDel)
        {
            var item = itemToDel as DataGrid;
            item.CommitEdit();
            if (itemToDel != null)
            {

                var currItem = item.CurrentItem as PayDuesModel;

                int index = PayDues.IndexOf(currItem);
                if (index == -1)
                {
                    return;
                }
                AmountTotal = AmountTotal - currItem.Amount;
                if (AmountTotal <= 0)
                {
                    AmountTotal = 0.00f;
                    IsAdding = false;
                }

                item.BeginEdit();

                //int index = PayDues.IndexOf(currItem)
                PayDues.RemoveAt(index);
                item.CommitEdit();


            }
            //  var catExists = CurrentCat.FirstOrDefault(p => p.ID == SelectedItem.ID);

        }
        private async void AddNewItem(object itemToAdd)
        {

            var item = itemToAdd as DataGrid;

            item.CommitEdit();
            if (item != null)
            {

                var currItem = item.CurrentItem as PayDuesModel;
                if (currItem != null)
                {
                    var price = await Utilites.ALanoClubUtilites.ConvertToFloat(currItem.Price.ToString());
                    if (price <= 0.0f)
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Price {currItem.Price} is invalid", "Price", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var quanity = await Utilites.ALanoClubUtilites.ConvertToFloat(currItem.Quanity.ToString());
                    if (quanity <= 0.0f)
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Quanity {currItem.Quanity} is invalid", "Quanity", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(currItem.Description))
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Need a Description", "Description", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    currItem.Amount = price * quanity;
                    AmountTotal += currItem.Amount;
                    if (!(IsAdding))
                        IsAdding = true;
                    item.BeginEdit();
                    int index = PayDues.IndexOf(currItem);
                    currItem.ID = IDInvoice++;


                    var pdNew = new PayDuesModel { ID = currItem.ID, Amount = currItem.Amount, Quanity = currItem.Quanity, Description = currItem.Description, Price = currItem.Price };
                    PayDues.RemoveAt(index);
                    PayDues.Insert(index, pdNew);



                    item.CommitEdit();



                }
            }
        }
        private async void InitPayDues()
        {
            PayDues.Add(new PayDuesModel { ID = IDInvoice, Description = string.Empty, Amount = 0.0f, Price = 0.0f, Quanity = 1 });


        }
        private async void AddDuesToPay()
        {
            var m = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedMonths.Value);
            if ((m != int.MaxValue) && (m > 0))
            {


                if (PayDues.Count == 0) return;
                else
                {
                    var index = PayDues.Count - 1;
                    var pd = PayDues[index];
                    pd.Quanity = m;
                    PayDues.RemoveAt(index);
                    PayDues.Insert(index, pd);

                }
            }
        }
        private async void Init()
        {
            GetSqlConn();
            GetAlnoClubInfo();
            
            GetMembersList();
            GetRecNumber();
            GetItemsDescPrices();
            AddDuesMonths();
            AmountTotal = 0.0f;
            // DuesAmount = "$10.00";
            RecBY = $"Rec'd / Sold by {LoginUserModel.LoginInstance.UserIntils}";
            IsAdding = false;
            RecepitDate = DateTime.Now;
            IsMember = false;
            if ((PayDues != null) && (PayDues.Count > 0))
                PayDues.Clear();
        }
        public async void PrintEmailRecipt(int mID)
        {
            var navWindow = Application.Current.MainWindow as NavigationWindow;
            if (navWindow != null && navWindow.CanGoBack)
            {
                PrintEmailRecieptPage printEmail = new PrintEmailRecieptPage(RecepitDate, PayDues.ToList(), RceiptNumber, mID);
                navWindow.Navigate(printEmail);
              
            }
          
        }
       
        protected void OnPropertyChanged(string propertyName = null) =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
