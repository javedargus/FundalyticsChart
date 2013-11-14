using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FundalyticsChart.AppCode
{

    public class Database
    {

        public string ConnectionString = @"Server=database.fundalytics.argusdata.com;Database=CoreData;User Id=aardvark;Password=monument1358";
        Connection _db;

        public Database()
        {
            _db = new Connection(ConnectionString);
        }

        public DataTable GetDataTable(string sql)
        {
            try
            {
                var ds = new DataSet();
                var adapter = new SqlDataAdapter(sql, _db.Instance);
                adapter.Fill(ds);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                _db.Close();
            }
        }

        class Connection
        {
            SqlConnection _connection;

            public Connection(string connectionString)
            {
                _connection = new SqlConnection(connectionString);
            }

            public SqlConnection Instance
            {
                get
                {
                    if (_connection.State != ConnectionState.Open) _connection.Open();
                    return _connection;
                }
            }

            public void Close()
            {
                if (_connection != null && _connection.State != ConnectionState.Closed) _connection.Close();
            }
        }
    }
}