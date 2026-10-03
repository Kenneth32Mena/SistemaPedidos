using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SistemaPedidos.Infrastructure.Data
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Server=(localdb)\\MSSQLLocalDB;Database=ArquitecturaCapasPedidosDB;Integrated Security=True;MultipleActiveResultSets=True;";
        }

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            return connection;
        }

        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
