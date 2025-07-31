using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;


namespace ResourceTracker.DAO
{
    public class SqlConnect
    {
        private readonly IConfiguration _configuration;

        public SqlConnect(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
