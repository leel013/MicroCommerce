# MicroCommerce

MicroCommerce is a sample microservices-based application built with ASP.NET Core. The project demonstrates service-to-service communication using REST APIs, API Gateway implementation with YARP, asynchronous messaging using RabbitMQ, and containerization with Docker Compose.

The primary objective of this project is to understand the core concepts of microservices architecture and the communication patterns commonly used in enterprise applications.

---

## Architecture

```
                        Client
                           |
                           |
                    API Gateway (YARP)
                     /              \
                    /                \
                   /                  \
        Product Service          Order Service
                                      |
                                      |
                                 RabbitMQ
                                 /       \
                                /         \
                               /           \
                 Inventory Service   Notification Service
```

---

## Solution Structure

```
MicroCommerce
│
├── ApiGateway
├── ProductService
├── OrderService
├── InventoryService
├── NotificationService
├── docker-compose.yml
└── MicroCommerce.sln
```

---

## Services

### API Gateway

Acts as a single entry point for client applications and routes incoming requests to the appropriate microservice using YARP Reverse Proxy.

Responsibilities:

- Request routing
- API Composition
- Service abstraction

---

### Product Service

Responsible for managing product-related operations.

Sample Endpoints

| Method | Endpoint |
|--------|----------|
| GET | `/api/products` |
| GET | `/api/products/{id}` |

---

### Order Service

Responsible for order creation and retrieval.

During order creation, an event is published to RabbitMQ for downstream processing.

Sample Endpoints

| Method | Endpoint |
|--------|----------|
| GET | `/api/orders/{id}` |
| POST | `/api/orders` |

---

### Inventory Service

Consumes order events from RabbitMQ and updates inventory.

Responsibilities:

- Consume OrderCreated events
- Update stock
- Event-driven processing

---

### Notification Service

Consumes order events from RabbitMQ and simulates sending customer notifications.

Responsibilities:

- Consume OrderCreated events
- Send notification
- Independent background processing

---

## Communication Flow

### Synchronous Communication

```
Client
   |
   |
API Gateway
   |
   |
Order Service
   |
   |
Product Service
```

REST APIs are used for synchronous communication between services.

---

### Asynchronous Communication

```
Order Service

      |

RabbitMQ Exchange

   |          |

   |          |

Inventory   Notification
 Service      Service
```

RabbitMQ is used for asynchronous communication following the Publish/Subscribe pattern.

---

## Technologies

- ASP.NET Core 8
- C#
- YARP Reverse Proxy
- RabbitMQ
- Worker Service
- Docker
- Docker Compose
- REST API
- Dependency Injection

---

## Running the Application

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- Visual Studio 2022

### Run using Docker

```bash
docker compose up --build
```

---

## RabbitMQ Management

Management Console

```
http://localhost:15672
```

Default Credentials

```
Username: guest
Password: guest
```

---

## Concepts Demonstrated

- Microservices Architecture
- API Gateway
- API Composition
- REST Communication
- RabbitMQ
- Exchanges
- Queues
- Fanout Exchange
- Publisher / Consumer Pattern
- Worker Services
- Event-Driven Architecture
- Docker
- Docker Compose
- Docker Networking

---

## Author

**Leel Kumar**
