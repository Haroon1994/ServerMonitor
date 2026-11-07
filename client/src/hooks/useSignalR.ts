import { useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import type { HealthCheckDto, IncidentDto, ServerDto } from '../types';

interface SignalRCallbacks {
  onHealthCheck?: (check: HealthCheckDto) => void;
  onServerStatus?: (server: ServerDto) => void;
  onIncidentCreated?: (incident: IncidentDto) => void;
  onIncidentResolved?: (incident: IncidentDto) => void;
}

export function useSignalR(callbacks: SignalRCallbacks) {
  const callbacksRef = useRef(callbacks);
  callbacksRef.current = callbacks;

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/monitor')
      .withAutomaticReconnect()
      .build();

    connection.on('HealthCheckUpdated', (check: HealthCheckDto) =>
      callbacksRef.current.onHealthCheck?.(check));

    connection.on('ServerStatusChanged', (server: ServerDto) =>
      callbacksRef.current.onServerStatus?.(server));

    connection.on('IncidentCreated', (incident: IncidentDto) =>
      callbacksRef.current.onIncidentCreated?.(incident));

    connection.on('IncidentResolved', (incident: IncidentDto) =>
      callbacksRef.current.onIncidentResolved?.(incident));

    connection.start().catch(console.error);

    return () => { connection.stop(); };
  }, []);
}
