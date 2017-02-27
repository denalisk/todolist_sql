using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
    public class DB
    {
        public static SqlConnection Connection()
        {
            SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }

        public static void CloseSqlConnection(SqlDataReader rdr, SqlConnection conn)
        {
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
}
