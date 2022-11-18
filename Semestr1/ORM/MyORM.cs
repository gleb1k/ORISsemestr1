using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Semestr1.ORM
{
    public class MyORM
    {
        public IDbConnection _connection = null;
        public IDbCommand _cmd = null;

        public MyORM(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _cmd = _connection.CreateCommand();
        }
        //Колво строк
        public int ExecuteNonQuery(string query)
        {
            int noOfAffectedRows = 0;
            using (_connection)
            {
                _cmd!.CommandText = query;
                _connection!.Open();
                noOfAffectedRows = _cmd.ExecuteNonQuery();
            }
            return noOfAffectedRows;
        }
        //Добавить параметры
        public MyORM AddParameter<T>(string name, T value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = name;
            param.Value = value;
            _cmd!.Parameters.Add(param);
            return this;
        }
        //Много штук
        public IEnumerable<T> ExecuteQuery<T>(string query)
        {
            IList<T> list = new List<T>();
            Type type = typeof(T);

            using (_connection)
            {
                _cmd!.CommandText = query;
                _connection.Open();
                var reader = _cmd.ExecuteReader();
                while (reader.Read())
                {
                    T obj = (T)Activator.CreateInstance(type);
                    type.GetProperties().ToList().ForEach(p =>
                    {
                        p.SetValue(obj, reader[p.Name]);
                    });
                    list.Add(obj);
                }
            }
            return list;
        }
        //Первый столбец первой попавшей строки
        public T ExectureScalar<T>(string query)
        {
            T result = default;
            using (_connection)
            {
                _cmd.CommandText = query;
                _connection.Open();
                result = (T)_cmd.ExecuteScalar();
            }
            return result;
        }
        /// <summary>
        /// Select * from table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Select<T>()
        {
            IList<T> list = new List<T>();
            Type t = typeof(T);

            using (_connection)
            {
                //НУЖНО ЧТОБ ТАБЛИЦА НАЗЫВАЛАСЬ Accounts, Users, Cars ... (иначе не робит)
                string sqlExpression = $"SELECT * FROM {t.Name}s";

                _cmd.CommandText = sqlExpression;

                _connection.Open();
                var reader = _cmd.ExecuteReader();
                while (reader.Read())
                {
                    T obj = (T)Activator.CreateInstance(t);
                    var temp = t.GetProperties().ToList();
                    t.GetProperties().ToList().ForEach(x =>
                    x.SetValue(obj, reader[x.Name]));

                    list.Add(obj);
                }
            }
            return list;
        }
        /// <summary>
        /// Inserts values into table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Insert<T>(T entity)
        {
            Type t = typeof(T);
            var args = t.GetProperties();

            var values = args.Select(value => $"@{value.GetValue(entity)}").ToArray();

            var argsWithDog = args.Select(value => $"@{value.ToString().Split(" ")[1]}").ToArray();
            var argsWithoutDog = args.Select(value => $"{value.ToString().Split(" ")[1]}").ToArray();

            foreach (var parameter in args)
            {
                var sqlParameter = new SqlParameter($"@{parameter.Name}", parameter.GetValue(entity));
                _cmd.Parameters.Add(sqlParameter);
            }

            string nonQuery = $"SET IDENTITY_INSERT {t.Name}s ON " +
            $"INSERT INTO {t.Name}s ({string.Join(", ", argsWithoutDog)}) VALUES ({string.Join(", ", argsWithDog)}) " +
            $"SET IDENTITY_INSERT {t.Name}s OFF";

            ExecuteNonQuery(nonQuery);
        }
        /// <summary>
        /// Updating field by VALUES. ID doesn't matter!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity)
        {
            Type t = typeof(T);
            var args = t.GetProperties();

            var values = args.Select(value => $"@{value.GetValue(entity)}").ToArray();

            var argsWithDog = args.Select(value => $"@{value.ToString().Split(" ")[1]}").ToArray();
            var argsWithoutDog = args.Select(value => $"{value.ToString().Split(" ")[1]}").ToArray();

            foreach (var parameter in args)
            {
                var sqlParameter = new SqlParameter($"@{parameter.Name}", parameter.GetValue(entity));
                _cmd.Parameters.Add(sqlParameter);
            }
            //Row = @Value. without ID
            string setQuery = "";
            for (int i = 1; i < args.Length; i++)
            {
                setQuery += $"{argsWithoutDog[i]}={argsWithDog[i]}, ";
            }
            setQuery = setQuery.Remove(setQuery.Length - 2, 2);

            string nonQuery = $"UPDATE {t.Name}s SET {setQuery} " +
                $"FROM " +
                $"(SELECT * FROM {t.Name}s WHERE Id={argsWithDog[0]}) AS Selected " +
                $"WHERE {t.Name}s.Id = Selected.Id";

            ExecuteNonQuery(nonQuery);
        }
        /// <summary>
        /// Deleting entity by ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity)
        {
            Type t = typeof(T);
            var args = t.GetProperties();

            var values = args.Select(value => $"@{value.GetValue(entity)}").ToArray();

            var argsWithDog = args.Select(value => $"@{value.ToString().Split(" ")[1]}").ToArray();
            var argsWithoutDog = args.Select(value => $"{value.ToString().Split(" ")[1]}").ToArray();

            foreach (var parameter in args)
            {
                var sqlParameter = new SqlParameter($"@{parameter.Name}", parameter.GetValue(entity));
                _cmd.Parameters.Add(sqlParameter);
            }

            string nonQuery = $"DELETE FROM {t.Name}s " +
                $"WHERE {argsWithoutDog[0]}={argsWithDog[0]}";

            ExecuteNonQuery(nonQuery);
        }
        /// <summary>
        /// Checking existence of entity by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsExist<T>(T entity)
        {
            return false;
        }
    }
}
