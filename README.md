# ST10434048 Booking System

A comprehensive event booking management system built with ASP.NET Core MVC, Entity Framework Core, and Azure services. This application allows users to manage venues, events, and bookings with a modern web interface.

## 🚀 Features

### Core Functionality
- **Venue Management**: Add, edit, and display venues with capacity and location information
- **Event Management**: Create and manage events with dates, descriptions, and event types
- **Booking System**: Make bookings by associating venues with events
- **Search Functionality**: Search through venues and events
- **Image Management**: Azure Blob Storage integration for venue images

### Technical Features
- **Modern Web Interface**: Responsive design using Bootstrap
- **Database Integration**: SQL Server with Entity Framework Core
- **Cloud Storage**: Azure Blob Storage for image management
- **Data Validation**: Comprehensive input validation and error handling
- **RESTful Architecture**: Clean controller-based API design

## 🏗️ Architecture

### Technology Stack
- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Cloud Services**: Azure SQL Database, Azure Blob Storage
- **Frontend**: HTML, CSS (Bootstrap), JavaScript
- **Development**: Visual Studio 2022

### Project Structure
```
ST10434048_BookingSystem/
├── Controllers/          # MVC Controllers
├── Data/                # Database context and migrations
├── Models/              # Entity models and view models
├── Services/            # Business logic services
├── Views/               # Razor views
├── wwwroot/            # Static files (CSS, JS, Images)
└── Migrations/         # Entity Framework migrations
```

## 📋 Prerequisites

Before running this application, ensure you have:

- **.NET 8.0 SDK** installed
- **SQL Server** (Local or Azure)
- **Visual Studio 2022** (recommended) or VS Code
- **Azure Account** (for cloud services)

## 🛠️ Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd ST10434048_BookingSystem
```

### 2. Database Setup

#### Option A: Local SQL Server
1. Ensure SQL Server is running locally
2. Update the connection string in `appsettings.json`:
```json
"localConnection": "Server=YOUR_SERVER\\SQLEXPRESS;Database=BookingSystem;Trusted_Connection=True;TrustServerCertificate=True"
```

#### Option B: Azure SQL Database
1. Use the existing Azure connection string in `appsettings.json`
2. Ensure the Azure SQL Database is accessible

### 3. Run Database Migrations
```bash
# Navigate to the project directory
cd ST10434048_BookingSystem

# Update the database with migrations
dotnet ef database update
```

### 4. Configure Azure Blob Storage (Optional)
If you want to use Azure Blob Storage for image management:
1. Update the Azure Blob Storage connection string in `appsettings.json`
2. Ensure the container exists in your Azure Storage account

### 5. Run the Application
```bash
# Build the project
dotnet build

# Run the application
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## 🗄️ Database Schema

### Core Entities

#### Venue
- `VenueId` (Guid, Primary Key)
- `VenueName` (string, required)
- `Location` (string, required)
- `Capacity` (int, required, 1-100000)
- `ImageUrl` (string, required)

#### Event
- `EventId` (Guid, Primary Key)
- `EventName` (string, required)
- `EventDate` (DateOnly, required)
- `Description` (string, required)
- `EventTypeID` (int, Foreign Key)

#### Booking
- `BookingId` (Guid, Primary Key)
- `VenueId` (Guid, Foreign Key)
- `EventId` (Guid, Foreign Key)
- `BookingDate` (DateOnly, required)

#### EventType
- `EventTypeID` (int, Primary Key)
- `EventTypeName` (string, required)

## 🎯 Usage

### Managing Venues
1. Navigate to **Venues** section
2. **Add Venue**: Create new venues with name, location, capacity, and image
3. **Display Venues**: View all available venues
4. **Edit Venue**: Modify existing venue information

### Managing Events
1. Navigate to **Events** section
2. **Add Event**: Create new events with name, date, description, and type
3. **Display Events**: View all scheduled events
4. **Edit Event**: Modify existing event details

### Making Bookings
1. Navigate to **Bookings** section
2. **Add Booking**: Create new bookings by selecting venue and event
3. **Display Bookings**: View all existing bookings
4. **Edit Booking**: Modify booking details

### Searching
1. Use the **Search** functionality to find venues and events
2. Filter by various criteria to locate specific items

## 🔧 Configuration

### Connection Strings
The application supports multiple connection strings:
- **PrimaryConnection**: Azure SQL Database
- **localConnection**: Local SQL Server instance

### Azure Blob Storage
Configure Azure Blob Storage settings in `appsettings.json`:
```json
"AzureBlobStorage": {
    "ConnectionString": "your-connection-string",
    "ContainerName": "your-container-name",
    "BaseUrl": "your-base-url"
}
```

## 🚀 Deployment

### Azure Deployment
1. Create an Azure App Service
2. Configure the connection strings in Azure App Settings
3. Deploy using Visual Studio or Azure CLI

### Local Deployment
1. Ensure SQL Server is running
2. Update connection strings for local environment
3. Run the application using `dotnet run`

## 📁 Key Files

- **Program.cs**: Application entry point and service configuration
- **BookingsDbContext.cs**: Entity Framework database context
- **Controllers/**: MVC controllers for each entity
- **Models/Entities/**: Database entity models
- **Views/**: Razor view templates
- **appsettings.json**: Configuration settings

## 🔒 Security Considerations

- Connection strings should be stored securely (not in source control)
- Use Azure Key Vault for production secrets
- Implement proper authentication and authorization
- Validate all user inputs
- Use HTTPS in production

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify SQL Server is running
   - Check connection string in `appsettings.json`
   - Ensure database exists and migrations are applied

2. **Azure Blob Storage Issues**
   - Verify Azure Storage account credentials
   - Check container permissions
   - Ensure container exists

3. **Migration Errors**
   - Run `dotnet ef database update` to apply migrations
   - Check for conflicting migrations

## 📝 License

This project is part of the ST10434048 coursework and is intended for educational purposes.

## 👥 Contributing

This is an academic project. For questions or issues, please contact the development team.

---

**Developed by**: ST10434048  
**Framework**: ASP.NET Core 8.0  
**Database**: SQL Server with Entity Framework Core  
**Cloud Services**: Azure SQL Database, Azure Blob Storage 