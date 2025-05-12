using System;
using System.Data.SqlClient;

namespace EmployeeManagementSystem
{
    class Program
    {
        static string connectionString = "Server=NITEESHNSVN\\SQLEXPRESS;Database=Niteesh;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== Employee Management System ===");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. View All Employees");
                Console.WriteLine("3. Update Employee");
                Console.WriteLine("4. Delete Employee");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddEmployee();
                            break;
                        case "2":
                            ViewEmployees();
                            break;
                        case "3":
                            UpdateEmployee();
                            break;
                        case "4":
                            DeleteEmployee();
                            break;
                        case "5":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void AddEmployee()
        {
            try
            {
                Console.Write("Enter ID: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter Email: ");
                string email = Console.ReadLine();
                Console.Write("Enter Department: ");
                string department = Console.ReadLine();
                Console.Write("Enter Hire Date (yyyy-mm-dd): ");
                DateTime hireDate = DateTime.Parse(Console.ReadLine());
                Console.Write("Enter Salary: ");
                decimal salary = decimal.Parse(Console.ReadLine());

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO E_mployees VALUES (@ID, @Name, @Email, @Department, @HireDate, @Salary)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Department", department);
                    cmd.Parameters.AddWithValue("@HireDate", hireDate);
                    cmd.Parameters.AddWithValue("@Salary", salary);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Employee added successfully!");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter correct data types.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void ViewEmployees()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM E_mployees";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("\n--- Employee Records ---\n");
                    Console.WriteLine(
                        "{0,-10} | {1,-20} | {2,-30} | {3,-20} | {4,-12} | {5,10}",
                        "ID", "Name", "Email", "Department", "Hire Date", "Salary"
                    );
                    Console.WriteLine(new string('-', 120));

                    while (reader.Read())
                    {
                        Console.WriteLine(
                            "{0,-10} | {1,-20} | {2,-30} | {3,-20} | {4,-12:yyyy-MM-dd} | {5,10:F2}",
                            reader["EmployeeID"],
                            reader["Name"],
                            reader["Email"],
                            reader["Department"],
                            reader["HireDate"],
                            reader["Salary"]
                        );
                    }

                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }


        static void UpdateEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID to update: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter New Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter New Email: ");
                string email = Console.ReadLine();
                Console.Write("Enter New Department: ");
                string department = Console.ReadLine();
                Console.Write("Enter New Hire Date (yyyy-mm-dd): ");
                DateTime hireDate = DateTime.Parse(Console.ReadLine());
                Console.Write("Enter New Salary: ");
                decimal salary = decimal.Parse(Console.ReadLine());

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE E_mployees 
                                     SET Name=@Name, Email=@Email, Department=@Department, HireDate=@HireDate, Salary=@Salary 
                                     WHERE EmployeeID=@ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Department", department);
                    cmd.Parameters.AddWithValue("@HireDate", hireDate);
                    cmd.Parameters.AddWithValue("@Salary", salary);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Employee updated successfully!" : "Employee not found.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter correct data types.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void DeleteEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID to delete: ");
                int id = int.Parse(Console.ReadLine());

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM E_mployees WHERE EmployeeID=@ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Employee deleted successfully!" : "Employee not found.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a numeric ID.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
