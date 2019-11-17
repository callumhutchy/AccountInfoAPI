using System;
using System.Collections.Generic;

namespace AccountInfoApi.Models
{
    public partial class UserInformation
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PreviousPassword { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime? DateOfLastLogin { get; set; }
        public int Admin { get; set; }
        public string ValidationCode { get; set; }
        public int Validated { get; set; }
        public string AuthenticationToken { get; set; }
        public int LoggedIn { get; set; }
        public byte[] Salt {get;set;}
    }
}
