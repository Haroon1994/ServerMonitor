import { Server, CheckCircle, XCircle, Activity } from 'lucide-react';
import type { DashboardDto } from '../types';

export function StatsCards({ data }: { data: DashboardDto }) {
  const cards = [
    { label: 'Total Servers', value: data.totalServers, icon: Server, color: 'text-blue-400', bg: 'bg-blue-400/10' },
    { label: 'Servers Up', value: data.serversUp, icon: CheckCircle, color: 'text-emerald-400', bg: 'bg-emerald-400/10' },
    { label: 'Servers Down', value: data.serversDown, icon: XCircle, color: 'text-red-400', bg: 'bg-red-400/10' },
    { label: 'Uptime (24h)', value: `${data.uptimePercentage}%`, icon: Activity, color: 'text-purple-400', bg: 'bg-purple-400/10' },
  ];

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
      {cards.map(c => (
        <div key={c.label} className="bg-gray-800/50 border border-gray-700/50 rounded-xl p-5">
          <div className="flex items-center justify-between mb-3">
            <span className="text-sm text-gray-400">{c.label}</span>
            <div className={`${c.bg} p-2 rounded-lg`}>
              <c.icon className={`w-5 h-5 ${c.color}`} />
            </div>
          </div>
          <p className="text-3xl font-bold text-white">{c.value}</p>
        </div>
      ))}
    </div>
  );
}
