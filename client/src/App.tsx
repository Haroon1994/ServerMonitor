import { useCallback, useEffect, useState } from 'react';
import { Activity } from 'lucide-react';
import { getDashboard, deleteServer as apiDeleteServer } from './api/client';
import { useSignalR } from './hooks/useSignalR';
import { StatsCards } from './components/StatsCards';
import { ServerTable } from './components/ServerTable';
import { AddServerForm } from './components/AddServerForm';
import { ResponseChart } from './components/ResponseChart';
import { IncidentList } from './components/IncidentList';
import type { DashboardDto, HealthCheckDto, ServerDto } from './types';

export default function App() {
  const [dashboard, setDashboard] = useState<DashboardDto | null>(null);
  const [selectedServer, setSelectedServer] = useState<ServerDto | null>(null);
  const [realtimeChecks, setRealtimeChecks] = useState<HealthCheckDto[]>([]);

  const loadDashboard = useCallback(() => {
    getDashboard().then(setDashboard);
  }, []);

  useEffect(() => { loadDashboard(); }, [loadDashboard]);

  useSignalR({
    onHealthCheck: (check) => {
      setRealtimeChecks(prev => [...prev.slice(-100), check]);
    },
    onServerStatus: (server) => {
      setDashboard(prev => {
        if (!prev) return prev;
        const servers = prev.servers.map(s => s.id === server.id ? server : s);
        const up = servers.filter(s => s.status === 1).length;
        const down = servers.filter(s => s.status === 2).length;
        return { ...prev, servers, serversUp: up, serversDown: down };
      });
      setSelectedServer(prev => prev?.id === server.id ? server : prev);
    },
    onIncidentCreated: (incident) => {
      setDashboard(prev => prev ? {
        ...prev,
        recentIncidents: [incident, ...prev.recentIncidents].slice(0, 10),
      } : prev);
    },
    onIncidentResolved: (incident) => {
      setDashboard(prev => {
        if (!prev) return prev;
        const recentIncidents = prev.recentIncidents.map(i =>
          i.id === incident.id ? incident : i
        );
        return { ...prev, recentIncidents };
      });
    },
  });

  const handleDelete = async (id: string) => {
    await apiDeleteServer(id);
    if (selectedServer?.id === id) setSelectedServer(null);
    loadDashboard();
  };

  const handleAdded = (server: ServerDto) => {
    setDashboard(prev => prev ? {
      ...prev,
      totalServers: prev.totalServers + 1,
      servers: [...prev.servers, server],
    } : prev);
  };

  if (!dashboard) {
    return (
      <div className="min-h-screen bg-gray-950 flex items-center justify-center">
        <div className="animate-spin text-blue-500">
          <Activity className="w-8 h-8" />
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-950 text-gray-100">
      <header className="border-b border-gray-800">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 py-4 flex items-center gap-3">
          <div className="bg-blue-600 p-2 rounded-lg">
            <Activity className="w-6 h-6 text-white" />
          </div>
          <div>
            <h1 className="text-xl font-bold text-white">Server Monitor</h1>
            <p className="text-xs text-gray-500">Real-time infrastructure monitoring</p>
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 sm:px-6 py-6 space-y-6">
        <StatsCards data={dashboard} />

        <AddServerForm onAdded={handleAdded} />

        {selectedServer && (
          <ResponseChart
            server={selectedServer}
            realtimeChecks={realtimeChecks}
            onClose={() => setSelectedServer(null)}
          />
        )}

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2">
            <ServerTable
              servers={dashboard.servers}
              onDelete={handleDelete}
              onSelect={setSelectedServer}
            />
          </div>
          <div>
            <IncidentList incidents={dashboard.recentIncidents} />
          </div>
        </div>
      </main>
    </div>
  );
}
