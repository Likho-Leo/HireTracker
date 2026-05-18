using System.ComponentModel.DataAnnotations;

namespace HireTracker.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public string? Pickup { get; set; }
        [Required]
        public string? Dropoff { get; set; }
        [Required]
        public string? Vehicle {  get; set; }
        [Required]
        public string? CompanyName { get; set; }
        [Required]
        public string? ContactNumber { get; set; }
        [Required]
        public int Passengers { get; set; }
        [Required]
        public decimal PricePerhead { get; set; }
        public decimal AmountDue {  get; set; }
        public decimal AmountPaid { get; set; }
        [Required]
        public string? PaymentStatus { get; set; }
        [Required]
        public string? TripStatus { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
