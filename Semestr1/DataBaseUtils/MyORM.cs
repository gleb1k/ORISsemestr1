using Npgsql;

namespace Semestr1.ORM
{
    public class MyORM
    {
        // private readonly string _connectionString ;
        // private  NpgsqlConnection _connection;
        // private  NpgsqlCommand _cmd;
        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlCommand _cmd;

        public MyORM(string connectionString)
        {
            // _connectionString = connectionString;
            // _connection = new NpgsqlConnection(connectionString);

            _connection = new NpgsqlConnection(connectionString);
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
            NpgsqlParameter param = new NpgsqlParameter
            {
                ParameterName = name,
                Value = value
            };
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
                        if (reader[p.Name] is DBNull)
                        {
                            p.SetValue(obj, null);
                            return;
                        }

                        p.SetValue(obj, reader[p.Name]);
                    });
                    list.Add(obj);
                }
            }

            return list;
        }

        //Первый столбец первой попавшей строки
        public T ExecuteScalar<T>(string query)
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

        public long CountRows(string query)
        {
            using (_connection)
            {
                _cmd.CommandText = query;
                _connection.Open();

                return (long)_cmd.ExecuteScalar();
            }
        }

        #region useless

        ///// <summary>
        ///// Select * from table
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public IEnumerable<T> Select<T>()
        //{
        //    IList<T> list = new List<T>();
        //    Type t = typeof(T);

        //    using (_connection)
        //    {
        //        //НУЖНО ЧТОБ ТАБЛИЦА НАЗЫВАЛАСЬ Accounts, Users, Cars ... (иначе не робит)
        //        string sqlExpression = $"SELECT * FROM {t.Name}s";

        //        _cmd.CommandText = sqlExpression;

        //        _connection.Open();
        //        var reader = _cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            T obj = (T)Activator.CreateInstance(t);
        //            var temp = t.GetProperties().ToList();


        //            //Не работает тк NULLABLE INT
        //            //var tdsf = temp[2].PropertyType;

        //            //var tdsf3 = temp[2].PropertyType.Name;
        //            ////Convert.ChangeType(queryParams[i], methodParams[i].ParameterType)
        //            //for (int i=0; i<temp.Count; i++)
        //            //{
        //            //    if (temp[i].PropertyType.Equals(tdsf))
        //            //    {

        //            //        t = Nullable.GetUnderlyingType(t);
        //            //        var temp2 = Convert.ChangeType(temp[i], t);
        //            //    }
        //            //}
        //            temp.ForEach(x =>
        //            x.SetValue(obj,Convert.ChangeType(reader[x.Name], x.PropertyType)));

        //            list.Add(obj);
        //        }
        //    }
        //    return list;
        //}
        /// <summary>
        /// Inserts values into table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        // public void Insert<T>(T entity)
        // {
        //     Type t = typeof(T);
        //     var args = t.GetProperties();
        //
        //     var values = args.Select(value => $"@{value.GetValue(entity)}").ToArray();
        //
        //     var argsWithDog = args.Select(value => $"@{value.ToString().Split(" ")[1]}").ToArray();
        //     var argsWithoutDog = args.Select(x => x.Name).ToArray();
        //
        //     foreach (var parameter in args)
        //     {
        //         var sqlParameter = new NpgsqlParameter($"@{parameter.Name}", parameter.GetValue(entity));
        //         _cmd.Parameters.Add(sqlParameter);
        //     }
        //
        //     string nonQuery = $"SET IDENTITY_INSERT {t.Name}s ON " +
        //     $"INSERT INTO {t.Name}s ({string.Join(", ", argsWithoutDog)}) VALUES ({string.Join(", ", argsWithDog)}) " +
        //     $"SET IDENTITY_INSERT {t.Name}s OFF";
        //
        //     ExecuteNonQuery(nonQuery);
        // }
        ////todo
        ///// <summary>
        ///// Updating field by VALUES. ID doesn't matter!
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="entity"></param>
        //public void Update<T>(T entity)
        //{
        //    Type t = typeof(T);
        //    var args = t.GetProperties();

        //    var values = args.Select(value => $"@{value.GetValue(entity)}").ToArray();

        //    var argsWithDog = args.Select(value => $"@{value.ToString().Split(" ")[1]}").ToArray();
        //    var argsWithoutDog = args.Select(x => x.Name).ToArray();

        //    foreach (var parameter in args)
        //    {
        //        var sqlParameter = new SqlParameter($"@{parameter.Name}", parameter.GetValue(entity));
        //        _cmd.Parameters.Add(sqlParameter);
        //    }
        //    //Row = @Value. without ID
        //    string setQuery = "";
        //    for (int i = 1; i < args.Length; i++)
        //    {
        //        setQuery += $"{argsWithoutDog[i]}={argsWithDog[i]}, ";
        //    }
        //    setQuery = setQuery.Remove(setQuery.Length - 2, 2);

        //    string nonQuery = $"UPDATE {t.Name}s SET {setQuery} " +
        //        $"FROM " +
        //        $"(SELECT * FROM {t.Name}s WHERE Id={argsWithDog[0]}) AS Selected " +
        //        $"WHERE {t.Name}s.Id = Selected.Id";

        //    ExecuteNonQuery(nonQuery);
        //}
        ///// <summary>
        ///// Deleting entity by ID
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="entity"></param>
        //public void Delete<T>(T entity)
        //{
        //    Type t = typeof(T);
        //    var args = t.GetProperties();

        //    var values = args.Select(value => $"@{value.GetValue(entity)}").ToArray();

        //    var argsWithDog = args.Select(value => $"@{value.ToString().Split(" ")[1]}").ToArray();
        //    var argsWithoutDog = args.Select(x => x.Name).ToArray();

        //    foreach (var parameter in args)
        //    {
        //        var sqlParameter = new SqlParameter($"@{parameter.Name}", parameter.GetValue(entity));
        //        _cmd.Parameters.Add(sqlParameter);
        //    }

        //    string nonQuery = $"DELETE FROM {t.Name}s " +
        //        $"WHERE {argsWithoutDog[0]}={argsWithDog[0]}";

        //    ExecuteNonQuery(nonQuery);
        //}
        ///// <summary>
        ///// Checking existence of entity by id
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public bool IsExistById<T>(int id)
        //{
        //    Type t = typeof(T);
        //    string nonQuery = $"Select * from {t.Name}s " +
        //        $"WHERE Id={id}";
        //    if (ExecuteNonQuery(nonQuery) > 0)
        //    {
        //        return true;
        //    }
        //    else return false;

        #endregion
    }
}