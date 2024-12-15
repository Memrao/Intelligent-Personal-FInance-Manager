using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System;
using System.Linq;
using System.Windows;
using Microsoft.VisualBasic;

namespace ExpenseTracker.Windows
{
    public partial class AddExpenseWindow : Window
    {
        public AddExpenseWindow()
        {
            InitializeComponent();

            using (var context = new ExpenseTrackerContext())
            {
                // Завантажуємо всі категорії, крім "Other"
                var categories = context.Categories
                    .Where(c => c.Name != "Other")
                    .OrderBy(c => c.Name)
                    .Select(c => c.Name)
                    .ToList();

                // Додаємо категорію "Other" в кінець списку
                categories.Add("Other");

                // Встановлюємо список категорій як джерело даних для ComboBox
                categoryComboBox.ItemsSource = categories;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримати дані з текстових полів
            string categoryName = categoryComboBox.SelectedItem as string; // Отримуємо обрану категорію з ComboBox

            decimal amount;
            if (!decimal.TryParse(amountTextBox.Text, out amount))
            {
                MessageBox.Show("Invalid amount format!");
                return;
            }
            DateTime date = datePicker.SelectedDate ?? DateTime.Now;
            string description = descriptionTextBox.Text;

            using (var context = new ExpenseTrackerContext())
            {
                Category selectedCategory = null;

                // Обробка нової категорії
                if (categoryName == "Other")
                {
                    // Отримуємо нове ім'я категорії
                    string newCategoryName = Interaction.InputBox("Enter the name of the new category:", "New Category");

                    // Перевірка на пусту назву категорії
                    if (string.IsNullOrEmpty(newCategoryName))
                    {
                        MessageBox.Show("Enter the name of the new category!");
                        return;
                    }

                    // Перевірка, чи така категорія вже існує
                    selectedCategory = context.Categories.FirstOrDefault(c => c.Name == newCategoryName);

                    if (selectedCategory == null)
                    {
                        // Добавление новой категории в базу данных
                        selectedCategory = new Category { Name = newCategoryName };
                        context.Categories.Add(selectedCategory);
                        context.SaveChanges();

                        // Обновление списка категорий в ItemsSource
                        var categories = context.Categories.ToList(); // Получаем обновленный список категорий
                        categoryComboBox.ItemsSource = categories; // Обновляем ItemsSource

                        // Выбираем новую категорию в ComboBox
                        categoryComboBox.SelectedItem = selectedCategory;
                    }

                }
                else
                {
                    // Используем существующую категорию
                    selectedCategory = context.Categories.FirstOrDefault(c => c.Name == categoryName);
                }

                // Перевірка, чи обрано категорію
                if (selectedCategory == null)
                {
                    MessageBox.Show("Select a category!");
                    return;
                }

                // Створити новий об'єкт Expense
                Expense newExpense = new Expense
                {
                    CategoryId = selectedCategory.Id, // Отримуємо Id обраної категорії
                    Amount = amount,
                    Date = date,
                    Description = description
                };

                // Зберегти витрату в базі даних
                context.Expenses.Add(newExpense);
                context.SaveChanges();
            }

            // Оновити DataGrid в MainWindow
            ((MainWindow)Owner).UpdateExpensesDataGrid();

            // Повідомити про успішне збереження
            MessageBox.Show("Data saved!");

            // Очистити поля форми
            categoryComboBox.SelectedItem = null; // Очищаємо вибір в ComboBox
            amountTextBox.Text = "";
            datePicker.SelectedDate = null;
            descriptionTextBox.Text = "";
        }
    }
}
