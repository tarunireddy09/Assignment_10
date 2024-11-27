using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalSystem.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]  
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Range(1900, 2100)]  
        public int Year { get; set; }

        [Required]
        [Range(1, double.MaxValue)]  
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;  
    }
}
