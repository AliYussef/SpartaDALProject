using Sparta.Dal;
using Sparta.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDAL
{
    class DALGebruiker
    {

        public static void voegtoeContactInfo(Contact info)
        {
            
            SqlConnection connection = DALConnection.GetConnectionByName("Writer");

            //insert a new user information
            string sqlQuery = "INSERT INTO ContactInfo(ContactInfoId, PersoonId, Straat, Huisnummer, Huisnummertoevoeging, Plaats, Postcode, Email, Telefoon) VALUES(@ContactInfoId, @PersoonId, @Straat, @Huisnummer, @Huisnummertoevoeging, @Plaats, @Postcode, @Email, @Telefoon)";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataReader reader = command.ExecuteReader();

            //replace parrameters by values
            command.Parameters.AddWithValue("@ContactInfoId", info.Id);
            command.Parameters.AddWithValue("@PersoonI", info.Persoonid);
            command.Parameters.AddWithValue("@Straat", info.Straat);
            command.Parameters.AddWithValue("@Huisnummertoevoeging", info.Huisnummertoevoeging);
            command.Parameters.AddWithValue("@Plaats", info.Plaats);
            command.Parameters.AddWithValue("@Postcode", info.Postcode);
            command.Parameters.AddWithValue("@Email", info.Email);
            command.Parameters.AddWithValue("@Telefoon", info.Telefoon);

            //close connection
            reader.Close();
            DALConnection.CloseSqlConnection(connection);

        }

        public static void vernieuwContactInfo(Contact info)
        {
            SqlConnection connection = DALConnection.GetConnectionByName("Writer");

            //update user info, creating the query
            string sqlQuery = "UPDATE ContactInfo SET PersoonId = @PersoonId, Straat = @Straat, Huisnummer = @Huisnummer, Huisnummertoevoeging = @Huisnummertoevoeging, Plaats = @Plaats, Postcode = @Postcode, Email = @Email, Telefoon = @Telefoon WHERE ContactInfoId = @ContactInfoId";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataReader reader = command.ExecuteReader();

            //replace parameters by values
            command.Parameters.AddWithValue("@PersoonI", info.Persoonid);
            command.Parameters.AddWithValue("@Straat", info.Straat);
            command.Parameters.AddWithValue("@Huisnummertoevoeging", info.Huisnummertoevoeging);
            command.Parameters.AddWithValue("@Plaats", info.Plaats);
            command.Parameters.AddWithValue("@Postcode", info.Postcode);
            command.Parameters.AddWithValue("@Email", info.Email);
            command.Parameters.AddWithValue("@Telefoon", info.Telefoon);
            command.Parameters.AddWithValue("@ContactInfoId", info.Id);

            //close connection
            reader.Close();
            DALConnection.CloseSqlConnection(connection);

        }
    }
}
