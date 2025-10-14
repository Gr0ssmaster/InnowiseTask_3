using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Task_3
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory()
        {
            var config = new ConfigurationBuilder().AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"),
                optional: false, reloadOnChange: false).Build();

            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
