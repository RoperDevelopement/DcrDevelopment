using AlanoClubInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Models
{
  public  class AlanoCLubProfitLossModel : IAlanoClubProfitLoss, IAlanoClubPrices
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int ItemsPerCase { get; set; } = 0;
        public float Price { get; set; } = 0.0f;
        public int Quantity { get; set; } = 0;
        public float Volume { get; set; } = 0.0f;
        public float TotalPrice { get; set; } = 0.0f;
        public float     CostPerIteam {  get; set; } = 0.0f;
        public float ClubPrice { get; set; } = 0.0f;
        public float ClubNonMemberPrice { get; set; } = 0.0f;
       public float ProfitMemnber { get; set; } = 0.0f;
       public float ProfitNonMemnber { get; set; } = 0.0f;
       public float TotalProfitMember { get; set; } = 0.0f;
       public float TotalProfitNonMember { get; set; } = 0.0f;
    }
}
