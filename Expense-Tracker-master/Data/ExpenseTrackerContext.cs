using System.Data.Entity;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data
{
    // Inheritance: Base class for common DbSet properties
    public abstract class BaseContext : DbContext
    {
        protected BaseContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        // Abstract method for customizing model creation
        protected abstract void OnModelCreatingCustom(DbModelBuilder modelBuilder);

        // Ensures database model is created properly
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingCustom(modelBuilder);
        }
    }

    // ExpenseTrackerContext now inherits from BaseContext
    public class ExpenseTrackerContext : BaseContext
    {
        // Constructor to pass connection string to the base class
        public ExpenseTrackerContext() : base("PersonalExpenses")
        {
        }

        // Customizing model creation (optional)
        protected override void OnModelCreatingCustom(DbModelBuilder modelBuilder)
        {
            // Your customizations for Expenses and Categories can go here
            modelBuilder.Entity<Expense>().ToTable("Expenses");
            modelBuilder.Entity<Category>().ToTable("Categories");
        }
    }
}
