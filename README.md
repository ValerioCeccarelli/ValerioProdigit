# ValerioProdigit
Simple web api project inspired by the reservation system of my university

## Installation, Build and Run
### Minimum requirements
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
### Optional settings
1) Configure the SendGrid email services:
    - Create a free account on SendGrid
    ```
    https://signup.sendgrid.com/
    ```
    - Create an API key
    ```
    https://app.sendgrid.com/settings/api_keys
    ```
    - Add the API key to the appsettings.json file
    ```
    Settings -> SendGridSettings -> ApiKey
    ```
    - Add the email address of the sender
    ```
    Settings -> SendGridSettings -> SenderEmail
    ```
2) Configure the Swagger UI:
   ```json
   "Swagger": {
    "Contact": {
      "Name": "<contact name>",
      "Email": "<contact email>",
      "Url": "<contact url>"
    },
    "License": {
      "Name": "<license name>",
      "Url": "<license url>"
    },
    "Info": {
      "Version": "V1",
      "Title": "Api for ValerioProdigit",
      "Description": "<description>",
    }
   }
   ```
    