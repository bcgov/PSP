import { TelemetrySettings } from './config';

export const isBrowserEnvironment = () => {
  return typeof window !== 'undefined';
};

export const isNodeEnvironment = () => {
  return typeof process !== 'undefined' && process.release && process.release.name === 'node';
};

export const isBlocked = (uri: string, config: TelemetrySettings) => {
  const blockList = [...(config.denyUrls ?? []), config.collectorUrl];
  return blockList.findIndex(blocked => uri.includes(blocked)) >= 0;
};

// List of meters in the application: e.g. "network", "webvitals", "app", etc
export const NETWORK_METER = 'network-meter';
