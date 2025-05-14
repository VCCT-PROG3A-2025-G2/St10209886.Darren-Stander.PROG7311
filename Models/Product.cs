// Start of file
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TestApp.Models
{
    // Start of Product class
    public class Product
    {
        // Primary key for the product
        public int Id { get; set; }

        // Name of the product
        public string Name { get; set; }

        // Category of the product
        public string Category { get; set; }

        // Date when the product was produced
        public DateTime ProductionDate { get; set; }

        // Foreign key linking to the owning Farmer
        public int FarmerId { get; set; }

        // Navigation property to the associated Farmer
        [ValidateNever]
        public Farmer Farmer { get; set; }
    }
    // End of Product class
}
// End of file
