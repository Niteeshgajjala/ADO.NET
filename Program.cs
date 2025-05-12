using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "Server=NITEESHNSVN\\SQLEXPRESS;Database=Niteesh;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n===== Employee Data Fetch Menu =====");
            Console.WriteLine("1. Asynchronous Execution");
            Console.WriteLine("2. Synchronous Execution");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1-3): ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n=== Asynchronous Execution ===");
                    Stopwatch swAsync = Stopwatch.StartNew();
                    await FetchRecentEmployeesAsync(connectionString);
                    swAsync.Stop();
                    Console.WriteLine($"\nAsync execution time: {swAsync.ElapsedMilliseconds} ms\n");
                    break;

                case "2":
                    Console.WriteLine("\n=== Synchronous Execution ===");
                    Stopwatch swSync = Stopwatch.StartNew();
                    FetchRecentEmployeesSync(connectionString);
                    swSync.Stop();
                    Console.WriteLine($"\nSync execution time: {swSync.ElapsedMilliseconds} ms\n");
                    break;

                case "3":
                    Console.WriteLine("Exiting the program. Thank you!");
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
                    break;
            }
        }
    }

    static async Task FetchRecentEmployeesAsync(string connStr)
    {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("GetRecentEmployees", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    // Table header
                    Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15}", "ID", "Name", "Department", "Date Of Joining");

                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string dept = reader.GetString(2);
                        DateTime doj = reader.GetDateTime(3);

                        // Row
                        Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15}",
                            id, name, dept, doj.ToString("dd-MM-yyyy"));
                    }
                }
            }
        }
    }

    static void FetchRecentEmployeesSync(string connStr)
    {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetRecentEmployees", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Table header
                    Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15}", "ID", "Name", "Department", "Date Of Joining");

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string dept = reader.GetString(2);
                        DateTime doj = reader.GetDateTime(3);

                        // Row
                        Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15}",
                            id, name, dept, doj.ToString("dd-MM-yyyy"));
                    }
                }
            }
        }
    }
}
