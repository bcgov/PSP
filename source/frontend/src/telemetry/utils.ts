import { metrics, trace } from '@opentelemetry/api';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { browserDetector } from '@opentelemetry/opentelemetry-browser-detector';
import { detectResourcesSync, Resource, ResourceAttributes } from '@opentelemetry/resources';
import { MeterProvider, PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import {
  BatchSpanProcessor,
  ConsoleSpanExporter,
  SimpleSpanProcessor,
  SpanProcessor,
  WebTracerProvider,
} from '@opentelemetry/sdk-trace-web';
import { ATTR_SERVICE_NAME, ATTR_SERVICE_VERSION } from '@opentelemetry/semantic-conventions';
import { ATTR_DEPLOYMENT_ENVIRONMENT_NAME } from '@opentelemetry/semantic-conventions/incubating';
import { v4 as uuidv4 } from 'uuid';

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

const makeResource = (config: TelemetryConfig, extraAttributes?: ResourceAttributes) => {
  let resource = new Resource({
    [ATTR_SERVICE_NAME]: config?.name,
    [ATTR_SERVICE_VERSION]: config?.appVersion,
    [ATTR_DEPLOYMENT_ENVIRONMENT_NAME]: config?.environment,
    'session.instance.id': uuidv4(),
    'browser.width': window.screen.width,
    'browser.height': window.screen.height,
    ...extraAttributes,
  });

  const detectedResources = detectResourcesSync({ detectors: [browserDetector] });
  resource = resource.merge(detectedResources);
  return resource;
};

export const registerMeterProvider = (
  config: TelemetryConfig,
  extraAttributes?: ResourceAttributes,
) => {
  // This is common metadata sent with every metric measurement
  const resource = makeResource(config, extraAttributes);
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

export const registerTracerProvider = (
  config: TelemetryConfig,
  extraAttributes?: ResourceAttributes,
) => {
  // This is common metadata sent with every trace
  const resource = makeResource(config, extraAttributes);
  const exporter = new OTLPTraceExporter({
    url: new URL('/v1/traces', config.otlpEndpoint).href,
  });

  const processors: SpanProcessor[] = [];
  if (config.debug) {
    processors.push(new SimpleSpanProcessor(new ConsoleSpanExporter()));
  }
  processors.push(
    new BatchSpanProcessor(exporter, { scheduledDelayMillis: config?.exportInterval || 5000 }),
  );

  const provider = new WebTracerProvider({
    resource,
    spanProcessors: [...processors],
  });

  provider.register({
    // Changing default contextManager to use ZoneContextManager - to support asynchronous operations
    contextManager: new ZoneContextManager(),
  });

  // set this TracerProvider to be global to the app
  trace.setGlobalTracerProvider(provider);
};
