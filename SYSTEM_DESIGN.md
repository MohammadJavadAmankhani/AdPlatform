# System Design for Ad Platform

## Overview
A microservices-based system for managing wallets, ads, and ad delivery, optimized for accurate cost reporting, high availability, and efficient communication.

## Components
- **Wallet Service**: Manages user wallets with multiple accounts (e.g., toman, points).
- **Ad Management Service**: Creates ads and updates status based on budget.
- **Ad Delivery Service**: Serves active ads to users.
- **Event Bus (RabbitMQ)**: Handles asynchronous communication.
- **Database (Cosmos DB)**: Stores wallets, ads, and transactions with Strong Consistency for transactions.
- **Reporting Service**: Generates detailed cost reports.

## Workflow
1. User creates an ad via `POST /ads { userId, budget }`.
2. **Ad Management Service** checks balance via **Wallet Service**, saves ad, and publishes `AdCreated` to **RabbitMQ**.
3. **Ad Delivery Service** consumes `AdCreated`, serves ads, and publishes `AdImpression` events.
4. **Ad Management Service** processes `AdImpression`, updates wallet via **Wallet Service**.
5. **Reporting Service** queries **Cosmos DB** for cost reports.

## Requirements
1. **Accurate Reporting**:
   - Store transactions with `transactionId`, `timestamp` in **Cosmos DB**.
   - Use Strong Consistency (PC/EC) for wallet updates.
   - Example: `{ userId: "ali123", adId: "ad123", totalSpent: 1000000 }`.
2. **High Availability**:
   - **RabbitMQ** with mirroring (PA/EL).
   - **Cosmos DB** with multi-region deployment.
   - Circuit Breaker in **Ad Delivery Service** for fault tolerance.
3. **Optimized Communication**:
   - Asynchronous via **RabbitMQ** for events (e.g., `AdImpression` in 10ms).
   - Synchronous via **gRPC** for balance checks (20ms).
   - Cache active ads in **Redis**.

## CAP and PACELC
- **Wallet Service**: CP (Consistency for accurate balances), PC/EC (Consistency over Latency).
  - Example: Transactions take 300ms to ensure all nodes are synced.
- **Ad Delivery Service**: AP (Availability for ad serving), PA/EL (Low Latency).
  - Example: Ads served in 50ms, with eventual consistency.

## Scalability
- Auto-scaling **Cosmos DB** and **Kubernetes** for services.
- **Redis** for caching active ads.
- Rate limiting in **API Gateway** for peak loads (e.g., Black Friday).

## Monitoring
- **Prometheus** for latency and error metrics.
- **Grafana** for transaction dashboards.



+----------------+       +-----------------+
|    Client      | ----> |   API Gateway   |
+----------------+       +-----------------+
                           |
                           |
        +------------------+------------------+
        |                  |                  |
        v                  v                  v
+----------------+  +----------------+  +----------------+
| Wallet Service |  | Ad Management |  | Ad Delivery    |
+----------------+  +----------------+  +----------------+
        |                  |                  |
        |                  |                  |
        +--------[ Event Bus (RabbitMQ) ]----+
        |                                     |
        v                                     v
+----------------+                    +----------------+
| Database       |                    | Reporting      |
| (Cosmos DB)    |                    | Service        |
+----------------+                    +----------------+
