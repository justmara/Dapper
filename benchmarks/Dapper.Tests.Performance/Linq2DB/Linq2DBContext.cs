using System.Data.Common;
using LinqToDB;
using LinqToDB.DataProvider.SqlServer;

namespace Dapper.Tests.Performance.Linq2Db
{
    public class Linq2DBContext : LinqToDB.Data.DataConnection
    {
        public Linq2DBContext(DbConnection connection) : base (SqlServerTools.GetDataProvider(SqlServerVersion.v2017, SqlServerProvider.MicrosoftDataSqlClient), connection) {}
        public ITable<Post> Posts => this.GetTable<Post>();
    }
}
