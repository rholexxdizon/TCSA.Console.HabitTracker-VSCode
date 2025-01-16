using System;
using Microsoft.Data.Sqlite;

 namespace Habit_Tracker
 {
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=habit-tracker.db";

            using (var connection = new SqliteConnection(connectionString)){
                connection.Open();
                var tableCmd = connection.CreateCommand();
                
                tableCmd.CommandText = 
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quanitity INTEGER
                    )";

                tableCmd.ExecuteNonQuery();
                
                connection.Close();
            }
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\n\nWhat would you like to do?");
                Console.WriteLine("\n\nType 0 to Close Application");
                Console.WriteLine("\n\nType 1 to View All Records.");
                Console.WriteLine("\n\nType 2 to Insert Record.");
                Console.WriteLine("\n\nType 3 to Delete Record.");
                Console.WriteLine("\n\nType 4 to Update Record.");
                Console.WriteLine("-------------------------------------------------------------\n");
            }
        }
    }

 }