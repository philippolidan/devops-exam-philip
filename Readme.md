## Add appsettings.json
cd Application/aspnet-core-dotnet-core

appsettings.json should include AppSettings.Token for JWT Authentication and ConnectionString.DefaultConnection for SQL Database

## Run Application
docker build -t dotnetcore-exam .

docker run -d -p 8080:80 dotnetcore-exam

## Import Postman Collection to access the APIs
https://www.getpostman.com/collections/c97a166b7cecfa3e9450

## APIs
Register User - Creates user to access Book APIs

Login User - Response authentication token to access Book APIs

** Place the Bearer "access" token from Login User API to Headers of the APIs **

Get Books - Return all the books

Add Books - Create Book API

Get Book  - Return specific book based on Id

Update Book - Update specific book based on Id

Delete Book - Delete specific book based on Id

## Note:
You can use this URL: https://devops-exam-philip.azurewebsites.net as base url to access the hosted API in azure.
