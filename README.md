# PAPaymentGateway
This is a project aimed at implementing a Simple REST API Payment Gateway for Merchants.

## Technologies Used :
- .NET Core 3.0 => Some later dependencies have been added.
- SQL Server

## Documentation
### API Documentation
- There is a HTML file that has the documentation for the API functions that are currently available.

### Planning Stage Documentation
- I have added some documentation of some initial diagrams for the project. These were just rough guides in terms
of how I saw everything initially.

### Assumptions
- I have assumed some general things based on some of the information I have seen
about payments and validation. I have implemented just a general version of what I initially have had time
to implement regarding a Payment Gateway.

## Project Setup

### Database
- Currently the project has been setup in a way to use a relational database. At the moment is SQL Server.
- If you would like to try this out, you will need to tweak the AppSettings.json file to point to wherever you would like.

### Structure
- The structure of the project at the moment consists of a Single REST styled API and some class libraries.
- There is a WinForms application to represent a Client using the API.

