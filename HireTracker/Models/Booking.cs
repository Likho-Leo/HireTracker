using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireTracker.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }
        [Required(ErrorMessage = "Pick up location is required")]
        public string Pickup { get; set; }
        [Required(ErrorMessage = "Dropoff location is required")]
        public string Dropoff { get; set; }
        [Required(ErrorMessage = "Vehicle is required")]
        public string Vehicle {  get; set; }
        [Required(ErrorMessage = "Company name is required")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Company contact number is required")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Passengers is required")]
        public int Passengers { get; set; }
        [Required(ErrorMessage = "Price per head is required")]
        [Column (TypeName = "decimal(18,2)")]
        public decimal PricePerhead { get; set; }
        [Required(ErrorMessage = "Amount due is required")]
        [Column (TypeName = "decimal(18,2)")]
        public decimal AmountDue {  get; set; }
        [Column (TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
        [Required(ErrorMessage = "Payment status is required")]
        public string PaymentStatus { get; set; }
        [Required(ErrorMessage = "Trip status is required")]
        public string TripStatus { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
