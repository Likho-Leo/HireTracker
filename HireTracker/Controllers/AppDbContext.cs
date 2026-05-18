using HireTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HireTracker.Controllers
{
    //This class is what connects the models to the database
    public class AppDbContext : IdentityDbContext<IdentityUser>       
    {
        //Constructor passes options up to the base class so that connection string gets passed in
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }    
        //Booking tbl
        public DbSet<Booking> Bookings { get; set; }
    }
}
