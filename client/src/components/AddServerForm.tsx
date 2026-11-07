import { useState } from 'react';
import { Plus } from 'lucide-react';
import { createServer } from '../api/client';
import type { ServerDto } from '../types';

export function AddServerForm({ onAdded }: { onAdded: (s: ServerDto) => void }) {
  const [name, setName] = useState('');
  const [url, setUrl] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim() || !url.trim()) return;
    setLoading(true);
    try {
      const server = await createServer({ name: name.trim(), url: url.trim() });
      onAdded(server);
      setName('');
      setUrl('');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col sm:flex-row gap-3">
      <input
        type="text"
        placeholder="Server name"
        value={name}
        onChange={e => setName(e.target.value)}
        className="flex-1 bg-gray-800/50 border border-gray-700/50 rounded-lg px-4 py-2.5 text-white placeholder-gray-500 focus:outline-none focus:border-blue-500 transition-colors"
      />
      <input
        type="url"
        placeholder="https://example.com"
        value={url}
        onChange={e => setUrl(e.target.value)}
        className="flex-1 bg-gray-800/50 border border-gray-700/50 rounded-lg px-4 py-2.5 text-white placeholder-gray-500 focus:outline-none focus:border-blue-500 transition-colors"
      />
      <button
        type="submit"
        disabled={loading}
        className="flex items-center gap-2 bg-blue-600 hover:bg-blue-500 disabled:opacity-50 text-white font-medium px-5 py-2.5 rounded-lg transition-colors"
      >
        <Plus className="w-4 h-4" />
        Add
      </button>
    </form>
  );
}
