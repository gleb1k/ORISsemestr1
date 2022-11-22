using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semestr1.Models;

namespace Semestr1.ORM
{
    public interface IUserRepository
    {
        bool Register(User user);
        bool Login(User user);
        User GetById(int id);
        void Update(User user);
        void Delete(User entity);
    }
}
