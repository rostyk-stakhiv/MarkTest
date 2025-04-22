using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.Validations
{
    public static class Validation
    {
        public static bool ValidateQuestion(QuestionModel item)
        {
            if(item==null)
            {
                throw new MarkTestException("Value cannot be null");
            }
            
            return true;
        }
        public static bool ValidateUser(UserModel item)
        {
            if (item.Name.Length < 3 || item.Name.Length > 50)
            {
                throw new MarkTestException("Name length must be between 3 and 50 symbols");
            }


            if (item.Surname.Length < 3 || item.Surname.Length > 50)
            {
                throw new MarkTestException("Name length must be between 3 and 50 symbols");
            }

            
            if(!Regex.IsMatch(item.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                throw new MarkTestException("Email format is invalid");
            }

            if(item.Login.Length<5||item.Login.Length>50)
            {
                throw new MarkTestException("Login length must be between 5 and 50 symbols");
            }

            if(item.Password.Length<8||item.Password.Length>50)
            {
                throw new MarkTestException("Password length must be between 8 and 50 symbols");
            }

            if(!Regex.IsMatch(item.Phone, @"^[+]?[3]?[8]?[0][0-9]{9}$"))
            {
                throw new MarkTestException("Phone format is invalid");
            }
            return true;
        }
    }
}
