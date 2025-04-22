using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class UserResponse : UserModel
    {
        public string Token { get; set; }
    }
}
