import { useEffect, useState } from 'react';
import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { getHealthChecks } from '../api/client';
import type { HealthCheckDto, ServerDto } from '../types';
import { X } from 'lucide-react';

interface Props {
  server: ServerDto;
  realtimeChecks: HealthCheckDto[];
  onClose: () => void;
}

export function ResponseChart({ server, realtimeChecks, onClose }: Props) {
  const [checks, setChecks] = useState<HealthCheckDto[]>([]);

  useEffect(() => {
    getHealthChecks(server.id, 30).then(data => setChecks(data.reverse()));
  }, [server.id]);

  useEffect(() => {
    const latest = realtimeChecks.filter(c => c.serverId === server.id);
    if (latest.length > 0) {
      setChecks(prev => [...prev, ...latest].slice(-30));
    }
  }, [realtimeChecks, server.id]);

  const chartData = checks.map(c => ({
    time: new Date(c.checkedAt).toLocaleTimeString(),
    responseTime: c.responseTimeMs ?? 0,
  }));

  return (
    <div className="bg-gray-800/50 border border-gray-700/50 rounded-xl p-5">
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-lg font-semibold text-white">
          Response Time — {server.name}
        </h2>
        <button onClick={onClose} className="text-gray-500 hover:text-gray-300 transition-colors">
          <X className="w-5 h-5" />
        </button>
      </div>
      {chartData.length === 0 ? (
        <p className="text-gray-500 text-center py-8">No health check data yet. Waiting for first check...</p>
      ) : (
        <ResponsiveContainer width="100%" height={280}>
          <AreaChart data={chartData}>
            <defs>
              <linearGradient id="responseGradient" x1="0" y1="0" x2="0" y2="1">
                <stop offset="0%" stopColor="#3b82f6" stopOpacity={0.3} />
                <stop offset="100%" stopColor="#3b82f6" stopOpacity={0} />
              </linearGradient>
            </defs>
            <CartesianGrid strokeDasharray="3 3" stroke="#374151" />
            <XAxis dataKey="time" stroke="#6b7280" fontSize={12} />
            <YAxis stroke="#6b7280" fontSize={12} unit="ms" />
            <Tooltip
              contentStyle={{ backgroundColor: '#1f2937', border: '1px solid #374151', borderRadius: '8px', color: '#fff' }}
              labelStyle={{ color: '#9ca3af' }}
            />
            <Area type="monotone" dataKey="responseTime" stroke="#3b82f6" strokeWidth={2} fill="url(#responseGradient)" />
          </AreaChart>
        </ResponsiveContainer>
      )}
    </div>
  );
}
