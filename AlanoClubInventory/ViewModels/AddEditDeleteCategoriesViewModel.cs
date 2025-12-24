using AlanoClubInventory.Models;
using AlanoClubInventory.SqlServices;
using AlanoClubInventory.Utilites;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using static AlanoClubInventory.Utilites.RelayCommandPredicate;

using Scmd = AlanoClubInventory.SqlServices;
namespace AlanoClubInventory.ViewModels
{
    public class AddEditDeleteCategoriesViewModel : INotifyPropertyChanged
    {
        private CategoryModel barItem;
        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand addCategory;
        //  private ICommand AddCategory;
        private ICommand delCategory;
        private ICommand editCategory;
        private string categoryName;
        private CategoryModel _selectedItem;
        private ObservableCollection<CategoryModel> categories;
        ReadJsonFile readJson = new ReadJsonFile();
        private string SqlConnectionStr;
        public AddEditDeleteCategoriesViewModel()
        {
            GetCategories();

            // AddCategory=  new AlanoClubInventory.Utilites.RelayCommandPredicate(AddNewCategoryParm, CanExecuteAction);
            // AddCategory.Execute(true);
        }

        private IList<CategoryModel> CurrentCat { get; set; }
        
        private async void GetSqlConnectionStr()
        {
            //var conStr = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);
        }
        private async void GetCategories()
        {
            Categories.Clear();
            GetSqlConnectionStr();
            CurrentCat = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetCategories(SqlConnectionStr, SqlServices.SqlConstProp.SPGetCategories);
            if ((CurrentCat != null) && (CurrentCat.Count > 0))
            {
               
                foreach (var item in CurrentCat)
                {
                    Categories.Add(item);
                }
            }





        }
       
        public CategoryModel CModel
        { get; set; }
        public CategoryModel BarItem
        {
            get => barItem;
            set
            {
                barItem = value;
                OnPropertyChanged(nameof(barItem));
            }
        }
        public CategoryModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public ObservableCollection<CategoryModel> Categories
        {
            get
            {
                if (categories == null)
                    categories = new ObservableCollection<CategoryModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return categories;
            }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
                //  addCategory.Execute(true);
            }
        }
        public ICommand AddCategory
        {
            get
            {
                if (addCategory == null)
                    addCategory = new RelayCommd(AddNewCategory);
                //   addCategory = new AlanoClubInventory.Utilites.RelayCommand<DataGrid>(AddNewCategoryParm, CanExecuteAction);

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return addCategory;
            }
            set
            {
                addCategory = value;
                OnPropertyChanged(nameof(AddCategory));
                //  addCategory.Execute(true);
            }
        }
        public ICommand DelCategory
        {
            get
            {
                if (delCategory == null)
                    delCategory = new RelayCommd(DeleteCategory);
                //        //delCategory.Execute(true);
                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return delCategory;
            }
            set
            {
                delCategory = value;
                OnPropertyChanged(nameof(DelCategory));
            }
        }

        public ICommand EditCategory
        {
            get
            {
                if (editCategory == null)
                    editCategory = new RelayCommd(CategoryEdit);
                //      //  editCategory.Execute(true);
                //        //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return editCategory;
            }
            set
            {
                editCategory = value;
                OnPropertyChanged(nameof(EditCategory));
            }
        }

