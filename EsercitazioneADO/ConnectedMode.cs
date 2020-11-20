using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EsercitazioneADO
{
    public class ConnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = True; Initial Catalog = Polizia; Server = WINAPUXGGIRX7PJ\SQLEXPRESS ";
   
        public static void AgentiAssegnati()
        {
            Console.WriteLine("Inserisci ID Area:");
            int idArea = Int32.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString)) {
      
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM AgenteDiPolizia AS a" +
                    "                   INNER JOIN BridgeAgenteArea AS b" +
                    "                    ON b.AgenteID=a.ID  WHERE b.AreaID=@idArea";


                SqlParameter paramArea = new SqlParameter();
                paramArea.ParameterName = "@idArea";
                paramArea.Value = idArea;
                command.Parameters.Add(paramArea);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2}, CF: {3}, Data di nascita: {4}, Anni di servizio: {5}", reader["ID"], reader["Nome"], reader["Cognome"], reader["CodiceFiscale"], reader["DataDiNascita"], reader["AnniDiServizio"]);
                }

                reader.Close();
                connection.Close();

            }

        }

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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "INSERT INTO AgenteDiPolizia VALUES (@name, @surname, @cf, @birthdate, @yearsofwork)";


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
                paramBirthdate.Value = dataDiNascita;
                command.Parameters.Add(paramBirthdate);

                SqlParameter paramAL = new SqlParameter();
                paramAL.ParameterName = "@yearsofwork";
                paramAL.Value = anniDiServizio;
                command.Parameters.Add(paramAL);


                command.ExecuteNonQuery();


                connection.Close();
            }

        }

    }
}
