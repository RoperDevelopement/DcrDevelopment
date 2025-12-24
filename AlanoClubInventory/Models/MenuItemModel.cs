using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlanoClubInventory.Models
{
    public class MenuItemModel
    {
        public string Header { get; }
        public ICommand Command { get; }
        public string CommandParameter { get; set; }

        public MenuItemModel(string header, ICommand command, string commandParameter)
        {
            Header = header;
            Command = command;
            CommandParameter = commandParameter;
        }
    }
}
