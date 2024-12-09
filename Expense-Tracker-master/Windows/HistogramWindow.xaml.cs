using ExpenseTracker.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExpenseTracker.Windows
{
    public partial class HistogramWindow : Window
    {
        public List<string> Categories { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        private DateTime startDate;
        private DateTime endDate;

        public HistogramWindow(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            this.startDate = startDate;
            this.endDate = endDate;

            SeriesCollection = new SeriesCollection();
            Categories = new List<string>();

            using (var context = new ExpenseTrackerContext())
            {
                // Загружаем все категории из базы данных
                var allCategories = context.Categories.Select(c => c.Name).ToList();

                // Сортируем категории в алфавитном порядке, исключая "Other"
                Categories = allCategories
                    .Where(c => c != "Other")
                    .OrderBy(c => c)
                    .ToList();

                // Добавляем "Other" в конец списка
                Categories.Add("Other");
            }

            // Добавляем "All Categories" как специальную категорию в начало списка
            Categories.Insert(0, "All Categories");

            UpdateHistogram();
        }

        private void UpdateHistogram()
        {
            using (var context = new ExpenseTrackerContext())
            {
                string selectedCategory = (categoryComboBox.SelectedItem as string);

                // Если выбрана специальная категория "All Categories" или ничего не выбрано, показываем все расходы
                if (selectedCategory == "All Categories" || string.IsNullOrEmpty(selectedCategory))
                {
                    var expenses = context.Expenses
                        .Where(e => e.Date >= startDate && e.Date <= endDate)
                        .GroupBy(e => e.Date)
                        .Select(g => new { Date = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                        .ToList();

                    SeriesCollection.Clear();
                    SeriesCollection.Add(new ColumnSeries
                    {
                        Title = "All Categories",
                        Values = new ChartValues<decimal>(expenses.Select(e => e.TotalAmount)),
                    });

                    Labels = expenses.Select(e => e.Date.ToShortDateString()).ToArray();
                }
                else
                {
                    // Иначе показываем расходы только по выбранной категории
                    var expenses = context.Expenses
                        .Where(e => e.Date >= startDate && e.Date <= endDate)
                        .Where(e => e.Category.Name == selectedCategory)
                        .GroupBy(e => e.Date)
                        .Select(g => new { Date = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                        .ToList();

                    SeriesCollection.Clear();
                    SeriesCollection.Add(new ColumnSeries
                    {
                        Title = selectedCategory,
                        Values = new ChartValues<decimal>(expenses.Select(e => e.TotalAmount)),
                    });

                    Labels = expenses.Select(e => e.Date.ToShortDateString()).ToArray();
                }
            }

            DataContext = this;
        }

        private void ShowAllCategories_Click(object sender, RoutedEventArgs e)
        {
            categoryComboBox.SelectedItem = "All Categories";
            UpdateHistogram();
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateHistogram();
        }

        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            startDate = startDatePicker.SelectedDate ?? DateTime.MinValue;
            endDate = endDatePicker.SelectedDate ?? DateTime.MaxValue;
            UpdateHistogram();
        }
    }
}