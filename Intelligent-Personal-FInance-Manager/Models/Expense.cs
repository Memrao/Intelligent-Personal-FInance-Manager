using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        // Default constructor for EF (Entity Framework)
        public Expense() { }

        // Constructor to initialize values directly
        public Expense(int categoryId, decimal amount, DateTime date, string description)
        {
            CategoryId = categoryId;
            Amount = amount;
            Date = date;
            Description = description;
        }

        [Key]
        public int Id { get; set; }

        // Encapsulation: CategoryId as a foreign key (with validation)
        [Required]
        [ForeignKey("Category")] // Specifies that CategoryId is a foreign key to Category
        public int CategoryId { get; set; }

        // Navigation property for accessing related Category
        public virtual Category Category { get; set; }

        // Encapsulation: Amount property with validation
        [Required]
        public decimal Amount { get; set; }

        // Encapsulation: Date property with validation
        [Required]
        public DateTime Date { get; set; }

        // Optional description for the expense
        public string Description { get; set; }

        // Optional method to display expense information
        public string GetExpenseInfo()
        {
            return $"Expense ID: {Id}, Category ID: {CategoryId}, Amount: {Amount:C}, Date: {Date.ToShortDateString()}, Description: {Description}";
        }
    }
}
