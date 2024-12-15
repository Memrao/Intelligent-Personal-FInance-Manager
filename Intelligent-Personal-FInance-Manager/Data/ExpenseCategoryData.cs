using System;

namespace ExpenseTracker.Data
{
    public class ExpenseCategoryData
    {
        private string _category;
        private double _amount;

        // Encapsulation with validation for Category
        public string Category
        {
            get => _category;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty.");
                _category = value;
            }
        }

        // Encapsulation with validation for Amount
        public double Amount
        {
            get => _amount;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Amount cannot be negative.");
                _amount = value;
            }
        }

        // Parameterized Constructor for initialization
        public ExpenseCategoryData(string category, double amount)
        {
            Category = category;
            Amount = amount;
        }

        // Display Method (Optional for debugging or info)
        public string DisplayInfo()
        {
            return $"Category: {Category}, Amount: {Amount:C}";
        }
    }
}
