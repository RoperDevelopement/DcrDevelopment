using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using AlanoClubInventory.Interfaces;

namespace AlanoClubInventory.Utilites
{
    public class ShutDownService:IShutDownService
    {
        private readonly List<Action> actions = new();
        public ShutDownService() 
        {
            Application.Current.Exit += OnExit;
        }
        public void RegisterCleanUp(Action action)
        {
            if(action != null)
                actions.Add(action);
        }
        private void OnExit(object sender, EventArgs e)
        {
            foreach(var action in actions)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
     }
}
 
