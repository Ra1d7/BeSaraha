using Microsoft.Data.SqlClient;
using System.Data;

namespace BeSaraha.DataAccess
{
    public class BeSarahaDB
    {
        private readonly IConfiguration _config;

        public BeSarahaDB(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection GetConnection() => new SqlConnection(_config.GetConnectionString("Default"));
    }
}
