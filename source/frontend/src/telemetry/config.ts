import { Attributes } from '@opentelemetry/api';

// The configuration for browser telemetry (metrics and logs)
export interface TelemetrySettings {
  appName: string;
  appVersion?: string;
  // Set this to match the deployed environment (dev, test, uat, prod) or set to local for local development
  environment?: string;
  // The URL to the open-telemetry collector service that will receive the telemetry data
  collectorUrl?: string;
  // a list of URLs to ignore for traces and metrics
  denyUrls?: string[];
  // If true, it will output extra information to the console
  debug?: boolean;
  // Metric export interval in ms (default: 30,000)
  metricExportIntervalMs?: number;
  // Default buckets to apply to histogram metrics
  histogramBuckets?: number[];
  // Additional resource atttributes to add to all telemetry data (traces and metrics)
  resourceAttributes?: Attributes;
}

export const defaultHistogramBuckets = [
  0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10,
];
