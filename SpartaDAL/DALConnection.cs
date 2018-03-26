using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Sparta.Dal
{
    public static class DALConnection
    {

        public static SqlConnection GetConnectionByName(string name)
        {
            try
            {
                // create a string connection
                string connString = ConfigurationManager.ConnectionStrings[name].ConnectionString;

                // create a sql connection
                SqlConnection connection = new SqlConnection(connString);

                // open the connection
                connection.Open();

                // return the connection
                return connection; 

            } 
            catch (SqlException e)
            {
                SqlConnection connection = null;
                Console.WriteLine(e.ToString());
                return connection;
            }
        }

        // a methos for closing the sql connection
        public static void CloseSqlConnection (SqlConnection connection)
        {
            connection.Close();
        }


    }
}
