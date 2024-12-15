using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace ExpenseTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set the trace level to only show error messages in the data binding
            PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Error;

            try
            {
                // Update the expense data grid with current data
                UpdateExpensesDataGrid();
            }
            catch (Exception ex)
            {
                // Display an error message if something goes wrong
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Method to update the expenses data grid
        public void UpdateExpensesDataGrid()
        {
            using (var context = new ExpenseTrackerContext())
            {
                // Fetch expenses including their associated categories
                var expenses = context.Expenses.Include("Category").ToList();
                // Set the data source for the data grid
                expensesDataGrid.ItemsSource = expenses;
                // Sort the items by the 'Date' property in descending order
                expensesDataGrid.Items.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // This method can be used for any actions after the content is rendered
        }

        // Event handler for the 'Add Expense' button click
        private void addExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Add Expense window
            AddExpenseWindow addExpenseWindow = new AddExpenseWindow();
            addExpenseWindow.Owner = this;
            addExpenseWindow.Show();
        }

        // Event handler for the 'Edit' button click
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item from the data grid
            var selectedItem = expensesDataGrid.SelectedItem;

            // Check if the selected item is an 'Expense' object
            if (selectedItem is Expense selectedExpense)
            {
                // Open the Edit Expense window and pass the selected expense to it
                EditExpenseWindow editWindow = new EditExpenseWindow(selectedExpense);
                editWindow.Owner = this;
                // If the user confirms the changes, update the data grid
                if (editWindow.ShowDialog() == true)
                {
                    UpdateExpensesDataGrid();
                }
            }
            else
            {
                // Show a message if no expense is selected
                MessageBox.Show("Select an expense to edit!");
            }
        }

        // Event handler for the 'Delete' button click
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item from the data grid
            var selectedItem = expensesDataGrid.SelectedItem;

            // Check if the selected item is an 'Expense' object
            if (selectedItem is Expense selectedExpense)
            {
                // Ask for confirmation before deleting the expense
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this expense:\nCategory: {selectedExpense.Category.Name}\nAmount: {selectedExpense.Amount}\nDate: {selectedExpense.Date:dd.MM.yyyy}?", "Delete confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // If confirmed, delete the expense from the database
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new ExpenseTrackerContext())
                    {
                        context.Expenses.Attach(selectedExpense);
                        context.Expenses.Remove(selectedExpense);
                        context.SaveChanges();
                    }

                    // Update the data grid after deletion
                    UpdateExpensesDataGrid();
                }
            }
            else
            {
                // Show a message if no expense is selected
                MessageBox.Show("Select an expense to delete!");
            }
        }

        // Event handler for the 'Filter' button click
        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected start and end dates for filtering
            DateTime? startDate = startDatePicker.SelectedDate;
            DateTime? endDate = endDatePicker.SelectedDate;

            using (var context = new ExpenseTrackerContext())
            {
                // Filter the expenses based on the selected date range
                var expenses = context.Expenses.Include("Category")
                    .Where(expense =>
                        (startDate.HasValue && endDate.HasValue && expense.Date >= startDate.Value && expense.Date <= endDate.Value) ||
                        (startDate.HasValue && !endDate.HasValue && expense.Date == startDate.Value) ||
                        (!startDate.HasValue && endDate.HasValue && expense.Date == endDate.Value))
                    .ToList();

                // Update the data grid with the filtered expenses
                expensesDataGrid.ItemsSource = expenses;
            }
        }

        // Event handler for the 'Show Pie Chart' button click
        private void showPieChartButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Pie Chart window
            PieChartWindow pieChartWindow = new PieChartWindow();
            pieChartWindow.Show();
        }

        // Event handler for the 'Show Histogram' button click
        private void showHistogramButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected start and end dates for filtering
            DateTime startDate = startDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = endDatePicker.SelectedDate ?? DateTime.MaxValue;

            // Create and display the Histogram window with the selected date range
            HistogramWindow histogramWindow = new HistogramWindow(startDate, endDate);
            histogramWindow.Show();
        }
    }
}
