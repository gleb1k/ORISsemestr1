using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Account(int id, string login, string password, string mobile)
        {
            Id = id;
            Login = login;
            Password = password;
            Mobile = mobile;
        }
        public Account() { }

    }
}
