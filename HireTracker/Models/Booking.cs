using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireTracker.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public string Pickup { get; set; }
        [Required]
        public string Dropoff { get; set; }
        [Required]
        public string Vehicle {  get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public int Passengers { get; set; }
        [Required]
        [Column (TypeName = "decimal(18,2)")]
        public decimal PricePerhead { get; set; }
        [Column (TypeName = "decimal(18,2)")]
        public decimal AmountDue {  get; set; }
        [Column (TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public string TripStatus { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
