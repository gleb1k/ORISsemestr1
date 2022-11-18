using Semestr1.Server;
using Semestr1.Models;
using Semestr1.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.ORM
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly string connectionString = ServerSettings._connectionString;
        public void Create(T entity)
        {
            var myORM = new MyORM(connectionString);
            myORM.Insert(entity);
        }

        public void Delete(T entity)
        {
            var myORM = new MyORM(connectionString);
            myORM.Delete(entity);
        }

        public T GetById(int id)
        {
            var myORM = new MyORM(connectionString);
            var table = myORM.Select<T>();
            return table.Where(entity => entity.Id == id).FirstOrDefault();
        }

        public void Update(T entity)
        {
            var myORM = new MyORM(connectionString);
            myORM.Insert<T>(entity);
        }
    }
}
