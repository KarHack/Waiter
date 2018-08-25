using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waiter
{
    /*
     * 
     * Waiter Manager is supposed to Keep a track of all the waiters that are currently running.
     * It will maintain the status of every waiter.
     * This will also Interact with the database.
     * 
     */
    public static class WaiterManager
    {

        // Variables
        private static string Status { get; set; }
        private static StringBuilder StatusTrace { get; set; }
        private static Dictionary<string, Waiter> waiters;
        private static bool initialRun = true;
        //private static DBConnector dbConn;    TODO : Test & Release

        // Methods.
        // This will allow the waiters to start and function properly.
        internal static string Init(Waiter waiter)
        {
            try
            {
                // Here we will perform the required initialization.
                if (waiters == null)
                {
                    // Initialize the Waiter Map.
                    waiters = new Dictionary<string, Waiter>();
                }
                /*
                     * TODO : Test & Release.
                if (dbConn == null)
                {
                    // Initialize the Database Connection.
                    dbConn = new DBConnector();
                }*/

                // We will also flush the database if we require to.
                if (initialRun)
                {
                    // This is the first run.
                    // Here we will flush the database.
                    FlushDB();
                    // The initial run has been completed
                    initialRun = false;
                }

                // Add the Waiter to the Dictionary.
                string waiterID = Helper.StringManu.Generate(12);
                waiters.Add(waiterID, waiter);
                // Send the ID to the Waiter.
                return waiterID;
            }
            catch (Exception e)
            {
                // There was an Error.
                AddStatus("Init Err : " + e.Message);
                // Tell the Waiter there was an Error.
                return "Err : " + e.Message;
            }
        }

        // This will update the cache.
        internal static void Update(Waiter waiter)
        {
            try
            {
                // Here we will update the Cache.
                if (waiters.ContainsKey(waiter.ID))
                {
                    waiters[waiter.ID] = waiter;
                }
                else
                {
                    // Add the Value.
                    waiters.Add(waiter.ID, waiter);
                }
            }
            catch (Exception e)
            {
                // There was an error.
            }
        }

        // This will insert that the query is running here.
        internal static void Begin(Waiter waiter)
        {
            try
            {
                // Let's insert the waiter as running in the database.
                // Here we will update the Cache.
                if (waiters.ContainsKey(waiter.ID))
                {
                    waiters[waiter.ID] = waiter;
                }
                else
                {
                    // Add the Value.
                    waiters.Add(waiter.ID, waiter);
                }
                try
                {
                    /*
                     * TODO : Test & Release.
                    // Now we insert the waiter into the database.
                    dbConn.InsertSess.CommandText = "INSERT INTO requests (id, url, endpoint, method, data) "
                        + "VALUES (@id, @url, @endpoint, @method, @data)";
                    // Bind the params
                    SQLiteParameter idParam = new SQLiteParameter("@id", waiter.ID);
                    SQLiteParameter urlParam = new SQLiteParameter("@url", waiter.url);
                    SQLiteParameter endpointParam = new SQLiteParameter("@endpoint", waiter.endpoint);
                    SQLiteParameter methodParam = new SQLiteParameter("@method", (int)waiter.callMethod);
                    SQLiteParameter dataParam = new SQLiteParameter("@data", waiter.data);
                    // Add the Params to the SQL Command.
                    dbConn.InsertSess.Parameters.Add(idParam);
                    dbConn.InsertSess.Parameters.Add(urlParam);
                    dbConn.InsertSess.Parameters.Add(endpointParam);
                    dbConn.InsertSess.Parameters.Add(methodParam);
                    dbConn.InsertSess.Parameters.Add(dataParam);
                    // Prepare the SQL Command.
                    dbConn.InsertSess.Prepare();
                    dbConn.InsertSess.ExecuteNonQuery();
                    */
                }
                catch (Exception e)
                {
                    // There was an Error.
                }
            }
            catch (Exception e)
            {
                // There was an error.
            }
        }

        // This will delete the waiter from the running requests.
        internal static void End(Waiter waiter)
        {
            try
            {
                // Here we will remove the waiter from the requests table, as the waiter is completed its request.
                // Remove the Waiter from the cache of waiters.
                waiters.Remove(waiter.ID);

                // Now lets Remove the waiter from the database.
                try
                {
                    /*
                     * TODO : Test & Release.
                    // Remove the waiter from the table.
                    dbConn.DeleteSess.CommandText = "DELETE FROM requests WHERE id = @id";
                    // Create the Params.
                    SqlParameter idParam = new SqlParameter("@id", waiter.ID);
                    // Bind The Params.
                    dbConn.DeleteSess.Parameters.Add(idParam);
                    // Prepare the Statement.
                    dbConn.DeleteSess.Prepare();
                    // Execute the Statement.
                    dbConn.DeleteSess.ExecuteNonQuery();
                    */
                }
                catch (Exception e)
                {
                    // There was an Error.
                }
            }
            catch (Exception e)
            {
                // There was an Error.
            }
        }

        // Give count of the number of requests running currently.
        internal static int RunningRequests()
        {
            try
            {
                // Here we will return the Waiter Count.
                return waiters.Count;
            }
            catch (Exception e)
            {
                // There was an Error.
                return 0;
            }
        }

        // Lets Get the Running Requests of the Server.
        internal static Dictionary<string, Waiter> GetRunningRequests()
        {
            return waiters;
        }

        // Lets flush the database.
        private static void FlushDB()
        {
            try
            {
                /*
                 * TODO : Test & Release.
                // Lets flush the database here.
                dbConn.TruncateSess.CommandText = "DELETE FROM requests";
                // Execute the Query.
                int truncated = dbConn.TruncateSess.ExecuteNonQuery();
                // The Table was truncated.
                AddStatus("Waiter DB Truncated : " + (truncated > 0 ? "Successful" : "Error"));
                */
            }
            catch (Exception e)
            {
                // There was an Error.
                AddStatus("Flush Err : " + e.Message);
            }
        }

        // Here we set the status and the status trace.
        private static void AddStatus(string statuss)
        {
            try
            {
                // Here we append the status to the status trace and status.
                if (StatusTrace == null)
                {
                    StatusTrace = new StringBuilder();
                }
                Status = statuss;
                StatusTrace.Append(statuss)
                    .Append('\n');
            }
            catch (Exception e)
            {
                // There was an Error.
            }
        }

        // This will retrieve the whole status trace.
        public static string GetStatusTrace()
        {
            return StatusTrace.ToString();
        }

        // This will retrieve just the status of the cloud object.
        public static string GetStatus()
        {
            return Status;
        }

        /*
         * TODO : Test & Release.
        // Lets create the class that would link to the database.
        class DBConnector
        {
            // Variables.
            private SQLiteConnection conn { get; set; }
            internal SQLiteCommand CreateSess { get; set; }
            internal SQLiteCommand SelectSess { get; set; }
            internal SQLiteCommand InsertSess { get; set; }
            internal SQLiteCommand UpdateSess { get; set; }
            internal SQLiteCommand DeleteSess { get; set; }
            internal SQLiteCommand TruncateSess { get; set; }
            public static string DEFAULT_DB = "waiter_";
            internal string status { get; set; }
            internal string query { get; set; }
            internal bool foreignKeyEnabled { get; set; }

            // Default Connstructor.
            // This is the main database.
            // Will Hold all the main company related data.
            // This will not hold data that will be within the company.
            public DBConnector()
            {
                try
                {
                    // Constructor of the Database Helper.
                    conn = new SQLiteConnection("Data Source=" + DEFAULT_DB + ".db");
                    conn.Open();

                    // Create and connect the sql commands
                    CreateSess = new SQLiteCommand(null, conn);
                    InsertSess = new SQLiteCommand(null, conn);
                    SelectSess = new SQLiteCommand(null, conn);
                    UpdateSess = new SQLiteCommand(null, conn);
                    DeleteSess = new SQLiteCommand(null, conn);
                    TruncateSess = new SQLiteCommand(null, conn);

                    try
                    {
                        // Lets Initialize the Foreign Key Support.
                        foreignKeyEnabled = true;
                        // Enable Foreign Key Support for Create Session.
                        CreateSess.CommandText = "PRAGMA foreign_keys = ON;";
                        foreignKeyEnabled = CreateSess.ExecuteNonQuery() > 0;

                        // Enable Foreign Key Support for Insert Session.
                        InsertSess.CommandText = "PRAGMA foreign_keys = ON;";
                        foreignKeyEnabled = InsertSess.ExecuteNonQuery() > 0;

                        // Enable Foreign Key Support for Select Session.
                        SelectSess.CommandText = "PRAGMA foreign_keys = ON;";
                        foreignKeyEnabled = SelectSess.ExecuteNonQuery() > 0;

                        // Enable Foreign Key Support for Update Session.
                        UpdateSess.CommandText = "PRAGMA foreign_keys = ON;";
                        foreignKeyEnabled = UpdateSess.ExecuteNonQuery() > 0;

                        // Enable Foreign Key Support for Delete Session.
                        DeleteSess.CommandText = "PRAGMA foreign_keys = ON;";
                        foreignKeyEnabled = DeleteSess.ExecuteNonQuery() > 0;

                        // Enable Foreign Key Support for Truncate Session.
                        TruncateSess.CommandText = "PRAGMA foreign_keys = ON;";
                        foreignKeyEnabled = TruncateSess.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        // There was an Error.
                        status = "Foreign Key Err : " + e.Message;
                        foreignKeyEnabled = false;
                    }

                    try
                    {
                        // Lets create the required tables.
                        CreateSess.CommandText = "CREATE TABLE IF NOT EXISTS requests ("
                                + "id VARCHAR(12) PRIMARY KEY, "
                                + "url TEXT NOT NULL, "
                                + "endpoint TEXT, "
                                + "method INTEGER NOT NULL DEFAULT 0, "
                                + "data TEXT, "
                                + "time_stamp DATETIME DEFAULT CURRENT_TIMESTAMP)";
                        CreateSess.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        // There was an Error.
                    }
                }
                catch (Exception e)
                {
                    // There was an error in opening connection to database.
                    status = "Error : " + e.Message;
                }
            }

            // Close the Connection to the local database.
            public bool Close()
            {
                try
                {
                    // Here we will try to close the connection with the local database.
                    conn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    // There was an error.
                    return false;
                }
            }

        }*/


            }



    class Helper
    {
        // Generate Random Sring of 'x' size.
        public static class StringManu
        {
            public static string Generate(int size)
            {
                try
                {
                    // Here we will generate the random string.
                    Random random = new Random();
                    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890_-";
                    return new string(Enumerable.Repeat(chars, size)
                      .Select(s => s[random.Next(s.Length)]).ToArray());

                }
                catch (Exception e)
                {
                    // There was an Error.
                    return e.StackTrace;
                }
            }
            
        }
    }
}
