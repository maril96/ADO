using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class ConnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = True; Initial Catalog = CinemaDb; Server = WINAPUXGGIRX7PJ\SQLEXPRESS ";
    //Persist Security Info ci dice se salvare la password (se è True la salva); Integrated Security ci dice se stiamo accedendo con Windows Autentication, quindi con acocunt windows;
    //(se è false dovremo mettere Username e Password); poi abbiamo il nome del Database e il nome del Server (quello a cui ci connettiamo accedendo a SSMS.

        public static void Connected()
        {
            //1.Creare connessione
            //Metodo1:
            //SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = connectionString;
            //Metodo2:
            //SqlConnection connection = new SqlConnection(connectionString);
            //Metodo3:
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //in questo caso se avrò più connessioni avrò più using: nei primi due metodi poi dovremmo fare il Dispose, invece così lo fa da solo
                //2.Aprire connessione
                connection.Open();

                //3.Creare command (ed eventualmente 4. dei parametri) 
                SqlCommand command = new SqlCommand();
                command.Connection = connection; //a che database ci stiamo riferendo
                //oppure: SqlCommand command= connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Movies";
                //oppure, per la creazione del command e la sua inizializzazione: SqlCommand command = new SqlCommand("SELECT * FROM Movies", connection);
                
                //5.Eseguire Command (in questo caso ritorniamo una tabella per cui useremo DataReader)
                SqlDataReader reader = command.ExecuteReader();
                
                //6.Leggere dati 
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} ({2}) {3} minuti", reader["ID"], reader["Titolo"], reader["Genere"], reader["Durata"]);
                    //qui devo mettere come argomento di reader[] il nome della colonna sottoforma di stringa.
                }

                //7.Chiudere connessione
                reader.Close();
                connection.Close();
            }





        }

        public static void ConnectedWithParameter()
        {
            //Creare Connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Inserisco parametro da riga di comando
                Console.WriteLine("Genere del Film: ");
                string Genere=Console.ReadLine();

                //Aprire la connessione
                connection.Open();

                //Creare il command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Movies WHERE Genere=@Genere";

                //Creare Parametro
                SqlParameter genereParam = new SqlParameter();
                genereParam.ParameterName = "@Genere";
                genereParam.Value = Genere;
                command.Parameters.Add(genereParam);
                //altro modo per scrivere in un unico statement:
                //command.Parameters.AddWithValue("@genere", Genere);

                //Eseguire il command
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dei dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} ({2})", reader["ID"], reader["Titolo"], reader["Genere"]);
                }

                //Chiudere connessione e reader
                reader.Close();
                connection.Close();

            }
        }
   
        public static void ConnectedStoredProcedure()
        {
            //Creare connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //apro connessione
                connection.Open();
                //creare command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "stpGetActorByCachetRange";
                //qui gli dirò di eseguire la stp con questo nome

                //creare parametri
                //la procedure che voglio usare usa due parametri
                command.Parameters.AddWithValue("@min_cachet", 5000);
                //il nome del parametro deve corrispondere esattamente a quello definito nella stored procedure
                command.Parameters.AddWithValue("@max_cachet", 9000);

                //Creare valore di ritorno
                SqlParameter returnValue = new SqlParameter();
                returnValue.ParameterName = "@returnedCount";
                returnValue.SqlDbType = System.Data.SqlDbType.Int; //non è necessario, lo è se uso tipo nvarchar perchè vuole anche la lunghezza
                returnValue.Direction = System.Data.ParameterDirection.Output; //Devo per forza dirgli che è un output

                command.Parameters.Add(returnValue);

                //eseguire il command + visualizzare risultati

                SqlDataReader reader = command.ExecuteReader(); //qui stiamo eseguendo la storedProcedure
                //definendo reader come SqlDataReader gli dico che quello che otterrò tramite il comando sarà una tabella

                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2} {3}", reader["ID"], reader["FirstName"], reader["LastName"], reader["Cachet"]);
                }


                reader.Close();
                //voglio anche però ritrovare il parametro di ritorno:


                //oppure, per visualizzare SOLO il parametro di ritorno, posso non fare l'ExecuteReader e fare:
                //command.ExecuteNonQuery();
                //e mi modifica solo il valore di ritorno, che stampo come segue...

                Console.WriteLine("#Actors: {0}", command.Parameters["@returnedCount"].Value);


                connection.Close();
          
            }
        }
 
        public static void ConnectedScalar()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand scalarCommand = new SqlCommand();
                scalarCommand.Connection = connection;
                scalarCommand.CommandType = System.Data.CommandType.Text;
                scalarCommand.CommandText = "SELECT COUNT(*) FROM Movies";

                int count = (int) scalarCommand.ExecuteScalar(); //mi restituisce in generale un oggetto da definizione, ma sappiamo che è un inter
                //ExecuteScalar restituisce in realtà una tabella con una riga e una colonna, codificato come object...in realtà si esegue un comando
                //che restituisce una tabella. Di questa l'output di ExecuteScalar() sarà la prima riga e la prima colonna.

                Console.WriteLine("Conteggio dei film: {0}", count);

                connection.Close();

            }
        }
    
    }
}
