﻿using System;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
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
            GetUserInputMenu();
        }

        static void GetUserInputMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nThe CSharp Academy Console Calculator");
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Type 0 - Close Application");
                Console.WriteLine("Type 1 - Create a New Habit");
                Console.WriteLine("Type 2 - Choose an Existing Habit");

                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Console.WriteLine("");
                        CreateHabit();
                        break;
                    case "2":
                        GetUserInput();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid command. Please type a number from 0 to 4.");
                        break;
                }
            }
        }

        static void CreateHabit()
        {
            Console.Clear();
            Console.WriteLine("Enter Habit Name:");
            string habitName = Console.ReadLine();
            string modifiedInput = habitName.Replace(" ", "_").ToLower();
            string quotedHabitName = $"\"{modifiedInput}\"";
            Console.WriteLine($"Habit Name: {quotedHabitName}");
            Console.ReadKey();
            // Console.WriteLine("Enter habit date:");
            // string habitDate = Console.ReadLine();
            // Console.WriteLine("How do you measure your habit:");
            // string habitMeasure = Console.ReadLine();
            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkHabitName = connection.CreateCommand();

                checkHabitName.CommandText = 
                @$"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name={quotedHabitName}";

                long tableCount = (long)checkHabitName.ExecuteScalar();

                if (tableCount > 0)
                {
                    Console.WriteLine("Habit name is already taken.");
                }
                
                else
                {
                    var tableCmd = connection.CreateCommand();
                    
                    tableCmd.CommandText = 
                    @$"CREATE TABLE IF NOT EXISTS {modifiedInput} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                    tableCmd.ExecuteNonQuery();

                    Console.WriteLine("New habit table created.");
                    
                    connection.Close();
                }

            }
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nThe CSharp Academy Console Calculator");
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\n\nWhat would you like to do?");
                Console.WriteLine("Type 0 - Close Application");
                Console.WriteLine("Type 1 - Get All Records");
                Console.WriteLine("Type 2 - Create a Record");
                Console.WriteLine("Type 3 - Delete a Record");
                Console.WriteLine("Type 4 - Update a Record");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid command. Please type a number from 0 to 4.");
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
                    $"SELECT * FROM drinking_water";

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
                        });;
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

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or tpye 0 to go back to Main Menu.\n\n");
            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if(rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    Delete();
                }

                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted.");

                GetUserInput();
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
                $"INSERT INTO drinking_water(date,quantity) VALUES('{date}', '{quantity}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to main menu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
                    connection.Close();
                    Update();
                }

                string date = GetDateInput();

                int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed).\n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = 
                $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
                
                tableCmd.ExecuteNonQuery();

                connection.Close();
                
            }
        }

        internal static string GetDateInput()
        {
           Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
           
           string dateInput = Console.ReadLine();

           if (dateInput == "0") GetUserInput();
           
           while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
           {
                Console.WriteLine("\n\nInvalid date. (Format: dd-MM-yy. Type 0 to return to main menu or try again:\n\n)");
                dateInput = Console.ReadLine();
           }
           return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if(numberInput == "0") GetUserInput();

            
            while(!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("Invalid number. Try again.\n\n");
                numberInput = Console.ReadLine();
            }
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