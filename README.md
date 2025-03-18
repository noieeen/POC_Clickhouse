# Monitoring System Architecture

## Overview
This repository contains a monitoring system setup for .NET microservices using **OpenTelemetry, Jaeger, Prometheus, Vector, ClickHouse, and Grafana**.

## Architecture Diagram
```mermaid
graph LR;
    
    subgraph "Microservices"
        A[ServiceA ASP.NET Core]
        B[ServiceB ASP.NET Core]
    end

    subgraph "Telemetry Collection"
        C[OpenTelemetry Collector]
    end

    subgraph "Logging System"
        D[Vector]
        E[ClickHouse]
    end

    subgraph "Tracing System"
        F[Jaeger v2]
    end

    subgraph "Metrics System"
        G[Prometheus]
    end

    subgraph "Visualization & Monitoring"
        H[Grafana]
    end

    A -->|Telemetry Data| C;
    B -->|Telemetry Data| C;

    C -->|Logs| D;
    D -->|Store Logs| E;
    E -->|Query Logs| H;

    C -->|Traces| F;
    F -->|Trace Visualization| H;

    C -->|Metrics| G;
    G -->|Metrics Visualization| H;
```

## Components

### Microservices
- **ServiceA & ServiceB**: .NET Core applications sending telemetry data.

### Telemetry Collection
- **OpenTelemetry Collector**: Collects logs, traces, and metrics, then forwards them.

### Logging System
- **Vector**: Aggregates logs and sends them to ClickHouse.
- **ClickHouse**: Stores structured logs efficiently.

### Tracing System
- **Jaeger v2**: Stores traces for distributed request tracking.

### Metrics System
- **Prometheus**: Collects and stores metrics data.

### Visualization & Monitoring
- **Grafana**: Central dashboard for monitoring logs, traces, and metrics.

## Getting Started
### Running the System
Ensure you have Docker installed, then run:
```sh
docker-compose up -d
```

### Accessing Services
- **Grafana**: [http://localhost:3000](http://localhost:3000)
- **Jaeger UI**: [http://localhost:16686](http://localhost:16686)
- **Prometheus**: [http://localhost:9090](http://localhost:9090)

## Contributing
Feel free to submit pull requests or issues for improvements!
