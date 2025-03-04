export interface MetricsConfig {
  name?: string;
  environment?: string;
  otlpEndpoint?: string;
  debug?: boolean;
  urlBlocklist?: string[];
}
