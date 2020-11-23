using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EsercitazioneADO
{
    public class ConnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = True; Initial Catalog = Polizia; Server = WINAPUXGGIRX7PJ\SQLEXPRESS ";

        /// <summary>
        /// Il metodo stampa in Console gli Agenti assegnati ad una certa Area.
        /// L'area viene scelta inserendone l'ID da tastiera.
        /// </summary>
        public static void AgentiAssegnati()
        {
            Console.WriteLine("Inserisci ID Area:");
            int idArea = Int32.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Creo il comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM AgenteDiPolizia AS a" +
                    "                   INNER JOIN BridgeAgenteArea AS b" +
                    "                    ON b.AgenteID=a.ID  WHERE b.AreaID=@idArea";


                //Creo il parametro
                SqlParameter paramArea = new SqlParameter();
                paramArea.ParameterName = "@idArea";
                paramArea.Value = idArea;
                command.Parameters.Add(paramArea);

                //Eseguo il comando
                SqlDataReader reader = command.ExecuteReader();

                //Visualizzo i risultati
                while (reader.Read())
                {

                    string dataCompleta = reader["DataDiNascita"].ToString();
                    String[] splitData = dataCompleta.Split(' ');
                    string data = splitData[0];
                    Console.WriteLine("{0} - {1} {2}, CF: {3}, Data di nascita: {4}, Anni di servizio: {5}", reader["ID"], reader["Nome"], reader["Cognome"], reader["CodiceFiscale"], data, reader["AnniDiServizio"]);
                }

                reader.Close();

                //Chiudo la connessione
                connection.Close();

            }

        }

        /// <summary>
        /// Il metodo chiede di inserire da tastiera i dati relativi ad un nuovo record Agente da inserire nella tabella.
        /// Fatto ciò, il nuovo record viene inserito nella tabella AgenteDiPolizia del database selezionato.
        /// Non è previsto nessun ritorno
        /// </summary>
        public static void InserisciAgente()
        {
            Console.WriteLine("Nome agente:");
            string nome = Console.ReadLine();
            Console.WriteLine("Cognome agente:");
            string cognome = Console.ReadLine();
            Console.WriteLine("Codice Fiscale:");
            string cf = Console.ReadLine();
            Console.WriteLine("Data di nascita (yyyy-mm-dd) :");
            string dataDiNascita = Console.ReadLine();
            Console.WriteLine("Anni di servizio:");
            string anniDiServizio = Console.ReadLine();


            //bisogna mettere un try catch per vedere se c'è qualcosa che non va, ad esempio il cf dev'essere di esattamente
            //16 caratteri, quindi se gli passiamo qualcosa di sbagliato si blocca (c'è il vincolo sul database)

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Apro la connessione
                connection.Open();

                //Creo il comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO AgenteDiPolizia VALUES (@name, @surname, @cf, @birthdate, @yearsofwork)";

                //Creo i parametri
                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@name";
                paramName.Value = nome;
                command.Parameters.Add(paramName);

                SqlParameter paramSurname = new SqlParameter();
                paramSurname.ParameterName = "@surname";
                paramSurname.Value = cognome;
                command.Parameters.Add(paramSurname);

                SqlParameter paramCF = new SqlParameter();
                paramCF.ParameterName = "@cf";
                paramCF.Value = cf;
                command.Parameters.Add(paramCF);

                SqlParameter paramBirthdate = new SqlParameter();
                paramBirthdate.ParameterName = "@birthdate";
                paramBirthdate.Value = DateTime.Parse(dataDiNascita);
                command.Parameters.Add(paramBirthdate);

                SqlParameter paramAL = new SqlParameter();
                paramAL.ParameterName = "@yearsofwork";
                paramAL.Value = Int32.Parse(anniDiServizio);
                command.Parameters.Add(paramAL);

                //Eseguo il comando
                command.ExecuteNonQuery();

                //Chiudo la connessione
                connection.Close();
            }

        }

        /// <summary>
        /// Il metodo stampa in Console tutta la tabella AgenteDiPolizia del DataBase
        /// </summary>
        public static void StampaAgenti()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM AgenteDiPolizia";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    string dataCompleta = reader["DataDiNascita"].ToString();
                    String[] splitData = dataCompleta.Split(' ');
                    string data = splitData[0];
                    Console.WriteLine("{0} - {1} {2}, CF: {3}, Data di nascita: {4}, Anni di servizio: {5}", reader["ID"], reader["Nome"], reader["Cognome"], reader["CodiceFiscale"], data, reader["AnniDiServizio"]);
                }

                reader.Close();
                connection.Close();

            }

        }

    }
}
