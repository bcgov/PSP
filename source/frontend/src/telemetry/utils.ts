import { metrics } from '@opentelemetry/api';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { Resource, ResourceAttributes } from '@opentelemetry/resources';
import { MeterProvider, PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import {
  ATTR_BROWSER_LANGUAGE,
  ATTR_DEPLOYMENT_ENVIRONMENT_NAME,
  ATTR_SERVICE_NAME,
  ATTR_SERVICE_VERSION,
} from '@opentelemetry/semantic-conventions/incubating';

import { TelemetryConfig } from './config';

export const isBrowserEnvironment = () => {
  return typeof window !== 'undefined';
};

export const isNodeEnvironment = () => {
  return typeof process !== 'undefined' && process.release && process.release.name === 'node';
};

export const isBlocked = (uri: string, config: TelemetryConfig) => {
  const blockList = [...(config.denyUrls ?? []), config.otlpEndpoint];
  return blockList.findIndex(blocked => uri.includes(blocked)) >= 0;
};

// List of meters in the application: e.g. "network", "webvitals", "app", etc
export const NETWORK_METER = 'network-meter';

export const registerMeterProvider = (
  config: TelemetryConfig,
  extraAttributes?: ResourceAttributes,
) => {
  // This is common metadata sent with every metric measurement
  const resource = new Resource({
    [ATTR_SERVICE_NAME]: config?.name,
    [ATTR_SERVICE_VERSION]: config?.appVersion,
    [ATTR_DEPLOYMENT_ENVIRONMENT_NAME]: config?.environment,
    [ATTR_BROWSER_LANGUAGE]: window.navigator.language,
    'browser.width': window.screen.width,
    'browser.height': window.screen.height,
    'user_agent.original': window.navigator.userAgent,
    ...extraAttributes,
  });
  const metricExporter = new OTLPMetricExporter({
    url: new URL('/v1/metrics', config.otlpEndpoint).href,
  });

  const meterProvider = new MeterProvider({
    resource: resource,
    readers: [
      new PeriodicExportingMetricReader({
        exporter: metricExporter,
        exportIntervalMillis: config?.exportInterval || 30_000, // export metrics every 30 seconds by default
      }),
    ],
  });

  // set this MeterProvider to be global to the app being instrumented.
  metrics.setGlobalMeterProvider(meterProvider);
};
