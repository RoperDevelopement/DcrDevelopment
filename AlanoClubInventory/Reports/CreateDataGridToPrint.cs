using AlanoClubInventory.Models;
using ScottPlot.TickGenerators.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AlanoClubInventory.Reports
{
    class CreateDataGridToPrint
    {
        public async Task PrintDataGrid(IList<AlanoCLubProfitLossModel> ts)
        {
            DataGrid dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,   // automatically create columns from properties
                Margin = new Thickness(10),
                ItemsSource = ts 
             //   EnableRowVirtualization = true// bind the ObservableCollection
            };
            
            DataGridTextColumn nameColumn = new DataGridTextColumn
            {
                Header = "Product Per Case",
                Binding = new Binding("ItemsPerCase")
            };
            dataGrid.Columns.Add(nameColumn);

            DataGridTextColumn namePurchasePrice = new DataGridTextColumn
            {
                Header = "Purchase Price",
                Binding = new Binding("Price")
            };
            dataGrid.Columns.Add(namePurchasePrice);

            DataGridTextColumn nameInStock = new DataGridTextColumn
            {
                Header = "In Stock",
                Binding = new Binding("Quantity")
            };
            dataGrid.Columns.Add(nameInStock);

            DataGridTextColumn nameCases = new DataGridTextColumn
            {
                Header = "Cases",
                Binding = new Binding("Volume")
            };
            dataGrid.Columns.Add(nameCases);

            DataGridTextColumn nameCostPerItem = new DataGridTextColumn
            {
                Header = "Cost Per Item",
                Binding = new Binding("CostPerItem")
            };
            dataGrid.Columns.Add(nameCostPerItem);

            DataGridTextColumn nameTotalCost = new DataGridTextColumn
            {
                Header = "Total Cost",
                Binding = new Binding("TotalPrice")
            };
            dataGrid.Columns.Add(nameTotalCost);

            DataGridTextColumn nameClubPrice = new DataGridTextColumn
            {
                Header = "Club Price",
                Binding = new Binding("ClubPrice")

                
            };
            dataGrid.Columns.Add(nameClubPrice);

            DataGridTextColumn nameProfitMember = new DataGridTextColumn
            {
                Header = "Profit Member",
                Binding = new Binding("ProfitMemnber")
            };
            dataGrid.Columns.Add(nameProfitMember);

            DataGridTextColumn nameTPMember = new DataGridTextColumn
            {
                Header = "TP Member",
                Binding = new Binding("TotalProfitMember")
            };
            dataGrid.Columns.Add(nameTPMember);

            DataGridTextColumn nameNonMemberPrice = new DataGridTextColumn
            {
                Header = "Non Member Price",
                Binding = new Binding("ClubNonMemberPrice")
            };
            dataGrid.Columns.Add(nameNonMemberPrice);

            DataGridTextColumn nameProfitNonMember = new DataGridTextColumn
            {
                Header = "Profit Non Member",
                Binding = new Binding("ProfitNonMemnber")
            };
            dataGrid.Columns.Add(nameProfitNonMember);
             DataGridTextColumn nameTPNonMember = new DataGridTextColumn
            {
               Header = "TP Non Member",
                Binding = new Binding("TotalProfitNonMember")
            };
            dataGrid.Columns.Add(nameTPNonMember);

            
           
            
            Print printHelper = new Print();
            printHelper.PrintDataGridAllRows(dataGrid);
        }
    }
}
