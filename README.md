![License](https://img.shields.io/badge/license-MIT-green)
![Azure](https://img.shields.io/badge/cloud-Azure-blue)
![Architecture](https://img.shields.io/badge/architecture-event--driven-orange)

# LedgerFlow

**LedgerFlow** is an event-driven, AI-assisted personal finance platform built on Azure.  
It demonstrates how to design resilient, scalable financial systems using modern cloud-native and event-driven architecture patterns.

> This project is intentionally designed as a **staff-level reference architecture**, focusing on clarity, scalability, observability, and real-world trade-offs.

---

## 🚀 Key Goals

- Track personal financial transactions (expenses & income)
- Automatically extract and categorize transactions using AI
- Maintain accurate account balances with eventual consistency
- Provide financial insights through projections and events
- Demonstrate a production-grade, event-driven Azure architecture

---

## 🧠 Architecture Overview

LedgerFlow is built as a **pipeline of independent, event-driven stages**, where each stage is responsible for a single concern.

**Core principles:**
- Event-driven first
- Asynchronous processing
- Loose coupling via contracts
- AI-in-the-loop with confidence and fallback
- Observability and resilience by design

### High-Level Flow

1. Transactions are ingested via API or uploaded receipts
2. Files trigger events through Azure Event Grid
3. Azure Service Bus acts as the event backbone
4. AI services extract and categorize transactions
5. Core finance logic applies transactions to accounts
6. Projections generate read-optimized models and insights

---

## 🧩 Azure Services Used

- **Azure Functions** – Compute and orchestration
- **Azure Service Bus** – Event backbone (topics, retries, DLQ)
- **Azure Event Grid** – Reactive events and fan-out
- **Azure Blob Storage** – Receipt and document storage
- **Azure Storage / Cosmos DB / SQL** – State and projections
- **Azure OpenAI / Document Intelligence** – AI extraction and categorization
- **Application Insights** – Telemetry and observability

---

## 🏗️ Repository Structure

```
src/
├── LedgerFlow.Api # HTTP entry point
├── LedgerFlow.Ingestion # Unified ingestion layer
├── LedgerFlow.AI.Extraction # AI-based data extraction
├── LedgerFlow.AI.Categorization # AI-based categorization
├── LedgerFlow.Transactions # Core finance processing
├── LedgerFlow.Projections # Read models and summaries
└── LedgerFlow.Contracts # Shared event contracts
```

Infrastructure is managed using **Bicep** and lives under `/infra`.

---

## 📦 Event-Driven Design

All communication between components happens through **immutable events**.

Examples:
- `transaction.ingested`
- `transaction.extracted`
- `transaction.categorized`
- `transaction.applied`

Each event:
- Is versioned
- Contains correlation metadata
- Is idempotent-safe

---

## 🤖 AI Pipeline

LedgerFlow uses AI to:
- Extract structured data from receipts and statements
- Automatically categorize transactions
- Provide confidence scores for human review

AI is treated as an **assistive component**, not a source of truth.

Low-confidence results are routed for review or reprocessing.

---

## 📊 Observability & Metrics

LedgerFlow tracks:
- End-to-end transaction processing time
- Failure and retry rates per pipeline stage
- DLQ volume and reprocessing success
- Business metrics (spending by category, monthly flow)

---

## 🔐 Reliability & Safety

- Idempotent message handling
- Dead-letter queues with reprocessing support
- Schema versioning for events
- Eventual consistency with auditability

---

## 🛣️ Roadmap

- Budget limits and alerts
- Recurring transaction detection
- Anomaly detection via AI
- Open Banking integrations
- Web dashboard

---

## 📄 License

This project is licensed under the **MIT License**.
