using System;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

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

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        break;
                    case "1":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        break;
                    case "2":
                        Insert();
                        closeApp = true;
                        break;
                    case "3":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        break;
                }
            }
        }

        private static void Insert()
        {
            string date = GetDateInput();
        }

        internal static string GetDateInput()
        {
           Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
           
           string dateInput = Console.ReadLine();

           if (dateInput == "0") GetUserInput();
           
           return dateInput;
        }
    }
 }