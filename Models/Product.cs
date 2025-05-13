using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TestApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }

        // 🔥 Foreign Key
        public int FarmerId { get; set; }

        // 🔥 Navigation Property (Validation Ignored)
        [ValidateNever] // Ignore this during model validation
        public Farmer Farmer { get; set; }
    }
}
