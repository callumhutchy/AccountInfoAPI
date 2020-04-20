using System;
using System.Collections.Generic;

namespace AccountInfoApi.Models
{
    public partial class FriendsList
    {
        public int AccountId { get; set; }
        public int FriendsId { get; set; }
        public DateTime Since { get; set; }
    }
}
