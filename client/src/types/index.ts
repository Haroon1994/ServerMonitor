export enum ServerStatus {
  Unknown = 0,
  Up = 1,
  Down = 2,
}

export interface ServerDto {
  id: string;
  name: string;
  url: string;
  checkIntervalSeconds: number;
  status: ServerStatus;
  lastResponseTimeMs: number | null;
  lastCheckedAt: string | null;
  createdAt: string;
  isActive: boolean;
}

export interface CreateServerDto {
  name: string;
  url: string;
  checkIntervalSeconds?: number;
}

export interface HealthCheckDto {
  id: string;
  serverId: string;
  status: ServerStatus;
  responseTimeMs: number | null;
  errorMessage: string | null;
  checkedAt: string;
}

export interface IncidentDto {
  id: string;
  serverId: string;
  serverName: string;
  startedAt: string;
  resolvedAt: string | null;
  errorMessage: string | null;
}

export interface DashboardDto {
  totalServers: number;
  serversUp: number;
  serversDown: number;
  uptimePercentage: number;
  recentIncidents: IncidentDto[];
  servers: ServerDto[];
}
