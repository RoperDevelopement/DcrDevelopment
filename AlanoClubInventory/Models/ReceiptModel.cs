using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
  public    class ReceiptModel: PayDuesModel, ISignature
    {
        public byte[] Signature { get; set; }
    }
}
