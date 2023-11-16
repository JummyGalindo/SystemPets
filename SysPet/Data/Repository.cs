using Dapper;
using System.Data.SqlClient;

namespace SysPet.Data
{
    public class Repository
    {
        private readonly SqlConnection connection;
        private readonly string connectionString;

        public Repository()
        {
            connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Pets;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            connection  = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            try
            {
                var resut = await connection.QueryAsync<T>(query);
                return resut;
            }
            catch
            {
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object param)
        {
            try
            {
                var resut = await connection.QueryAsync<T>(query, param);
                return resut;
            }
            catch
            {
                return new List<T>();
            }
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object param)
        {
            var result = await connection.QuerySingleOrDefaultAsync<T>(sql, param);
            return result;
        }

        public int Execute<T>(string sql)
        {
            var result = connection.Execute(sql);
            return result;
        }

        public int ExecuteWithId<T>(string sql)
        {
            var result = connection.Query<int>(sql).Single();
            return result;
        }

        public int Execute<T>(string sql, object param)
        {
            var result = connection.Execute(sql, param);
            return result;
        }

    }
}
