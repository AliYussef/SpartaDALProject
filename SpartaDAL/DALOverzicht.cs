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
            SqlConnection connection = DALConnection.GetConnectionByName("Reader");


            List<Cursus> coursesList = new List<Cursus>();

            string sqlQuery = "SELECT CursusId, Naam, Niveau, Toelichting, Categorie FROM Cursus";

            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Cursus course = new Cursus();
                course.Id = (int)reader["CursusId"];
                course.Naam = Convert.ToString(reader["Naam"]);
                course.Niveau = Convert.ToInt32(reader["Niveau"]);
                course.Toelichting = Convert.ToString(reader["Toelichting"]);
                course.Categorie = (DeelnemerCategorie)reader["Categorie"];



                coursesList.Add(course);
            }

            reader.Close();
            DALConnection.CloseSqlConnection(connection);
           

            return coursesList;
        }


    }
}
