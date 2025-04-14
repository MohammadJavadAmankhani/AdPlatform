# AdPlatform

A microservices-based advertisement platform designed for high transaction volumes, as per the Digikala job posting requirements. The system ensures accurate wallet transaction reports, high availability, and optimized inter-service communication.

## Architecture

The system is built using **Clean Architecture**, **CQRS**, and **Event Sourcing** with a microservices approach. It consists of three services:

1. **Wallet Service** (PC/EC):
   - Manages user wallets with multiple account types.
   - Uses **SQL Server** for strong consistency.
   - Ensures accurate transaction reports.

2. **Ad Management Service**:
   - Updates ad statuses based on wallet balances.
   - Uses **Redis** for caching and **RabbitMQ** for event-driven communication.
   - Balances consistency and low latency.

3. **Ad Delivery Service** (PA/EL):
   - Delivers ads to users.
   - Uses **Cosmos DB** for high availability and low latency.

**CAP and PACELC:**
- **Wallet Service**: Prioritizes **Consistency** (PC/EC) for accurate reporting.
- **Ad Delivery Service**: Prioritizes **Availability** and **Low Latency** (PA/EL) for uninterrupted ad delivery.
- **Ad Management Service**: Balances consistency with wallet and low latency via caching.


[User] --> [API Gateway]
                    |
    --------------------------------
    |                |             |
[Wallet Service] [Ad Management] [Ad Delivery]
    |                |             |
[SQL Server]     [Redis]       [Cosmos DB]
    |                |             |
    ----------------[RabbitMQ]-----


## Technologies

- **.NET 8** with **ASP.NET Core** for all services.
- **SQL Server** for Wallet Service (PC/EC).
- **Cosmos DB** for Ad Delivery Service (PA/EL).
- **Redis** for caching in Ad Management Service.
- **RabbitMQ** for event-driven communication.
- **Docker** for containerization.

   
## Prerequisites

- .NET 8 SDK
- Docker (for RabbitMQ and Cosmos DB emulator)
- RabbitMQ (amqp://guest:guest@localhost:5672/)
- Cosmos DB Emulator (https://localhost:8081/)

Start RabbitMQ and Cosmos DB using Docker:
docker-compose up -d


AdPlatform
├── WalletService
│   ├── Core
│   ├── Application
│   ├── Infrastructure
│   ├── Presentation
├── AdManagementService
│   ├── Core
│   ├── Application
│   ├── Infrastructure
│   ├── Presentation
├── AdDeliveryService
│   ├── Core
│   ├── Application
│   ├── Infrastructure
│   ├── Presentation
├── docker-compose.yml
├── README.md


API Endpoints
WalletService: POST http://localhost:5037/api/wallet
AdManagementService: POST http://localhost:5248/api/ads
AdDeliveryService: GET http://localhost:5199/api/ads





