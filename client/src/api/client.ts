import axios from 'axios';
import type { CreateServerDto, DashboardDto, HealthCheckDto, ServerDto } from '../types';

const api = axios.create({ baseURL: '/api' });

export const getDashboard = () =>
  api.get<DashboardDto>('/dashboard').then(r => r.data);

export const getServers = () =>
  api.get<ServerDto[]>('/servers').then(r => r.data);

export const createServer = (dto: CreateServerDto) =>
  api.post<ServerDto>('/servers', dto).then(r => r.data);

export const deleteServer = (id: string) =>
  api.delete(`/servers/${id}`);

export const getHealthChecks = (serverId: string, limit = 50) =>
  api.get<HealthCheckDto[]>(`/healthchecks/server/${serverId}?limit=${limit}`).then(r => r.data);
