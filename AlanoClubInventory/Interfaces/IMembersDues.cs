using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Interfaces
{
    public interface IDuesReceipt : IUserID
    {
        int Quanity { get; set; }
        string Description { get; set; }
        DateTime DatePaid { get; set; }
        float Price { get; set; }
        float Amount { get; set; }
        string RecivedBy { get; set; }
        int NumberMonthsPayDues { get; set; }
    }
    public interface ISignature
    {
        byte[] Signature { get; set; }
    }
    public interface IReceiptNumber
    {
        int ReceiptNumber { get; set; }
    }
}
