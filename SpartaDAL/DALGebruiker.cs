using Sparta.Dal;
using Sparta.Model;
using System.Data.SqlClient;

namespace SpartaDAL
{
    class DALGebruiker
    {
        public static int GetLoginId(int persoonid, string pwdhash)
        {
            SqlConnection connection = DALConnection.GetConnectionByName("Reader");

            string query = @"
                SELECT LoginId 
                FROM Login
                WHERE PersoonId = @person AND PwdHash = @hash;
            ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("person", persoonid);
            command.Parameters.AddWithValue("hash", pwdhash);

            SqlDataReader reader = command.ExecuteReader();

            // should only be one, so no loop
            reader.Read();

            int loginId = (int)reader["LoginId"];

            reader.Close();
            DALConnection.CloseSqlConnection(connection);

            return loginId;
        }

        public static void UpdatePwd(int loginid, string pwdhash)
        {
            SqlConnection connection = DALConnection.GetConnectionByName("Writer");

            string query = @"
                UPDATE Login 
                SET PwdHash = @hash
                WHERE LoginId = @id;
            ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("id", loginid);
            command.Parameters.AddWithValue("hash", pwdhash);

            command.ExecuteNonQuery();

            DALConnection.CloseSqlConnection(connection);
        }

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
