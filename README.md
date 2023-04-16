# Project: Dating Application
This is a dating application that allows users to like or message other users.

### Technologies Used
- Backend - .Net 7.0/Entity Framework
- Database - Postgres
- Frontend - Angular 14
- Styling - Bootstrap 5
- Media Management - Cloudinary
- Deployment - Fly.io
- Container - Docker
- Git GUI - GitKraken
- IDE - Rider

### Setup/Installation
[Frontend Github](https://github.com/harryly140/dating-app-fe)  
Local Setup:  
Make sure your local appsettings.json exists and is correct  

Build docker image  
```
docker build -t SOME_DOCKER_IMAGE .
```  
Run docker desktop and connect to the Postgres db and go to (http://localhost:8080) 
``` 
docker run --rm -it -p 8080:80 SOME_DOCKER_IMAGE:latest
```  
If it runs as as intended, push the image to docker
```
docker push SOME_DOCKER_IMAGE:latest
```  
If any changes occur, you have to rebuild the image and push it

### Key Concepts
- CORS Policy
- HTTPS Security
- Hashing/Salting
- Registration/Validation
- Migrations
- Authorization
- Authentication Middleware
- Exception Handling Middleware
- AutoMapper
- CRUD Operations
- Pagination 
- Filtering/Sorting
- Messaging
- Roles/Identities
- Seeding Data
- JWT
- Presence/Message Hubs
- SignalR

### Approach
Worked on this tutorial during my free time to further my understanding with .Net and Angular while contracted on a project with a similar tech stack.
### Status
Application is finished and able to be run locally. Currently having issues with deployment on Fly.io.
(https://harry-dating-app.fly.dev/)
### Credits
Tutorial was taught by Neil Cummings on Udemy.