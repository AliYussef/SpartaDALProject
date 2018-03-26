using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Sparta.Model;

namespace Sparta.Dal
{
    public static class DALOverzicht
    {
        public static List <Cursus> GetCursussen()
        {
            // create a sql connection
            SqlConnection connection = DALConnection.GetConnectionByName("Reader");

            // create a list for storing data in it
            List<Cursus> coursesList = new List<Cursus>();

            // write a sql query 
            string sqlQuery = "SELECT CursusId, Naam, Niveau, Toelichting, Categorie FROM Cursus";

            // execute the sql query
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataReader reader = command.ExecuteReader();

            // read data from DB
            while (reader.Read())
            {
                Cursus course = new Cursus();
                course.Id = (int)reader["CursusId"];
                course.Naam = Convert.ToString(reader["Naam"]);
                course.Niveau = Convert.ToInt32(reader["Niveau"]);
                course.Toelichting = Convert.ToString(reader["Toelichting"]);
                course.Categorie = (DeelnemerCategorie)reader["Categorie"];

                // add data to thr list
                coursesList.Add(course);
            }

            // close all open connections
            reader.Close();
            DALConnection.CloseSqlConnection(connection);
 
            return coursesList;
        }


    }
}
