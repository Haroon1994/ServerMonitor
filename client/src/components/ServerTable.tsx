import { Trash2 } from 'lucide-react';
import { ServerStatus, type ServerDto } from '../types';

function StatusBadge({ status }: { status: ServerStatus }) {
  if (status === ServerStatus.Up)
    return <span className="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium bg-emerald-400/10 text-emerald-400">
      <span className="w-1.5 h-1.5 rounded-full bg-emerald-400 animate-pulse" /> Up
    </span>;
  if (status === ServerStatus.Down)
    return <span className="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium bg-red-400/10 text-red-400">
      <span className="w-1.5 h-1.5 rounded-full bg-red-400 animate-pulse" /> Down
    </span>;
  return <span className="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium bg-gray-400/10 text-gray-400">
    <span className="w-1.5 h-1.5 rounded-full bg-gray-400" /> Unknown
  </span>;
}

interface Props {
  servers: ServerDto[];
  onDelete: (id: string) => void;
  onSelect: (server: ServerDto) => void;
}

export function ServerTable({ servers, onDelete, onSelect }: Props) {
  return (
    <div className="bg-gray-800/50 border border-gray-700/50 rounded-xl overflow-hidden">
      <div className="px-5 py-4 border-b border-gray-700/50">
        <h2 className="text-lg font-semibold text-white">Monitored Servers</h2>
      </div>
      <div className="overflow-x-auto">
        <table className="w-full">
          <thead>
            <tr className="text-left text-xs text-gray-400 uppercase tracking-wider">
              <th className="px-5 py-3">Name</th>
              <th className="px-5 py-3">URL</th>
              <th className="px-5 py-3">Status</th>
              <th className="px-5 py-3">Response</th>
              <th className="px-5 py-3">Last Check</th>
              <th className="px-5 py-3"></th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-700/50">
            {servers.map(s => (
              <tr
                key={s.id}
                className="hover:bg-gray-700/30 cursor-pointer transition-colors"
                onClick={() => onSelect(s)}
              >
                <td className="px-5 py-3 text-white font-medium">{s.name}</td>
                <td className="px-5 py-3 text-gray-400 text-sm truncate max-w-[200px]">{s.url}</td>
                <td className="px-5 py-3"><StatusBadge status={s.status} /></td>
                <td className="px-5 py-3 text-gray-300 text-sm">
                  {s.lastResponseTimeMs !== null ? `${s.lastResponseTimeMs}ms` : '—'}
                </td>
                <td className="px-5 py-3 text-gray-400 text-sm">
                  {s.lastCheckedAt ? new Date(s.lastCheckedAt).toLocaleTimeString() : 'Never'}
                </td>
                <td className="px-5 py-3">
                  <button
                    onClick={(e) => { e.stopPropagation(); onDelete(s.id); }}
                    className="text-gray-500 hover:text-red-400 transition-colors"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                </td>
              </tr>
            ))}
            {servers.length === 0 && (
              <tr>
                <td colSpan={6} className="px-5 py-8 text-center text-gray-500">
                  No servers monitored yet. Add one above.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
