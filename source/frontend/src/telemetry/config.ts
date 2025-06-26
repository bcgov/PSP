// The configuration for browser telemetry (metrics and logs)
export interface TelemetryConfig {
  // by default the service name is set to 'frontend' - helps finding traces in the trace UI dashboard
  name?: string;
  appVersion?: string;
  // set this to match the deployed environment (dev, test, uat, prod) or set to local for local development
  environment?: string;
  // the URL to the open-telemetry collector
  otlpEndpoint?: string;
  // a list of URLs to ignore for traces and metrics
  denyUrls?: string[];
  // if true, it will output extra information to the console
  debug?: boolean;
  // how often to send traces and metrics back to the collector - defaults to 30 seconds
  exportInterval?: number;
  // the default buckets to apply to histogram metrics
  histogramBuckets?: number[];
}

export const defaultHistogramBuckets = [
  0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10,
];
