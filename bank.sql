CREATE TABLE SavingsAccount (
    AccountNumber NVARCHAR(20) PRIMARY KEY,
    HolderName NVARCHAR(100) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL CHECK (Balance >= 0)
);

CREATE TABLE CheckingAccount (
    AccountNumber NVARCHAR(20) PRIMARY KEY,
    HolderName NVARCHAR(100) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL CHECK (Balance >= 0)
);


INSERT INTO SavingsAccount (AccountNumber, HolderName, Balance)
VALUES 
('SAV1001', 'Niteesh', 10000.00),
('SAV1002', 'Dheeraj', 5000.00);


INSERT INTO CheckingAccount (AccountNumber, HolderName, Balance)
VALUES 
('CHK2001', 'Harshith', 2000.00),
('CHK2002', 'Shanmukh', 3000.00);

CREATE INDEX idx_savings_account ON SavingsAccount (AccountNumber);
CREATE INDEX idx_checking_account ON CheckingAccount (AccountNumber);

CREATE TABLE TransactionHistory (
    TransactionId INT IDENTITY(1,1) PRIMARY KEY,
    FromAccount VARCHAR(20),
    ToAccount VARCHAR(20),
    Amount DECIMAL(18,2),
    TransferDateTime DATETIME
);

