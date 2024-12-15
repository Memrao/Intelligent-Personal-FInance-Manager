using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Category
    {
        private string _name;

        // Constructor for initialization
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // Default constructor for EF
        public Category() { }

        [Key]
        public int Id { get; set; }

        // Encapsulation: Name property with validation
        [Required]
        public string Name
        {
            get => _name;
            set
            {
                // Validation: Name should not be null or empty
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty.");
                _name = value;
            }
        }

        // Optional method to display category info (could be helpful for debugging or logs)
        public string GetCategoryInfo()
        {
            return $"Category ID: {Id}, Name: {Name}";
        }
    }
}
