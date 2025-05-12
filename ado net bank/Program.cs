using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BankingTransactionSystem
{
    class Program
    {
        static string connectionString = "Server=NITEESHNSVN\\SQLEXPRESS;Database=Niteesh;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        static async Task Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Banking Transaction Menu =====");
                Console.WriteLine("1. Transfer Money");
                Console.WriteLine("2. View Account Balances");
                Console.WriteLine("3. View Transaction History");
                Console.WriteLine("4. Register Account");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        await TransferMoneyAsync();
                        break;
                    case "2":
                        await ViewBalancesAsync();
                        break;
                    case "3":
                        await ViewTransactionHistoryAsync();
                        break;
                    case "4":
                        await RegisterAccountAsync();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Exiting program...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static async Task RegisterAccountAsync()
        {
            Console.WriteLine("\n--- Register Account ---");
            Console.WriteLine("1. Register Savings Account");
            Console.WriteLine("2. Register Checking Account");
            Console.Write("Select account type: ");
            string option = Console.ReadLine();

            Console.Write("Enter Account Number: ");
            string accountNumber = Console.ReadLine();

            Console.Write("Enter Holder Name: ");
            string holderName = Console.ReadLine();

            Console.Write("Enter Initial Balance: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal balance) || balance < 0)
            {
                Console.WriteLine("Invalid balance.");
                return;
            }

            string insertQuery = option == "1"
                ? "INSERT INTO SavingsAccount (AccountNumber, HolderName, Balance) VALUES (@AccountNumber, @HolderName, @Balance)"
                : "INSERT INTO CheckingAccount (AccountNumber, HolderName, Balance) VALUES (@AccountNumber, @HolderName, @Balance)";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
                using SqlCommand cmd = new SqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@HolderName", holderName);
                cmd.Parameters.AddWithValue("@Balance", balance);

                int rows = await cmd.ExecuteNonQueryAsync();
                if (rows > 0)
                {
                    Console.WriteLine("Account registered successfully.");
                }
                else
                {
                    Console.WriteLine("Account registration failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error registering account: " + ex.Message);
            }
        }

        static async Task TransferMoneyAsync()
        {
            Console.Write("Enter Savings Account Number: ");
            string savingsAccountNumber = Console.ReadLine();

            Console.Write("Enter Checking Account Number: ");
            string checkingAccountNumber = Console.ReadLine();

            Console.Write("Enter Amount to Transfer: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal transferAmount) || transferAmount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            using SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string deductQuery = @"UPDATE SavingsAccount 
                                           SET Balance = Balance - @Amount 
                                           WHERE AccountNumber = @AccountNumber AND Balance >= @Amount";

                    using SqlCommand deductCommand = new SqlCommand(deductQuery, connection, transaction);
                    deductCommand.Parameters.AddWithValue("@Amount", transferAmount);
                    deductCommand.Parameters.AddWithValue("@AccountNumber", savingsAccountNumber);
                    int rowsAffected1 = await deductCommand.ExecuteNonQueryAsync();

                    string addQuery = @"UPDATE CheckingAccount 
                                        SET Balance = Balance + @Amount 
                                        WHERE AccountNumber = @AccountNumber";

                    using SqlCommand addCommand = new SqlCommand(addQuery, connection, transaction);
                    addCommand.Parameters.AddWithValue("@Amount", transferAmount);
                    addCommand.Parameters.AddWithValue("@AccountNumber", checkingAccountNumber);
                    int rowsAffected2 = await addCommand.ExecuteNonQueryAsync();

                    if (rowsAffected1 == 1 && rowsAffected2 == 1)
                    {
                        string historyQuery = @"INSERT INTO TransactionHistory 
                                                (FromAccount, ToAccount, Amount, TransferDateTime)
                                                VALUES (@From, @To, @Amount, @Time)";

                        using SqlCommand historyCmd = new SqlCommand(historyQuery, connection, transaction);
                        historyCmd.Parameters.AddWithValue("@From", savingsAccountNumber);
                        historyCmd.Parameters.AddWithValue("@To", checkingAccountNumber);
                        historyCmd.Parameters.AddWithValue("@Amount", transferAmount);
                        historyCmd.Parameters.AddWithValue("@Time", DateTime.Now);

                        await historyCmd.ExecuteNonQueryAsync();

                        await transaction.CommitAsync();
                        Console.WriteLine("Transaction completed and logged successfully.");
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine("Transaction failed. Account issue or insufficient funds.");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Transaction error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: " + ex.Message);
            }
        }

        static async Task ViewBalancesAsync()
        {
            using SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();

                Console.WriteLine("\n--- Savings Accounts ---");
                Console.WriteLine("{0,-20} {1,-20} {2,10}", "Account Number", "Holder Name", "Balance");
                Console.WriteLine(new string('-', 55));

                using SqlCommand cmd1 = new SqlCommand("SELECT * FROM SavingsAccount", connection);
                using SqlDataReader reader1 = await cmd1.ExecuteReaderAsync();
                while (await reader1.ReadAsync())
                {
                    Console.WriteLine("{0,-20} {1,-20} {2,10:F2}", reader1["AccountNumber"], reader1["HolderName"], reader1["Balance"]);
                }
                reader1.Close();

                Console.WriteLine("\n--- Checking Accounts ---");
                Console.WriteLine("{0,-20} {1,-20} {2,10}", "Account Number", "Holder Name", "Balance");
                Console.WriteLine(new string('-', 55));

                using SqlCommand cmd2 = new SqlCommand("SELECT * FROM CheckingAccount", connection);
                using SqlDataReader reader2 = await cmd2.ExecuteReaderAsync();
                while (await reader2.ReadAsync())
                {
                    Console.WriteLine("{0,-20} {1,-20} {2,10:F2}", reader2["AccountNumber"], reader2["HolderName"], reader2["Balance"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving balances: " + ex.Message);
            }
        }

        static async Task ViewTransactionHistoryAsync()
        {
            using SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();

                Console.WriteLine("\n--- Transaction History ---");
                Console.WriteLine("{0,-20} {1,-20} {2,10} {3,25}", "From Account", "To Account", "Amount", "Date & Time");
                Console.WriteLine(new string('-', 80));

                using SqlCommand cmd = new SqlCommand("SELECT * FROM TransactionHistory ORDER BY TransferDateTime DESC", connection);
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine("{0,-20} {1,-20} {2,10:F2} {3,25}", reader["FromAccount"], reader["ToAccount"], reader["Amount"], Convert.ToDateTime(reader["TransferDateTime"]).ToString("g"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching transaction history: " + ex.Message);
            }
        }
    }
}
