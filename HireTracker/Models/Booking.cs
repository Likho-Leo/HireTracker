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
        [Display(Name ="Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Company contact number is required")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Passengers is required")]
        public int Passengers { get; set; }

        [Required(ErrorMessage = "Price per head is required")]
        [Column (TypeName = "decimal(18,2)")]
        [Display(Name = "Price Per Head")]
        public decimal PricePerhead { get; set; }

        [Required(ErrorMessage = "Amount due is required")]
        [Column (TypeName = "decimal(18,2)")]
        [Display(Name = "Amount Due")]
        public decimal AmountDue {  get; set; }

        [Column (TypeName = "decimal(18,2)")]
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment status is required")]
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }

        [Required(ErrorMessage = "Trip status is required")]
        [Display(Name = "Trip Status")]
        public string TripStatus { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name ="Updated At")]
        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
