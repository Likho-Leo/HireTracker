# HireTracker

A private web-based hire management system built for a family transport business in Matatiele, Eastern Cape. Replaces a paper-based booking system used to track taxi and bus charter hires.

Currently in use by the business owners to manage bookings, track payments and monitor outstanding balances.

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- SQL Server with Entity Framework Core (Code First)
- ASP.NET Identity (authentication)
- Bootstrap 5
- X.PagedList (pagination)
- Resend (password reset emails)

## Features

**Bookings**
- Create, edit, view and soft delete bookings
- Multiple vehicle selection per booking (stored as comma separated string)
- Vehicle aggregation display (e.g. 22 seater x2)
- Return date support with same day return option
- Auto payment status based on amount paid vs amount due
- Not yet paid checkbox for quick booking capture
- Outstanding balance calculation per booking
- CreatedAt and UpdatedAt tracking

**Search and Filtering**
- Server side search by company name or route
- Filter by trip status, payment status and date range
- Pagination with filter persistence across pages

**Dashboard**
- Total bookings, pending trips, completed trips and outstanding payments
- Recent bookings table

**Authentication**
- Login with Remember Me
- Forgot password with email reset via Resend
- Password reset token expiry
- Account lockout on failed attempts
- User seeding on startup
- All routes protected with authorization

**Validation**
- Passenger limit based on combined vehicle capacity
- Return date cannot be before departure date
- Past date prevention on booking date
- South African contact number format validation
- Contact number restricted to numbers and + only

## Setup

**Prerequisites**
- .NET 8 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022

**Steps**

1. Clone the repository
2. Copy `appsettings.example.json` to `appsettings.json` and fill in your values
3. Update the connection string to point to your SQL Server instance
4. Run migrations:
```
Update-Database
```
5. Run the application — the seeder will create users automatically on first run

## Environment Variables

The following must be configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  },
  "SeedSettings": {
    "User1Email": "user@email.com",
    "User1Password": "Password123!"
  },
  "APISettings": {
    "ApiKey": "your-resend-api-key"
  }
}
```

Never commit `appsettings.json` to version control. Use `appsettings.example.json` as a template.

## Local IIS Deployment

1. Publish the app to a folder from Visual Studio
2. Install the ASP.NET Core Hosting Bundle (x64) on the target machine
3. Install SQL Server Express on the target machine
4. Enable IIS through Windows Optional Features
5. Create a new IIS site pointing to the published folder
6. Update `appsettings.json` in the published folder with the correct connection string
7. Grant the IIS app pool user sysadmin access on SQL Server
8. Run `iisreset` and navigate to `http://localhost`

## Notes

- Soft delete is used throughout — deleted bookings are flagged with `IsDeleted = true` and remain in the database
- Multiple vehicle selection is stored as a comma separated string in the Vehicle column
- Payment status is automatically set based on the relationship between Amount Due and Amount Paid
- The app is intentionally kept private as it handles real business and client data
