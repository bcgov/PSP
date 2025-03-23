export interface TelemetryConfig {
  name?: string;
  appVersion?: string;
  environment?: string;
  otlpEndpoint?: string;
  denyUrls?: string[];
  debug?: boolean;
}
