using Dapper;
using System.Data.SqlClient;

namespace SysPet.Data
{
    public class Repository
    {
        private readonly string connectionString;

        public Repository()
        {
            connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Pets;Integrated Security=True;Connect Timeout=30;Encrypt=False;MultipleActiveResultSets=True;";
        }

        public void GuardarDatosConReintentoAutomatico()
        {
            int intentos = 3; // Número de intentos antes de fallar definitivamente

            while (intentos > 0)
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                using var transaction = connection.BeginTransaction();
                try
                {
                    connection.Execute("INSERT INTO TuTabla (Columna1, Columna2) VALUES (@Valor1, @Valor2)",
                        new { Valor1 = "Dato1", Valor2 = "Dato2" }, transaction);
                    transaction.Commit();
                    break;
                }
                catch (SqlException ex) when (ex.Number == 1205) // Número de error para deadlock
                {
                    intentos--;
                    Thread.Sleep(1000);
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                }
            }

            if (intentos == 0)
            {
                throw new ApplicationException("No se pudo completar la operación después de múltiples intentos.");
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
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
            using var connection = new SqlConnection(connectionString);
            connection.Open();
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
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>(sql, param);
                return result;
            }
            catch (System.Exception ex)
            {

            }

            return default;
        }

        public int Execute<T>(string sql)
        {
            var result = 0;
            int intentos = 3; // Número de intentos antes de fallar definitivamente

            while (intentos > 0)
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                try
                {
                    result = connection.Execute(sql);
                    break;
                }
                catch (SqlException ex) when (ex.Number == 1205) // Número de error para deadlock
                {
                    intentos--;
                    Thread.Sleep(1000);
                }
            }

            if (intentos == 0)
            {
                throw new ApplicationException("No se pudo completar la operación después de múltiples intentos.");
            }

            return result;
        }

        public int ExecuteWithId<T>(string sql)
        {
            var result = 0;
            int intentos = 3; // Número de intentos antes de fallar definitivamente

            while (intentos > 0)
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                try
                {
                    result = connection.Query<int>(sql).Single();
                    break;
                }
                catch (SqlException ex) when (ex.Number == 1205) // Número de error para deadlock
                {
                    intentos--;
                    Thread.Sleep(1000);
                }
            }

            if (intentos == 0)
            {
                throw new ApplicationException("No se pudo completar la operación después de múltiples intentos.");
            }

            return result;
        }

        public int Execute<T>(string sql, object param)
        {
            var result = 0;
            int intentos = 3; // Número de intentos antes de fallar definitivamente

            while (intentos > 0)
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                try
                {
                    result = connection.Execute(sql, param);
                    break;
                }
                catch (SqlException ex) when (ex.Number == 1205) // Número de error para deadlock
                {
                    intentos--;
                    Thread.Sleep(1000);
                }
            }

            if (intentos == 0)
            {
                throw new ApplicationException("No se pudo completar la operación después de múltiples intentos.");
            }

            return result;
        }

    }
}
