using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class DisconnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = True; Initial Catalog = CinemaDb; Server = WINAPUXGGIRX7PJ\SQLEXPRESS ";

        public static void Disconnected()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Costruzione Adapter
                SqlDataAdapter adapter = new SqlDataAdapter();

                //adapter ha una serie di comandi di cui dobbiamo assegnare il comportamento
                //Creiamo comandi da associare all'adapter
                SqlCommand selectCommand = new SqlCommand();
                selectCommand.Connection = connection;
                selectCommand.CommandType = System.Data.CommandType.Text;
                selectCommand.CommandText = "SELECT * FROM Movies";
                //l'idea finale è quella di fare un insert, questo tipo di select ci servirà per vedere tutto

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.Connection = connection;
                insertCommand.CommandType = System.Data.CommandType.Text;
                insertCommand.CommandText = "INSERT INTO Movies VALUES(@Titolo, @Genere, @Durata)";
                //definita la procedura, devo definire i parametri utilizzati

                insertCommand.Parameters.Add("@Titolo", System.Data.SqlDbType.NVarChar, 255, "Titolo");
                //quello che metterò in @Titolo corrisponde al campo titolo della tabella.
                insertCommand.Parameters.Add("@Genere", System.Data.SqlDbType.NVarChar, 255, "Genere");
                insertCommand.Parameters.Add("@Durata", System.Data.SqlDbType.Int, 500, "Durata");
                //500 è una sorta di massimo che fissiamo ai valori che andiamo a inserire

                //dovremmo a questo punto anche definire il Delete e l'Update, ma siccome non li useremo, tralasciamo.

                adapter.SelectCommand = selectCommand;
                adapter.InsertCommand = insertCommand;
                //questi se vogliamo definire dei comandi diversi che ci serviranno, andranno riassegnati nel punto giusto del codice
                //per far sì che i comandi vengano tradotti sul db originale nel modo corretto

                DataSet dataset = new DataSet();
                try
                {
                    connection.Open();
                    adapter.Fill(dataset, "Movies"); //Metto Movies nel dataset, formando la collection di Tables
                    //in pratica si sta scaricando la tabella Movies

                    foreach (DataRow row in dataset.Tables["Movies"].Rows)
                    {
                        Console.WriteLine("Row: {0}", row["Titolo"]);
                    }
                    //Creazione Record:
                    DataRow movie = dataset.Tables["Movies"].NewRow();
                    movie["Titolo"] = "V per vendetta";
                    movie["Genere"] = "Azione";
                    movie["Durata"] = 125;

                    //aggiungo la riga al dataset locale
                    dataset.Tables["Movies"].Rows.Add(movie);

                    //update db originale
                    adapter.Update(dataset, "Movies"); //aggiorno la tabella Movies del db originale prendendola dal mio dataset
                    //quando fa l'Update lui usa le funzioni di Select/Insert/Update/Delete definite sopra
                    //in base a cosa rileva di dover fare. 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    connection.Close();
                }



            }
        }




    }
}
