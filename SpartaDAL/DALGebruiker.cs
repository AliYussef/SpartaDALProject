using Sparta.Dal;
using Sparta.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta.Dal
{
    public static class DALGebruiker
    {
        public static int GetLoginId(int persoonid, string pwdhash)
        {
            SqlConnection connection = DALConnection.GetConnectionByName("Reader");
            // find login based on the person and the hash
            string query = @"
                SELECT LoginId 
                FROM Login
                WHERE PersoonId = @person AND PwdHash = @hash;
            ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("person", persoonid);
            command.Parameters.AddWithValue("hash", pwdhash);

            SqlDataReader reader = command.ExecuteReader();

            int loginId = 0;

            // should only be one, so no loop
            if (reader.Read())
            {
                loginId = (int)reader["LoginId"];
            }

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
            string sqlQuery = @"
                INSERT INTO ContactInfo(
                    PersoonId,
                    Straat,
                    Huisnummer,
                    Huisnummertoevoeging,
                    Plaats,
                    Postcode,
                    Email,
                    Telefoon
                ) VALUES (
                    @PersoonId,
                    @Straat,
                    @Huisnummer,
                    @Huisnummertoevoeging,
                    @Plaats,
                    @Postcode,
                    @Email,
                    @Telefoon
                )";
            SqlCommand command = new SqlCommand(sqlQuery, connection);

            //replace parameters by values
            command.Parameters.AddWithValue("PersoonId", info.Persoonid);
            command.Parameters.AddWithValue("Straat", info.Straat);
            command.Parameters.AddWithValue("Huisnummer", info.Huisnummer);
            command.Parameters.AddWithValue("Huisnummertoevoeging", info.Huisnummertoevoeging);
            command.Parameters.AddWithValue("Plaats", info.Plaats);
            command.Parameters.AddWithValue("Postcode", info.Postcode);
            command.Parameters.AddWithValue("Email", info.Email);
            command.Parameters.AddWithValue("Telefoon", info.Telefoon);
            command.ExecuteNonQuery();
            //close connection

            DALConnection.CloseSqlConnection(connection);

        }

        public static Contact GetContactInfoByPersoonId(int persoonId)
        {
            // NOTE: only here for testing! otherwise cannot test the other methods

            SqlConnection connection = DALConnection.GetConnectionByName("Reader");

            string query = @"
                SELECT * FROM ContactInfo
                WHERE PersoonId = @person
            ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("person", persoonId);

            SqlDataReader reader = command.ExecuteReader();

            Contact contact = new Contact();

            contact.Persoonid = persoonId;

            if (!reader.Read())
            {
                // user not found
                return contact;
            }

            contact.Id = (int)reader["ContactInfoId"];
            contact.Persoonid = (int)reader["PersoonId"];
            contact.Straat = (string)reader["Straat"];
            contact.Huisnummer = (int)reader["Huisnummer"];
            contact.Huisnummertoevoeging = (string)reader["Huisnummertoevoeging"];
            contact.Plaats = (string)reader["Plaats"];
            contact.Postcode = (string)reader["Postcode"];
            contact.Email = (string)reader["Email"];
            contact.Telefoon = (string)reader["Telefoon"];

            reader.Close();
            DALConnection.CloseSqlConnection(connection);

            return contact;
        }

        public static void vernieuwContactInfo(Contact info)
        {
            SqlConnection connection = DALConnection.GetConnectionByName("Writer");

            //update user info, creating the query
            string sqlQuery = "UPDATE ContactInfo SET PersoonId = @PersoonId, Straat = @Straat, Huisnummer = @Huisnummer, Huisnummertoevoeging = @Huisnummertoevoeging, Plaats = @Plaats, Postcode = @Postcode, Email = @Email, Telefoon = @Telefoon WHERE ContactInfoId = @ContactInfoId";
            SqlCommand command = new SqlCommand(sqlQuery, connection);


            //replace parameters by values
            command.Parameters.AddWithValue("PersoonId", info.Persoonid);
            command.Parameters.AddWithValue("Straat", info.Straat);
            command.Parameters.AddWithValue("Huisnummer", info.Huisnummer);
            command.Parameters.AddWithValue("Huisnummertoevoeging", info.Huisnummertoevoeging);
            command.Parameters.AddWithValue("Plaats", info.Plaats);
            command.Parameters.AddWithValue("Postcode", info.Postcode);
            command.Parameters.AddWithValue("Email", info.Email);
            command.Parameters.AddWithValue("Telefoon", info.Telefoon);
            command.Parameters.AddWithValue("ContactInfoId", info.Id);
            command.ExecuteNonQuery();
            //close connection

            DALConnection.CloseSqlConnection(connection);
        }

        public static Persoon checkCredentials(Login user)
        {
            // create a sql connection
            SqlConnection connection = DALConnection.GetConnectionByName("Reader");

            // get a person ID
            int personId = PersonId(user);

            // write a sql query 
            string SQLquery = @"SELECT PersoonId, Naam, Achternaam, Categorie, GeboorteDatum 
                                FROM Persoon WHERE PersoonId = @PersoonId ";

            // execute the sql query
            SqlCommand command = new SqlCommand(SQLquery, connection);
            command.Parameters.AddWithValue("@PersoonId", personId);
            command.ExecuteNonQuery();

            // read from db
            SqlDataReader reader = command.ExecuteReader();

            Persoon persoon = new Persoon();
            while (reader.Read())
            {
                persoon.Persoonid = (int)reader["PersoonId"];
                persoon.Naam = Convert.ToString(reader["Naam"]);
                persoon.Achternaam = Convert.ToString(reader["Achternaam"]);
                persoon.Geboortedatum = DateTime.Parse(reader["GeboorteDatum"].ToString());
                persoon.Categorie = (DeelnemerCategorie)reader["Categorie"];
            }

            // close all connections
            reader.Close();
            DALConnection.CloseSqlConnection(connection);

            return persoon;
        }

        public static int PersonId(Login aanmeld)
        {
            // create a sql connection
            SqlConnection connection = DALConnection.GetConnectionByName("Reader");

            // write a sql query 
            string sqlQuery = @"SELECT PersoonId FROM login 
                                where AanmeldNaam = @AanmeldNaam AND PwdHash = @PwdHash";

            // execute the sql query
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@AanmeldNaam", aanmeld.Naam);
            command.Parameters.AddWithValue("@PwdHash", aanmeld.Pwdhash);
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();

            int personId = 0;
            while (reader.Read())
            {
                personId = (int)reader["PersoonId"];
            }

            // close all connections
            reader.Close();
            DALConnection.CloseSqlConnection(connection);

            return personId;
        }

        public static void RegistreerPersoon(DeelnemerCategorie category, string naam, string achternaam,
            DateTime gebdatum, Login aanmeld)
        {
            // add new user to the person table
            int persId = GetPersonId(category, naam, achternaam, gebdatum);

            // add new person to the login table
            SetLoginInfo(aanmeld, persId);
        }

        public static int GetPersonId(DeelnemerCategorie category, string naam, string achternaam,
            DateTime gebdatum)
        {
            // create a sql connection
            SqlConnection connection = DALConnection.GetConnectionByName("Writer");

            // write a sql query 
            string sqlQuery = @"INSERT INTO Persoon(Naam, Achternaam, Categorie, GeboorteDatum) 
                                VALUES(@Naam, @Achternaam, @Categorie, @GeboorteDatum)";

            string sqlQuery1 = @"SELECT PersoonId FROM Persoon 
                                 where Naam = @Naam AND Achternaam = @Achternaam";

            // execute the sql query
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Naam", naam);
            command.Parameters.AddWithValue("@Achternaam", achternaam);
            command.Parameters.AddWithValue("@Categorie", category);
            command.Parameters.AddWithValue("@GeboorteDatum", gebdatum);
            command.ExecuteNonQuery();

            // execute the sql query
            SqlCommand command1 = new SqlCommand(sqlQuery1, connection);
            command1.Parameters.AddWithValue("@Naam", naam);
            command1.Parameters.AddWithValue("@Achternaam", achternaam);
            int id = (int)command1.ExecuteScalar();

            // close the connection
            DALConnection.CloseSqlConnection(connection);

            return id;
        }

        public static void SetLoginInfo(Login aanmeld, int persId)
        {
            // create a sql connection
            SqlConnection connection = DALConnection.GetConnectionByName("Writer");

            // write a sql query 
            string sqlQuery = @"INSERT INTO Login(AanmeldNaam, PwdHash, PersoonId) 
                                VALUES(@AanmeldNaam, @PwdHash, @PersoonId)";

            // execute the sql query
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@AanmeldNaam", aanmeld.Naam);
            command.Parameters.AddWithValue("@PwdHash", aanmeld.Pwdhash);
            command.Parameters.AddWithValue("@PersoonId", persId);
            command.ExecuteNonQuery();

            // close the connection
            DALConnection.CloseSqlConnection(connection);
        }



    }
}
