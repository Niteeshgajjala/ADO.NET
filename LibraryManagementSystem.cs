using System.Data.SqlClient;
using System;

namespace ado.net_libary
{
    internal class Program
    {
        static string connServer = "Server=NITEESHNSVN\\SQLEXPRESS;Database=Niteesh;Encrypt=True;Integrated Security=True;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Library Management System");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Update Book");
                Console.WriteLine("3. Delete Book");
                Console.WriteLine("4. Show All Books");
                Console.WriteLine("5. Register Student");
                Console.WriteLine("6. Borrow Book");
                Console.WriteLine("7. Return Book");
                Console.WriteLine("8. Show Borrowed Books");
                Console.WriteLine("0. Exit");
                choice = Validation.GetInt("Enter your choice: ");

                switch (choice)
                {
                    case 1:
                        Console.Write("Book Title: ");
                        string title = Console.ReadLine();
                        Console.Write("Author: ");
                        string author = Console.ReadLine();
                        int yearPublished = Validation.GetInt("Year Published: ");
                        AddBook(title, author, yearPublished);
                        break;
                    case 2:
                        int updateId = Validation.GetInt("Book ID to update: ");
                        Console.Write("New Title: ");
                        string newTitle = Console.ReadLine();
                        Console.Write("New Author: ");
                        string newAuthor = Console.ReadLine();
                        int newYear = Validation.GetInt("New Year Published: ");
                        UpdateBook(updateId, newTitle, newAuthor, newYear);
                        break;
                    case 3:
                        int deleteId = Validation.GetInt("Book ID to delete: ");
                        DeleteBook(deleteId);
                        break;
                    case 4:
                        ShowBooks();
                        break;
                    case 5:
                        Console.Write("Enter Student Name: ");
                        string studentName = Console.ReadLine();
                        Console.Write("Enter Student Email: ");
                        string studentEmail = Console.ReadLine();
                        RegisterStudent(studentName, studentEmail);
                        break;
                    case 6:
                        int borrowBookId = Validation.GetInt("Enter Book ID to borrow: ");
                        int studentId = Validation.GetInt("Enter Student ID: ");
                        DateTime borrowDate = DateTime.Today;
                        int days = Validation.GetInt("For how many days?: ");
                        DateTime returnDate = borrowDate.AddDays(days);
                        BorrowBook(borrowBookId, studentId, borrowDate, returnDate);
                        break;
                    case 7:
                        int returnId = Validation.GetInt("Enter Borrow Record ID to return: ");
                        ReturnBook(returnId);
                        break;
                    case 8:
                        ShowBorrowedBooks();
                        break;
                    case 0:
                        Console.WriteLine("Thank you!");
                        break;
                    default:
                        Console.WriteLine(" Invalid choice!");
                        break;
                }
                if (choice != 0)
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            } while (choice != 0);
        }

        static void AddBook(string title, string author, int yearPublished)
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("EXEC AddBook @Title, @Author, @YearPublished", conn);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@YearPublished", yearPublished);

                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? " Book added successfully!" : " Failed to add book.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❗ Error: " + ex.Message);
                }
            }
        }

        static void UpdateBook(int id, string title, string author, int yearPublished)
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("EXEC UpdateBook @BookId, @Title, @Author, @YearPublished", conn);
                    cmd.Parameters.AddWithValue("@BookId", id);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@YearPublished", yearPublished);

                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? " Book updated successfully!" : " Book not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❗ Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        static void DeleteBook(int id)
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("EXEC DeleteBook @BookId", conn);
                    cmd.Parameters.AddWithValue("@BookId", id);

                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? " Book deleted successfully!" : " Book not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❗ Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        static void ShowBooks()
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("EXEC GetAllBooks", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("\n{0,-5} {1,-30} {2,-20} {3,10}", "ID", "Title", "Author", "Year Published");
                    Console.WriteLine(new string('-', 70));

                    while (reader.Read())
                    {
                        Console.WriteLine("{0,-5} {1,-30} {2,-20} {3,10}",
                            reader["BookId"], reader["Title"], reader["Author"], reader["YearPublished"]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❗ Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        static void RegisterStudent(string name, string email)
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EXEC AddStudent @Name, @Email", conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "✅ Student registered!" : "❌ Failed to register.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❗ Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        static void BorrowBook(int bookId, int studentId, DateTime borrowDate, DateTime returnDate)
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EXEC BorrowBook @BookId, @StudentId, @BorrowDate, @ReturnDate", conn);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@BorrowDate", borrowDate);
                    cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? " Book borrowed!" : " Failed to borrow.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❗ Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        static void ReturnBook(int borrowId)
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EXEC ReturnBook @BorrowId", conn);
                    cmd.Parameters.AddWithValue("@BorrowId", borrowId);
                    int result = cmd.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Book returned!" : " Return failed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❗ Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        static void ShowBorrowedBooks()
        {
            using (SqlConnection conn = new SqlConnection(connServer))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EXEC GetAllBorrowedBooks", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Console.WriteLine("📋 Borrowed Books:");
                    Console.WriteLine($"{"ID",-5}{"Title",-30}{"Student",-20}{"Borrowed",-12}{"Due",-12}{"Returned"}");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Id"],-5}{reader["Title"],-30}{reader["StudentName"],-20}" +
                            $"{Convert.ToDateTime(reader["BorrowDate"]).ToShortDateString(),-12}" +
                            $"{Convert.ToDateTime(reader["ReturnDate"]).ToShortDateString(),-12}" +
                            $"{(Convert.ToBoolean(reader["Returned"]) ? "✅" : "❌")}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
