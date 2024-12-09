-- Create a PersonalExpenses database
CREATE DATABASE PersonalExpenses;
GO

-- Using the created database
USE PersonalExpenses;
GO

-- Creating the Categories table
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL
);
GO

-- Create Expenses table
CREATE TABLE Expenses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CategoryId INT,
    Amount DECIMAL(18,2) NOT NULL,
    Date DATE NOT NULL,
    Description NVARCHAR(MAX),
    CONSTRAINT FK_Expenses_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO

-- Inserting initial data into the Categories table
INSERT INTO Categories(Name)
VALUES 
  ('Clothing'),
  ('Debt Repayment'),
  ('Dining Out'),
  ('Education'),
  ('Entertainment'),
  ('Gifts & Donations'),
  ('Groceries'),
  ('Health'),
  ('Household Supplies'),
  ('Insurance'),
  ('Investments'),
  ('Other'),
  ('Personal Care'),
  ('Rent'),
  ('Savings'),
  ('Subscriptions'),
  ('Transportation'),
  ('Travel'),
  ('Utilities');
GO

-- Adding a CategoryId column to the Expenses table if it doesn't already exist
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Expenses' AND COLUMN_NAME = 'CategoryId')
BEGIN
    ALTER TABLE Expenses
    ADD CategoryId INT;
END
GO

-- Update CategoryId based on existing data in Category
UPDATE Expenses
SET CategoryId = c.Id
FROM Expenses e
JOIN Categories c ON e.CategoryId = c.Id;
GO

-- Removing the Category column from the Expenses table if it is not already removed
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Expenses' AND COLUMN_NAME = 'Category')
BEGIN
    ALTER TABLE Expenses
    DROP COLUMN Category;
END
GO
