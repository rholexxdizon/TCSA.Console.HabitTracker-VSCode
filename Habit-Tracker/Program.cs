using System;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

namespace Habit_Tracker
 {
    class Program
    {
        static string connectionString = @"Data Source=habit-tracker.db";
        static void Main(string[] args)
        {

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

            GetUserInput();
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
                        GetAllRecords();
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

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = 
                    $"SELECT * FROM drinking_water ";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)                
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();

                Console.WriteLine("-------------------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("-------------------------------------------------\n");
            }
        }

        private static void Insert()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed).\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = 
                $"INSERT INTO drinking_water(date,quanitity) VALUES('{date}, {quantity}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static string GetDateInput()
        {
           Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
           
           string dateInput = Console.ReadLine();

           if (dateInput == "0") GetUserInput();
           
           return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if(numberInput == "0") GetUserInput();

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }

        public class DrinkingWater
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int Quantity { get; set; }
        }
    }
 }