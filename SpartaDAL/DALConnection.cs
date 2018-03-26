﻿using System;
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
               //jihouoij
                string connString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
                SqlConnection connection = new SqlConnection(connString);
                connection.Open();
                return connection; // changed!

            } 
            catch (SqlException e)
            {
                SqlConnection connection = null;
                Console.WriteLine(e.ToString());
                return connection;
            }

        }

        public static void CloseSqlConnection (SqlConnection connection)
        {
            connection.Close();
        }


    }
}
