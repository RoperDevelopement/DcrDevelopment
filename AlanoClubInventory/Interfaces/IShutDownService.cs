using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Interfaces
{
  public    interface IShutDownService
    {
        void RegisterCleanUp(Action action);
    }
}
