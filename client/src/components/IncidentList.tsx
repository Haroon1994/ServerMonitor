import { AlertTriangle, CheckCircle } from 'lucide-react';
import type { IncidentDto } from '../types';

export function IncidentList({ incidents }: { incidents: IncidentDto[] }) {
  return (
    <div className="bg-gray-800/50 border border-gray-700/50 rounded-xl overflow-hidden">
      <div className="px-5 py-4 border-b border-gray-700/50">
        <h2 className="text-lg font-semibold text-white">Recent Incidents</h2>
      </div>
      <div className="divide-y divide-gray-700/50">
        {incidents.map(i => (
          <div key={i.id} className="px-5 py-3 flex items-center gap-3">
            {i.resolvedAt ? (
              <CheckCircle className="w-5 h-5 text-emerald-400 shrink-0" />
            ) : (
              <AlertTriangle className="w-5 h-5 text-red-400 shrink-0 animate-pulse" />
            )}
            <div className="flex-1 min-w-0">
              <p className="text-white text-sm font-medium">{i.serverName}</p>
              <p className="text-gray-400 text-xs truncate">{i.errorMessage ?? 'No details'}</p>
            </div>
            <div className="text-right shrink-0">
              <p className="text-xs text-gray-400">
                {new Date(i.startedAt).toLocaleString()}
              </p>
              {i.resolvedAt ? (
                <p className="text-xs text-emerald-400">Resolved</p>
              ) : (
                <p className="text-xs text-red-400">Ongoing</p>
              )}
            </div>
          </div>
        ))}
        {incidents.length === 0 && (
          <div className="px-5 py-8 text-center text-gray-500">
            No incidents recorded.
          </div>
        )}
      </div>
    </div>
  );
}
