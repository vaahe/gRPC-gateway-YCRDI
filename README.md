# gRPC-gateway-YCRDI

## Overview
This repository contains the backend services for the game, built using ASP.NET Core, gRPC, and the API Gateway pattern.

## Project Architecture: ApiGateway + Microservices
This project demonstrates a `microservice architecture` using `.NET 9`, `gRPC`, and a `REST API` gateway. The system consists of:

**1.** ApiGateway \
**2.** AuthService \
**3.** UserService \
**4.** SerialPortService \
**5.** InternalCommandsService

## Folder Structure
```
ProjectRoot/
├─ Services/
│  └─ AuthService/
│     ├─ Protos/auth.proto
│     ├─ Services/AuthServiceImpl.cs
│     ├─ Program.cs
│     └─ AuthService.csproj
└─ Gateway/
   └─ ApiGateway/
      ├─ Controllers/AuthController.cs
      ├─ Program.cs
      └─ ApiGateway.csproj
```

## Communication Flow
**1.** Client sends REST request to ApiGateway. \
**2.** ApiGateway maps the REST request to gRPC request. \
**3.** AuthService processes gRPC request and returns a response. \
**4.** ApiGateway returns the result to the client as JSON.

```
POST /api/auth/signin
Body: { "username": "test", "password": "123" }

ApiGateway -> AuthService.SignInAsync(SignInRequest)
AuthService -> SignInResponse { userId = "guid" }
ApiGateway -> Client JSON response
```

## Key Design Decisions
| Component       | Choice                             | Reason                                                                   |
| --------------- | ---------------------------------- | ------------------------------------------------------------------------ |
| AuthService     | gRPC microservice                  | High performance, strongly-typed, scalable for microservice architecture |
| ApiGateway      | REST + gRPC client                 | REST-friendly for clients, forwards to gRPC services                     |
| Proto files     | Protobuf                           | Defines contracts between services, language-agnostic                    |
| Logging         | Console / ILogger                  | Observability for startup and request handling                           |
| Ports           | 5001 (Gateway), 5002 (AuthService) | Avoid port conflicts; each service independent                           |
| Swagger/OpenAPI | ApiGateway                         | Easy testing and documentation for REST endpoints                        |


## Feedback
Feel free to reach me out on [LinkedIn](https://www.linkedin.com/in/vaahe/).
