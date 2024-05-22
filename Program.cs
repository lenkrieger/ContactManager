using System;
using System.Data.SQLite;

namespace ContactManager
{
    internal class Program
    {
        private static string connectionString = "Data Source=contacts.db;Version=3;";

        private static void Main(string[] args)
        {
            CreateDatabase();

            while (true) // loop menu
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("1. View all contacts");
                Console.WriteLine("2. Search contact");
                Console.WriteLine("3. Add contact");
                Console.WriteLine("4. Edit contact");
                Console.WriteLine("5. Delete contact");
                Console.WriteLine("6. Exit");
                var choice = Console.ReadLine(); 
                switch (choice)
                {
                    case "1":
                        ViewAllContacts();
                        break;

                    case "2":
                        SearchContact();
                        break;

                    case "3":
                        AddContact();
                        break;

                    case "4":
                        EditContact();
                        break;

                    case "5":
                        DeleteContact();
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString)) // Create database and Contacts table if not exists
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS Contacts (Id INTEGER PRIMARY KEY, FirstName TEXT, LastName TEXT, PhoneNumber TEXT, Email TEXT)"; // creating table with 4 columns
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void AddContact()
        {
            // Get contact details from user
            Console.WriteLine("----------------------");
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            
            using (var connection = new SQLiteConnection(connectionString)) // Insert new contact into database
            {
                connection.Open(); //  open the connection to the SQLite database

                string sql = "INSERT INTO Contacts (FirstName, LastName, PhoneNumber, Email) VALUES (@FirstName, @LastName, @PhoneNumber, @Email)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName); 
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@Email", email);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Contact added.");
        }

        private static void EditContact()
        {
            Console.WriteLine("----------------------");
            ViewAllContacts();
            Console.Write("Enter contact ID to edit: "); // Get contact ID and new details from user
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter new first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter new last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter new phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter new email: ");
            string email = Console.ReadLine();

            // Update contact in database by changing already writen fields in Table with new one
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email WHERE Id = @Id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@Email", email);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Contact updated.");
        }

        private static void DeleteContact()
        {
            Console.WriteLine("----------------------");
            Console.Write("Enter contact ID to delete: "); // Get contact ID to delete from user
            int id = int.Parse(Console.ReadLine());

            // Delete contact from database
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Contacts WHERE Id = @Id"; // deleting contact @Id correspond with imputed by user 
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Contact deleted.");
        }

        private static void SearchContact()
        {
            Console.WriteLine("----------------------");
            Console.Write("Enter first name or last name to search: "); // Get search data from user
            string searchTerm = Console.ReadLine();

            // Search contacts in database
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Contacts WHERE FirstName LIKE @SearchTerm OR LastName LIKE @SearchTerm";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, First Name: {reader["FirstName"]}, Last Name: {reader["LastName"]}, Phone: {reader["PhoneNumber"]}, Email: {reader["Email"]}");
                        }
                    }
                }
            }
        }

        private static void ViewAllContacts()
        {
            Console.WriteLine("----------------------");
            using (var connection = new SQLiteConnection(connectionString)) // View all contacts in database
            {
                connection.Open();

                var sql = "SELECT * FROM Contacts";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            Console.WriteLine(
                                $"ID: {reader["Id"]}, First Name: {reader["FirstName"]}, Last Name: {reader["LastName"]}, Phone: {reader["PhoneNumber"]}, Email: {reader["Email"]}");
                    }
                }
            }
        }
    }
}