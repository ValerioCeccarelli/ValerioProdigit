# ValerioProdigit
Simple web api project inspired by the reservation system of my university

## Installation, Build and Run
1) Download and install the .NET SDK version 6 or higher
```
https://dotnet.microsoft.com/en-us/download
```
2) Open a terminal and clone the project using
```
git clone https://github.com/ValerioCeccarelli/ValerioProdigit.git
```
3) Browse the main project directory
```
cd ValerioProdigit/ValerioProdigit.Api/
```
4) Complete the configuration with your personal data:
go to appsettings.json and change the following settings
  - Jwt key token (REQUIRED): add an alphanumeric string of 32 chars ```Settings -> JwtSettings -> Secret```
  - HashId salt (REQUIRED): add an alphanumeric string ```Settings -> HashidSettings -> Salt```
  - Send Grid, sender email (REQUIRED): add a valid email registered on SendGrid ```Settings -> SendGridSettings -> ServiceEmail```
  - Send Grid, api token (REQUIRED): add a valid SendGrid api token ```Settings -> SendGridSettings -> Token```

5) Install the dotnet Entity Framework tools
```
dotnet tool install --global dotnet-ef
```
6) Create the first migration
```
dotnet ef migrations add "Init"
```
7) Update the database
```
dotnet ef database update
```
8) Restore dependencies
```
dotnet restore
```
9) Run the project
```
dotnet run
```