        public string CategoryName
        {
            get => categoryName;
            set
            {
                if (categoryName != value)
                {
                    categoryName = value;
                    OnPropertyChanged(nameof(CategoryName));
                }
            }
        }
        public async void DeleteCategory(object category)
        {
            var cat = category as DataGrid;
            cat.CommitEdit();
            if (cat != null)
            {
                if (SelectedItem == null)
                {
                    ALanoClubUtilites.ShowMessageBox("Need To add Category Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (SelectedItem.ID != 0)
                {
                    if (CheckCaegoryNameExists())
                    {


                        var catExists = CurrentCat.FirstOrDefault(p => p.ID == SelectedItem.ID);
                        if (!(string.IsNullOrEmpty(catExists.CategoryName)))
                        {
                            if (await ALanoClubUtilites.ShowMessageBoxResults($"Delete Category {SelectedItem.CategoryName}", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                            {
                                await Scmd.AlClubSqlCommands.SqlCmdInstance.AddEditDeleteCategories(SqlConnectionStr, catExists, SqlServices.SqlConstProp.SPAddUpdateCatagories,1);
                              
                                ALanoClubUtilites.ShowMessageBox($"Category name {SelectedItem.CategoryName} deleted ", "Category Added", MessageBoxButton.OK, MessageBoxImage.Information);
                                GetCategories();
                            }
                            else
                                return;
                        }
                    }
                    else
                        ALanoClubUtilites.ShowMessageBox($"Category Name to delete {SelectedItem.CategoryName} not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    ALanoClubUtilites.ShowMessageBox("Need To a Category Name to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ALanoClubUtilites.ShowMessageBox("Category Name Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void CategoryEdit(object category)
        {
            var cat = category as DataGrid;
            cat.CommitEdit();
            if (cat != null)
            {
                if (SelectedItem == null)
                {
                    ALanoClubUtilites.ShowMessageBox("Need To add Category Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!(string.IsNullOrEmpty(SelectedItem.CategoryName)) && (SelectedItem.ID != 0))
                {
                    var catExists = CurrentCat.FirstOrDefault(p => p.ID == SelectedItem.ID);
                    if (!(string.IsNullOrEmpty(catExists.CategoryName)))
                    {
                        // if (CheckCaegoryNameExists())
                        // {
                        //   ALanoClubUtilites.ShowMessageBox($"Category Name {SelectedItem.CategoryName} found cannot update ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        //   return;
                        // }
                        var cerCell = cat.CurrentItem as CategoryModel;
                        catExists.BarItem = cerCell.BarItem;
                        await Scmd.AlClubSqlCommands.SqlCmdInstance.AddEditDeleteCategories(SqlConnectionStr, catExists, SqlServices.SqlConstProp.SPAddUpdateCatagories);
                        GetCategories();
                        ALanoClubUtilites.ShowMessageBox("Category name updated", "Name UpDated", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                        ALanoClubUtilites.ShowMessageBox($"Category Name {SelectedItem.CategoryName} not found ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ALanoClubUtilites.ShowMessageBox("Need To add Category Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ALanoClubUtilites.ShowMessageBox("Category Name Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void AddNewCategory(object category)
        {
            var cat = category as DataGrid;
            cat.CommitEdit();
            if (cat != null)
            {
                if (SelectedItem == null)
                {
                    ALanoClubUtilites.ShowMessageBox("Need To add Category Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!(string.IsNullOrEmpty(SelectedItem.CategoryName)) && (SelectedItem.ID == 0))
                {
                    if (CheckCaegoryNameExists())
                    {
                        ALanoClubUtilites.ShowMessageBox($"Category Name {SelectedItem.CategoryName} found cannot add ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var cerCell = cat.CurrentItem as CategoryModel;
                    var catExists = new CategoryModel { ID = 0, CategoryName = SelectedItem.CategoryName,BarItem=cerCell.BarItem };


                    await Scmd.AlClubSqlCommands.SqlCmdInstance.AddEditDeleteCategories(SqlConnectionStr, catExists, SqlServices.SqlConstProp.SPAddUpdateCatagories);
                    GetCategories();
                    ALanoClubUtilites.ShowMessageBox($"Category name {catExists.CategoryName} Added", "Category Added", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ALanoClubUtilites.ShowMessageBox("Need To add Category Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ALanoClubUtilites.ShowMessageBox("Category Name Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // var cat = category.DataContext as CategoryModel;
        //var rowData = cat?.DataContext as CategoryModel;


        //public async void AddNewCategory()
        //{
        //    if (!string.IsNullOrWhiteSpace(CModel.CategoryName))
        //        await Scmd.AlClubSqlCommands.SqlCmdInstance.AddEditDeleteCategories(SqlConnectionStr, CModel, SqlServices.SqlConstProp.SPAddUpdateCatagories);

        //}
        private bool CheckCaegoryNameExists()
        {
            return CurrentCat.Where(p => p.CategoryName.Trim() == SelectedItem.CategoryName.Trim()).Any();
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
    }

}
