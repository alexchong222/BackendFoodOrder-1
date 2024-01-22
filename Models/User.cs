using System;
using System.Collections.Generic;

#nullable disable

namespace BackendFoodOrder.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserLevel { get; set; }       
        public string DTAdded { get; set; }
    }
}
