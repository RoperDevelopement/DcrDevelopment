using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Scmd = AlanoClubInventory.SqlServices;

namespace AlanoClubInventory.ViewModels
{
    public class AddEditProductPricesViewModel : INotifyPropertyChanged
    {
       private readonly ReadJsonFile readJson = new ReadJsonFile();
        private AlanoClubPricesModel barItem;
        private AlanoClubPricesModel itemPrice;
        private string categoryName;
        private string productName; 
        private string clubprice;
        private string clubNonMemberPrice;
        private bool isEnabled;
        private ICommand addPrices;
        private ICommand addNewClubPrice;
        private ObservableCollection<AlanoClubPricesModel> products;
        public event PropertyChangedEventHandler PropertyChanged;
        public AddEditProductPricesViewModel()
            {
            GetProducts();
            }
        private int ID { get; set; }
        private string SqlConnectionStr { get; set; }
        private AlanoClubPricesModel ItemPrice 
        {
            get => itemPrice;
            set
            {
                itemPrice = value;
                OnPropertyChanged(nameof(ItemPrice));
            }
        }
        public bool IsTextEnable
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsTextEnable));
            }
        }
        public ObservableCollection<AlanoClubPricesModel> Products
        {
            get
            {
                if (products == null)
                    products = new ObservableCollection<AlanoClubPricesModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return products;
            }
            set
            {
                products = value;
                OnPropertyChanged(nameof(products));
                //  addCategory.Execute(true);
            }
        }
        
        public string ProductName
            {
            get => productName;
            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }
        public string ClubPrice 
            {
            get => clubprice;
            set
            {
                clubprice = value;
                OnPropertyChanged(nameof(ClubPrice));
            }
        }

        public string ClubNonMemberPrice
            {
            get => clubNonMemberPrice;
            set
            {
                clubNonMemberPrice = value;
                OnPropertyChanged(nameof(ClubNonMemberPrice));
            }
        }
        public string CategoryName
        {             get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        public AlanoClubPricesModel BarItem
        {
            get => barItem;
            set
            {
                barItem = value;
                OnPropertyChanged(nameof(BarItem));
            }
        }
        public ICommand UpDatePrices
        {
            get
            {
                if (addPrices == null)
                    addPrices = new RelayCommd(UpDateACPrices);
                //      //  editCategory.Execute(true);
                //        //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return addPrices;
            }
            set
            {
                addPrices = value;
                OnPropertyChanged(nameof(UpDatePrices));
            }
        }
        public ICommand AddNewClubPrice
        {
            get
            {
                if (addNewClubPrice == null)
                    addNewClubPrice = new RelayCommd(ACNewPrice);
                //      //  editCategory.Execute(true);
                //        //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return addNewClubPrice;
            }
            set
            {
                addNewClubPrice = value;
                OnPropertyChanged(nameof(AddNewClubPrice));
            }
        }
        
        private async void SetTxt()
        {
             CategoryName = "Category Name";
        ProductName = "Product Name";
            ClubPrice = "0.00";
         ClubNonMemberPrice = "0.00";
            IsTextEnable = false;
            ID = 0;
            await Task.CompletedTask;
        }
        private async Task AddTillPrices()
        {
            AlanoClubTillPricesModel alanoClubTillPrices = new AlanoClubTillPricesModel()
            {
                ID = ID,
                ClubPrice = float.Parse(ClubPrice.ToString()),
                ClubNonMemberPrice = float.Parse(ClubNonMemberPrice.ToString()),
              
            };
            if(alanoClubTillPrices.ClubNonMemberPrice == 0f)
            {
                alanoClubTillPrices.ClubNonMemberPrice = alanoClubTillPrices.ClubPrice;
            }
            await Scmd.AlClubSqlCommands.SqlCmdInstance.AlanoCLubTillPrices(SqlConnectionStr,alanoClubTillPrices,SqlServices.SqlConstProp.SPAlanoClubTillPrices);
            GetProducts();
            await Task.CompletedTask;
        }
        private async void GetProducts()
        {
            Products.Clear();
            var conStr = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            SqlConnectionStr = conStr.AlanoClubSqlServer;
            var items = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetAlanoCLubProducts(SqlConnectionStr, SqlServices.SqlConstProp.SPGetAlanoCLubProducts);
            if ((items != null) && (items.Count > 0))
            {
                
                foreach (var item in items)
                {
                    Products.Add(item);
                }
                SetTxt();

            }
            else
            {
                return;
            }

          


        }
        public async void UpDateACPrices(object prices)
        {
            var acPrice = prices as DataGrid;
            acPrice.CommitEdit();
            if (acPrice != null)
            {
                var cerPriceCell = acPrice.CurrentItem as AlanoClubPricesModel;
                CategoryName = cerPriceCell.CategoryName;
                ProductName =  cerPriceCell.ProductName;
                ClubPrice =  cerPriceCell.ClubPrice.ToString();
                
                ClubNonMemberPrice = cerPriceCell.ClubNonMemberPrice.ToString();
                ID = cerPriceCell.ID;
                IsTextEnable = true;
                OnPropertyChanged(nameof(IsTextEnable));
              //  if ((string.IsNullOrWhiteSpace(ClubPrice)) || ClubPrice.StartsWith("0"))
              //  {
                //    ClubPrice = "0.00";
               // }
                //if ((string.IsNullOrWhiteSpace(ClubNonMemberPrice)) || ClubNonMemberPrice.StartsWith("0"))
                //{
                //    ClubNonMemberPrice = "0.00";
                //}
             
            }
                await Task.CompletedTask;
        }
        private async void ACNewPrice(object par)
        {
            var cp = await ALanoClubUtilites.ConvertToFloat(ClubPrice.ToString());
            if(cp < 1f)
            {
                ALanoClubUtilites.ShowMessageBox("Please enter a valid club price greater than 0", "Invalid Price", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            if ((!string.IsNullOrWhiteSpace(ClubNonMemberPrice)) && !(ClubNonMemberPrice.StartsWith("0")))
            {
                var cnmp = await ALanoClubUtilites.ConvertToFloat(ClubNonMemberPrice.ToString());

                if (cnmp < 1f)
                {
                    ALanoClubUtilites.ShowMessageBox("Please enter a valid club non member price greater than 0", "Invalid Price", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
            }
            else
                ClubNonMemberPrice = "0.00";

            await AddTillPrices();
            await Task.CompletedTask;
        }
        protected void OnPropertyChanged(string propertyName) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
