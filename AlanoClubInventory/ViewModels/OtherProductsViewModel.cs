using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Scmd = AlanoClubInventory.SqlServices;
namespace AlanoClubInventory.ViewModels
{
    public class OtherProductsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool inventoryItem;
        private string productName;
        private string price;
        private string totalOnHand;
        private string itemsPerCase;
        ReadJsonFile readJson = new ReadJsonFile();
        private ObservableCollection<ItemListModel> itemList;
        private ObservableCollection<InventoryModel> currInv;
        private ItemListModel itemsListCategoryID;
        private ICommand addInventory;
        private ICommand editProductName;
        private ICommand deleteInventory;
        private InventoryModel selectedItem;
        public OtherProductsViewModel()
        {
            GetCategories();
            Price = "0.0";
            TotalOnHand = "0";
            CurrentID = 0;
        }

        private string SqlConnectionStr { get; set; }
        public int CurrentID { get; set; }
        public ObservableCollection<ItemListModel> ItemsList
        {
            get
            {
                if (itemList == null)
                    itemList = new ObservableCollection<ItemListModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return itemList;
            }
            set
            {
                itemList = value;
                OnPropertyChanged(nameof(ItemsList));
                //  addCategory.Execute(true);
            }
        }
        private InventoryModel SelectedItem
        {
            get
            {
                if (selectedItem == null)
                    selectedItem = new InventoryModel();
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public ICommand DeleteInventory
        {
            get
            {
                if (deleteInventory == null)
                    deleteInventory = new RelayCommd(InventoryDel);
                //        //delCategory.Execute(true);
                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return deleteInventory;
            }
            set
            {
                deleteInventory = value;
                OnPropertyChanged(nameof(deleteInventory));
            }
        }
        public bool InventoryItem
        {

            get => inventoryItem;
            set
            {
                inventoryItem = value;
                OnPropertyChanged(nameof(InventoryItem));
            }



        }
        public ObservableCollection<InventoryModel> CurrentInv
        {
            get
            {
                if (currInv == null)
                    currInv = new ObservableCollection<InventoryModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return currInv;
            }
            set
            {
                currInv = value;
                OnPropertyChanged(nameof(CurrentInv));
                //  addCategory.Execute(true);
            }
        }
        public ItemListModel ItemsListCategoryID
        {
            // get => itemsListCategoryID;
            get
            {
                if (itemsListCategoryID == null)
                    itemsListCategoryID = new ItemListModel();
                return itemsListCategoryID;
            }
            set
            {
                itemsListCategoryID = value;
                OnPropertyChanged(nameof(ItemsListCategoryID));
                //  addCategory.Execute(true);
            }
        }
        public string ProductName
        {
            get => productName;



            //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);



            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));
                //  addCategory.Execute(true);
            }
        }
        public string ItemsPerCase
        {
            get => itemsPerCase;



            //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);



            set
            {
                itemsPerCase = value;
                OnPropertyChanged(nameof(ItemsPerCase));
                //  addCategory.Execute(true);
            }
        }
        public ICommand EditProductName
        {
            get
            {
                if (editProductName == null)
                    editProductName = new RelayCommd(ProcuctNameEdit);
                //      //  editCategory.Execute(true);
                //        //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return editProductName;
            }
            set
            {
                editProductName = value;
                OnPropertyChanged(nameof(EditProductName));
            }
        }
        public string Price
        {
            get => price;



            //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);



            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
                //  addCategory.Execute(true);
            }
        }

        public string TotalOnHand
        {
            get => totalOnHand;



            //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);



            set
            {
                totalOnHand = value;
                OnPropertyChanged(nameof(TotalOnHand));
                //  addCategory.Execute(true);
            }
        }
        private async void GetCategories()
        {
            ItemsList.Clear();
            var conStr = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            SqlConnectionStr = conStr.AlanoClubSqlServer;
            var currentCat = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetCategoriesBarItems(SqlConnectionStr, SqlServices.SqlConstProp.SPGetCategoriesNotBarItems);
            if ((currentCat != null) && (currentCat.Count > 0))
            {
                ItemsList.Add(new ItemListModel { Label = "Select a Category", Value = "0" });
                foreach (var item in currentCat)
                {
                    ItemsList.Add(new ItemListModel { Label = item.CategoryName, Value = item.ID.ToString() });
                }
             

            }
            else
            {
                return;
            }

            GetProdInventory();


        }
        private async void GetProdInventory()
        {
            CurrentInv.Clear();
            var inv = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<InventoryModel>(SqlConnectionStr, SqlServices.SqlConstProp.SPGetInventory, 0);

            if ((inv != null) && (inv.Count > 0))
            {

                foreach (var item in inv)
                {
                    CurrentInv.Add(item);
                }
              
            }
            var i = ItemsList.FirstOrDefault(i => i.Label == "Select a Category");
            ItemsListCategoryID = i;
        }
        public ICommand AddInventory
        {
            get
            {
                if (addInventory == null)
                    addInventory = new RelayCommd(AddEditDeleteInv);
                //   addCategory = new AlanoClubInventory.Utilites.RelayCommand<DataGrid>(AddNewCategoryParm, CanExecuteAction);

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return addInventory;
            }
            set
            {
                addInventory = value;
                OnPropertyChanged(nameof(AddInventory));
                //  addCategory.Execute(true);
            }
        }
        public async void AddEditDeleteInv(object par)
        {
            if ((itemsListCategoryID.Label == null) || (ItemsListCategoryID.Value == "0"))
            {
                if (itemsListCategoryID.Label != null)
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Invalid Category Selected {ItemsListCategoryID.Label}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Need to select a Category... ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Invalid Product Name ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var cost = await Utilites.ALanoClubUtilites.ConvertToFloat(Price);
            if (cost < 0)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Invalid Price {Price}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var quanity = await Utilites.ALanoClubUtilites.ConvertToInt(TotalOnHand);
            if (quanity == int.MinValue)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Invalid Quanity {TotalOnHand}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var ipc = await Utilites.ALanoClubUtilites.ConvertToInt(ItemsPerCase);
            if (ipc == int.MinValue)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Invalid Items Per case {ItemsPerCase}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((CurrentID == 0) && (ChekProductExists(ProductName)))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Product Name {ProductName} found need to edit", "Error Product Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var products = new AddEditInventoryModel { ID = CurrentID, CategoryID = int.Parse(itemsListCategoryID.Value), ProductName = ProductName, Price = (float)cost, Quantity = quanity,ItemsPerCase=ipc,InventoryItem=InventoryItem };
            await Scmd.AlClubSqlCommands.SqlCmdInstance.AddEditDeleteInventory(SqlConnectionStr, products, Scmd.SqlConstProp.SPAddEditDeleteAlconProducts, 0,0);
            RestValues();
            GetProdInventory();
            CurrentID = 0;
        }
        private bool ChekProductExists(string productName)
        {
            return CurrentInv.Any(p => p.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase));
        }
        private async void RestValues()
        {
            ProductName = string.Empty;
            Price = "0.00";
            TotalOnHand = "0";
            ItemsPerCase = "0";
        }
        private bool CanExecuteAction()
        {
            return true;
        }
        private bool CanExecuteAction(object parm)
        {
            return parm is CategoryModel;
        }
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public async void ProcuctNameEdit(object pName)
        {
            var pn = pName as DataGrid;
            pn.CommitEdit();
            if (pn != null)
            {
                //if (SelectedItem == null)
                //{
                //    ALanoClubUtilites.ShowMessageBox("Need To add Category Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
                var cerCell = pn.CurrentItem as InventoryModel;
                ProductName = cerCell.ProductName;
                Price = cerCell.Price.ToString();
                TotalOnHand = cerCell.Quantity.ToString();
                CurrentID = cerCell.ID;
                ItemsPerCase = cerCell.ItemsPerCase.ToString();
                // GetCategories();
                var i = ItemsList.FirstOrDefault(i => i.Label == cerCell.CategoryName);
                ItemsListCategoryID = i;

            }
        }
        public async void InventoryDel(object pName)
        {
            var pn = pName as DataGrid;
            pn.CommitEdit();
            if (pn != null)
            {
                var cerCell = pn.CurrentItem as InventoryModel;
                if (await Utilites.ALanoClubUtilites.ShowMessageBoxResults($"Delete Product Name {cerCell.ProductName}", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var products = new AddEditInventoryModel { ID = cerCell.ID, CategoryID = 0, ProductName = "NA", Price = 0.0f, Quantity = 0,ItemsPerCase=0 };
                    await Scmd.AlClubSqlCommands.SqlCmdInstance.AddEditDeleteInventory(SqlConnectionStr, products, Scmd.SqlConstProp.SPAddEditDeleteAlconProducts,1);
                    //await Scmd.AlClubSqlCommands.SqlCmdInstance.AddEditDeleteInventory(SqlConnectionStr, products, Scmd.SqlConstProp.SPAddEditDeleteAlconCLubInventory, 1);
                    RestValues();
                    GetProdInventory();
                    CurrentID = 0;
                }
            }

        }
    }
}
