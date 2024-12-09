using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System;
using System.Linq;
using System.Windows;

namespace ExpenseTracker.Windows
{
    /// <summary>
    /// Interaction logic for EditExpenseWindow.xaml
    /// </summary>
    public partial class EditExpenseWindow : Window
    {
        private Expense _expense; // Змінна для зберігання витрати, яку редагуємо

        public EditExpenseWindow(Expense expense)
        {
            InitializeComponent();
            _expense = expense;

            // Завантаження категорій з бази даних
            using (var context = new ExpenseTrackerContext())
            {
                // Завантажуємо всі категорії, крім "Other"
                var categories = context.Categories
                    .Where(c => c.Name != "Other")
                    .OrderBy(c => c.Name)
                    .ToList();

                // Додаємо категорію "Other" в кінець списку
                var otherCategory = context.Categories.FirstOrDefault(c => c.Name == "Other");
                if (otherCategory == null)
                {
                    otherCategory = new Category { Name = "Other" };
                    context.Categories.Add(otherCategory);
                    context.SaveChanges();
                }
                categories.Add(otherCategory);

                // Встановлюємо список категорій як джерело даних для ComboBox
                categoryComboBox.ItemsSource = categories;
            }

            // Заповнити поля форми даними витрати
            categoryComboBox.SelectedItem = _expense.Category; // Встановлюємо обрану категорію
            amountTextBox.Text = _expense.Amount.ToString();
            datePicker.SelectedDate = _expense.Date;
            descriptionTextBox.Text = _expense.Description;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримати дані з форми
            var selectedCategory = categoryComboBox.SelectedItem as Category;
            if (selectedCategory == null)
            {
                MessageBox.Show("Please select a valid category!");
                return;
            }

            if (!decimal.TryParse(amountTextBox.Text, out decimal amount))
            {
                MessageBox.Show("Incorrect amount format!");
                return;
            }

            DateTime date = datePicker.SelectedDate ?? DateTime.Now;
            string description = descriptionTextBox.Text;

            // Оновити дані витрати
            _expense.CategoryId = selectedCategory.Id; // Отримуємо Id обраної категорії
            _expense.Category = selectedCategory; // Присваиваем категорию объекту
            _expense.Amount = amount;
            _expense.Date = date;
            _expense.Description = description;

            // Зберегти зміни в базі даних
            try
            {
                using (var context = new ExpenseTrackerContext())
                {
                    context.Entry(_expense).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

                // Повідомити про успішне збереження
                MessageBox.Show("Changes saved!");
                DialogResult = true; // Повернути true, щоб MainWindow знав, що дані були змінені
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}");
            }

            // Закрити вікно редагування
            this.Close();
        }
    }
}
