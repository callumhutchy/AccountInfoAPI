using System;
using System.Collections.Generic;

namespace AccountInfoApi.Models
{
    public partial class FriendRequests
    {
        public int RecipientId { get; set; }
        public int SenderId { get; set; }
        public DateTime Sent { get; set; }
    }
}
