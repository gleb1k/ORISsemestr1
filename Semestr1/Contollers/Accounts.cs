using Semestr1.Models;
using Semestr1.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semestr1.Attributes;

namespace Semestr1.Contollers
{
    [HttpController("accounts")]
    public class Accounts
    {
        private const string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True";

        [HttpGET("getaccountbyid")]
        public Account GetAccountById(int id)
        {
            var myORM = new MyORM(_connectionString);
            return myORM.AddParameter("@id", id).ExecuteQuery<Account>("select * from [dbo].[Table] where Id=@id").FirstOrDefault();
        }

        [HttpGET("getaccounts")]
        public List<Account> GetAccounts()
        {
            return AccountDAO.GetAll();
        }

        [HttpPOST("saveaccount")]
        public static void SaveAccount(string login, string password)
        {
            AccountDAO.Create(login, password);
        }

        [HttpPOST("login")]
        public static bool LoginPOST(string login, string password)
        {
            var myORM = new MyORM(_connectionString);
            int count = 0;
            count += myORM.AddParameter("@login", login).AddParameter("@password", password)
                .ExecuteNonQuery("select * from [dbo].[Table] where Login='@login' and Password='@password'");
            if (count > 0)
                return true;
            else
                return false;
        }

    }
}
