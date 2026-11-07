# Server Monitor

Real-time server monitoring dashboard built with .NET 10, React, SignalR, and PostgreSQL.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![React](https://img.shields.io/badge/React-19-61DAFB?logo=react)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?logo=postgresql)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-4-06B6D4?logo=tailwindcss)

## Features

- **Server CRUD** — Add, edit, and remove servers to monitor
- **Automated Health Checks** — Background service pings servers at configurable intervals
- **Real-time Dashboard** — Live updates via SignalR (no page refresh needed)
- **Response Time Charts** — Recharts area charts showing response time history
- **Incident Tracking** — Automatic incident creation when servers go down, auto-resolved on recovery
- **Dark Modern UI** — Tailwind CSS dark theme with responsive layout

## Architecture

```
ServerMonitor.Domain          → Entities, Enums (no dependencies)
ServerMonitor.Application     → DTOs, Interfaces (depends on Domain)
ServerMonitor.Infrastructure  → EF Core DbContext, Services (depends on Application)
ServerMonitor.API             → Controllers, SignalR Hub, Program.cs (depends on all)
client/                       → React + TypeScript + Vite frontend
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker](https://www.docker.com/)

## Getting Started

### 1. Start the database

```bash
docker compose up -d
```

This starts PostgreSQL on port **5434** with database `servermonitor`.

### 2. Run migrations

```bash
cd src/ServerMonitor.API
dotnet ef database update
```

### 3. Start the API

```bash
cd src/ServerMonitor.API
dotnet run --urls "http://localhost:5100"
```

The API runs at `http://localhost:5100` with Swagger UI at `/swagger`.

### 4. Start the frontend

```bash
cd client
npm install
npm run dev
```

The dashboard opens at `http://localhost:5173` and proxies API calls to the backend.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/dashboard` | Aggregated dashboard data |
| GET | `/api/servers` | List all servers |
| POST | `/api/servers` | Add a server |
| PUT | `/api/servers/{id}` | Update a server |
| DELETE | `/api/servers/{id}` | Remove a server |
| GET | `/api/healthchecks/server/{id}` | Health check history |
| POST | `/api/healthchecks/server/{id}` | Trigger a manual check |
| GET | `/api/incidents` | Recent incidents |

## SignalR Events

Connect to `/hubs/monitor` to receive:

| Event | Payload | When |
|-------|---------|------|
| `HealthCheckUpdated` | `HealthCheckDto` | Every health check |
| `ServerStatusChanged` | `ServerDto` | Server goes up/down |
| `IncidentCreated` | `IncidentDto` | New incident |
| `IncidentResolved` | `IncidentDto` | Incident resolved |

## Tech Stack

**Backend:** .NET 10, ASP.NET Core, Entity Framework Core, SignalR, PostgreSQL  
**Frontend:** React 19, TypeScript, Vite, Tailwind CSS 4, Recharts, Lucide Icons  
**Infrastructure:** Docker, Docker Compose
