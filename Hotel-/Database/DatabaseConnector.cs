using Hotel_.Databases;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Hotel_.Database.Database
{
    public static class DatabaseConnector
    {
        // stel in waar de database gevonden kan worden
        //string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=fastfood;Uid=110712;Pwd=inf2122sql;";
        //string connectionString = "Server=172.16.160.21;Port=3306;Database=110712;Uid=110712;Pwd=inf2122sql;"; ;
        private static string connectionString = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7612776;Uid=sql7612776;Pwd=KddxAyvhem;";


        public static List<Dictionary<string, object>> GetRows(string query)
        {
            // maak een lege lijst waar we de namen in gaan opslaan
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            //return new List<Dictionary<string, object>>();
            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    var tableData = reader.GetSchemaTable();

                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        // haal voor elke kolom de waarde op en voeg deze toe
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }

                        rows.Add(row);
                    }
                }
            }

            // return de lijst met namen
            return rows;
        }

        public static void SaveGebruiker(Gebruiker gebruiker)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO gebruikers(naam, wachtwoord) VALUES(?naam, ?wachtwoord)", conn);

                // Elke parameter moet je handmatig toevoegen aan de query
                cmd.Parameters.Add("?naam", MySqlDbType.Text).Value = gebruiker.Naam;
                cmd.Parameters.Add("?wachtwoord", MySqlDbType.Text).Value = gebruiker.Wachtwoord;
                cmd.ExecuteNonQuery();
            }
        }

        /*public static Gebruiker GetGebruiker(string naam)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM gebruikers WHERE naam = '{naam}'", conn);

                var reader = cmd.ExecuteReader();
                reader.Read();
                Gebruiker gebruiker = new Gebruiker();
                gebruiker.Naam = reader.GetValue(1);
                gebruiker.Wachtwoord = reader.GetValue(2);
                //gebruiker.Naam = row["naam"].ToString();
                //gebruiker.Wachtwoord = row["wachtwoord"].ToString();
                return gebruiker;
            }
            return null;
        }*/

        public static void SaveContact(Contact contact)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO contact(voornaam, achternaam, email, bericht) VALUES(?voornaam, ?achternaam, ?email, ?bericht)", conn);

                // Elke parameter moet je handmatig toevoegen aan de query
                cmd.Parameters.Add("?naam", MySqlDbType.Text).Value = contact.Naam;
                cmd.Parameters.Add("?email", MySqlDbType.Text).Value = contact.Email;
                cmd.Parameters.Add("?telefoon", MySqlDbType.Text).Value = contact.Telefoon;
                cmd.Parameters.Add("?bericht", MySqlDbType.Text).Value = contact.Bericht;
                cmd.ExecuteNonQuery();
            }
        }
    }
}