using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Task_3.Interfaces;

namespace Task_3.Connections
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory()
        {
            var config = new ConfigurationBuilder().AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"),
                optional: false, reloadOnChange: false).AddUserSecrets<Program>().Build();

            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public bool TestConnection()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка подключения к базе данных!");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
