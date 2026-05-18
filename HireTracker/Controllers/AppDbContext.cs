using HireTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HireTracker.Controllers
{
    //This class is what connects the models to the database
    //AppDbContext inherits from the EF Core base class (DbContext)
    public class AppDbContext : DbContext       
    {
        //Constructor passes options up to the base class so that connection string gets passed in
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }    
        //Booking tbl
        public DbSet<Booking> Bookings { get; set; }
    }
}
