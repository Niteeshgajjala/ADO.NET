CREATE TABLE Employes (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Department NVARCHAR(50) NOT NULL,
    DateOfJoining DATE NOT NULL
);


INSERT INTO Employes (Name, Department, DateOfJoining) VALUES
('Niteesh', 'IT', '2025-04-15'),
('Dheeraj', 'Finance', '2025-03-20'),
('Shanmukh', 'HR', '2025-02-28'),
('Jai Vardhan', 'Marketing', '2025-01-10'),
('Manohar', 'Operations', '2023-12-25'),
('Nageshwar Rao', 'IT', '2025-03-01'),
('Harshith', 'Finance', '2025-04-05'),
('Biswanth', 'HR', '2023-05-01'),
('Vamsi Krishna', 'Operations', '2024-09-10'),
('Teja', 'HR', '2022-01-22');


CREATE PROCEDURE GetRecentEmployees
AS
BEGIN
    SELECT EmployeeID, Name, Department, DateOfJoining
    FROM Employes
    WHERE DateOfJoining >= DATEADD(MONTH, -6, GETDATE())
END




