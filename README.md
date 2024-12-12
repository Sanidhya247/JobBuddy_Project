
# Job Buddy

The “Job Buddy” is a modern web application that enhances the experience for job seekers and employers. 
The application aims to create a communication-friendly environment and provides interaction tools for 
employers and Job seekers. Key features of the Job Buddy project involves a dedicated space for all Job postings 
and filtering Jobs with Location, Salary range, Date posted etc., an Employer section for posting jobs and tracking 
the applications with options to reject or select and schedule interviews. Features which make the project 
outstanding from other available Job application portals are Job seeker can create or tailor their resume with our 
AI-integrated tools and check the ATS score for specific job descriptions. Job Buddy also provides a feature to 
connect with the Employer directly and discuss job-related skills and stand out from other applicants.
## Authors

- [@Hemanth Kumar Yamanki](https://github.com/HemanthTech-freak)
- [@Sanidhya Kanani](https://github.com/Sanidhya247)
- [@Zankhana Mayani](https://github.com/Zankhna1810)
- [@Tushar Dagar](https://github.com/01dagar)

## Color Reference

| Color             | Hex                                                                |
| ----------------- | ------------------------------------------------------------------ |
| Background-color | ![#2b4a42](https://via.placeholder.com/10/2b4a42?text=+) #2b4a42 |
| Text font - Color | ![#e3e6d8](https://via.placeholder.com/10/e3e6d8?text=+) #e3e6d8 |
| Error Color | ![#e74c3c](https://via.placeholder.com/10/e74c3c?text=+) #e74c3c |



## pre-requisites

- [Visual studio 2022](https://visualstudio.microsoft.com/downloads/)
- [Dotnet core 8.0 sdk](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Sql server](https://www.microsoft.com/en-ca/sql-server/sql-server-downloads)
- [Visual studio 2022](https://visualstudio.microsoft.com/downloads/)
- [Sql Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)
- [Node Js](https://nodejs.org/en/download/package-manager)
- [Visual studio Code](https://code.visualstudio.com/download)
## Technology Stack

- Backend - C#, .net core 8.0, Entity framework core
- Frontend - React
- Other tools and technologies - Jwt, Axios, Automapper, Fluent Validators, Font awesome icons, Bcrypt, Serilog
## Steps to Run

```bash 
Clone the github code using "Git clone https://github.com/Sanidhya247/JobBuddy_Project.git"
```
Backend :
```bash 
Open the backend project solution in either Visual studio(Preffered) or Visual studio code 
```
```bash 
Go to Appsettings.json file and change the server name to your sql server name (for eg., "localhost","Your-laptopname"check for the server name in your ssms connections)
```
```bash 
Open Package manager console in Visual studio from Tools > Nuget Package Manager> Package manager console
```
```bash 
Run "Update-database"
```
```bash 
JobBuddy DB is created in your sql server.
```
```bash 
Or you can skip the above two steps and directly run the "JobbuddySqlScriptSprint2" file shared via submission folder in the SSMS to create the JobBuddyDb along with data needed to run application.
```
```bash 
Now! Job buddy database with tables are created in your sql server.
```
```bash 
Run the application from Visual studio by clicking on green button from below the menu section. The app starts on port 7113.
```
Frontend:
```bash 
Open the front end code in VS code and open the terminal and run the following command
```
```bash 
Npm install
```
```bash 
If your backend url runs on any other port than 7113, you need to configure the backend url in .env file in frontend
```
```bash 
Run "Npm start" command 
```
```bash 
Now UI application runs on localhost:3000
```
## Demo

https://drive.google.com/file/d/1oZIWdZHSxJO3qT2auFkxFE0SbhnUQ-OT/view?usp=drive_link

# API Documentation - Swagger
## Version: 1.0

##Below are the few api endpoints, for more details on the API documentation please visit [Swagger-API documentation](https://localhost:7113/swagger/index.html)

### /api/Auth/register

#### POST

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| fullName | body |  | Yes | string |
| email | body |  | Yes | string |
| password | body |  | Yes | string |
| role | body |  | Yes | string |
| phoneNumber | body |  | No | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Auth/login

#### POST

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| email | body |  | Yes | string |
| password | body |  | Yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Auth/forgot-password

#### POST

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| email | body |  | Yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Auth/confirm-email

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| token | query |  | No | string |
| userId | query |  | No | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Auth/reset-password

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| token | query |  | No | string |
| userId | query |  | No | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Auth/verify-token

#### GET
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Configuration/{key}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| key | path |  | Yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| key | path |  | Yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Configuration/all

#### GET
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Configuration/create

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| key | body |  | Yes | string |
| value | body |  | Yes | string |
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Configuration/update

#### PUT
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Configuration/clear-cache

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |



