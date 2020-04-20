using System;
using System.Collections.Generic;

namespace AccountInfoApi.Models
{
    public partial class Characters
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Zone { get; set; }
        public string Position { get; set; }
        public string Skin { get; set; }
        public int? InventoryId { get; set; }
        public int? WalletId { get; set; }
    }
}
