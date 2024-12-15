# Intelligent Personal Finance Manager 📊💰

**Intelligent Personal Finance Manager** is a user-friendly application I developed to help individuals efficiently manage and track their personal finances. It offers robust features for categorizing, recording, and analyzing expenses, empowering users to make smarter financial decisions.

## Features

- **Record Expenses**: Effortlessly add daily expenses with details such as category, amount, date, and description.
- **Categorize Expenses**: Organize expenses into categories to gain insights into spending habits.
- **Visualize Data**: Interactive charts and graphs make it easy to understand spending trends.
- **Flexible Filtering**: Filter expenses by date range to analyze specific periods.
- **Database Integration**: Ensures all financial data is securely stored and retrieved using a database.

## Getting Started

### Prerequisites
- Windows OS
- Visual Studio (with .NET Framework support)
- SQL Server / SQL Server Management Studio (SSMS)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-username/Intelligent-Personal-Finance-Manager.git
   ```

2. **Set up the Database**:
   - Open SQL Server Management Studio (SSMS).
   - Run the SQL scripts available in the [`DatabaseScripts`](DatabaseScripts/create_database.sql) folder to set up the database schema.

3. **Build and Run**:
   - Open `IntelligentPersonalFinanceManager.sln` in Visual Studio.
   - Build the solution and run the application. 🚀

### Usage
- **CRUD**: complete crud operations.
- **Add Expense**: Use the "Add Expense" button to record a new expense.
- **Edit/Delete Expense**: Modify or delete entries directly from the expense list.
- **Analyze Data**: Utilize the pie chart and histogram features to visualize expense trends.


## Technologies Used

- **C#** / **.NET Framework**: Core application logic
- **WPF (Windows Presentation Foundation)**: For creating a responsive and user-friendly UI
- **Entity Framework (EF)**: For managing database operations
- **SQL Server**: Backend database
- **Git/GitHub**: Version control and collaboration

