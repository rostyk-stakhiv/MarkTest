using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class UserModel : BaseModel
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
