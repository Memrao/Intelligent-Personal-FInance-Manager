using ExpenseTracker.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Windows;

namespace ExpenseTracker.Windows
{
    public partial class PieChartWindow : Window
    {
        public PieChartWindow()
        {
            InitializeComponent();
            LoadChartData();
        }

        private void LoadChartData()
        {
            DateTime startDate = startDatePicker.SelectedDate ?? new DateTime(2001, 1, 1);
            DateTime endDate = endDatePicker.SelectedDate ?? new DateTime(2050, 12, 31);

            using (var context = new ExpenseTrackerContext())
            {
                var data = context.Expenses
                    .Where(e => e.Date >= startDate && e.Date <= endDate)
                    .GroupBy(e => e.Category.Name)
                    .Select(g => new { Category = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                    .ToList();

                SeriesCollection series = new SeriesCollection();

                foreach (var item in data)
                {
                    series.Add(new PieSeries
                    {
                        Title = item.Category,
                        Values = new ChartValues<double> { (double)item.TotalAmount }, // Використовуємо item.TotalAmount
                        DataLabels = true
                    });
                }

                pieChart.Series = series;
            }
        }

        private void OnLoadDataClick(object sender, RoutedEventArgs e)
        {
            LoadChartData();
        }
    }

}