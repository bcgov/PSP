import { metrics, trace } from '@opentelemetry/api';
import { W3CTraceContextPropagator } from '@opentelemetry/core';
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
import {
  ATTR_SERVICE_NAME,
  ATTR_SERVICE_VERSION,
  ATTR_USER_AGENT_ORIGINAL,
} from '@opentelemetry/semantic-conventions';
import {
  ATTR_DEPLOYMENT_ENVIRONMENT_NAME,
  ATTR_SERVICE_INSTANCE_ID,
} from '@opentelemetry/semantic-conventions/incubating';
import isAbsoluteUrl from 'is-absolute-url';
import { v4 as uuidv4 } from 'uuid';

import { TelemetryConfig } from './config';
import { BrowserAttributesSpanProcessor } from './traces/BrowserAttributesSpanProcessor';
import { UserInfoSpanProcessor } from './traces/UserInfoSpanProcessor';

export const isBrowserEnvironment = () => {
  return typeof window !== 'undefined';
};

export const isNodeEnvironment = () => {
  return typeof process !== 'undefined' && process.release && process.release.name === 'node';
};

// creates URL and appends query parameters
export const buildUrl = (inputUrl: string, queryParams: Record<string, any> = {}): URL => {
  const baseUrl = window.location.origin;
  const urlInstance = isAbsoluteUrl(inputUrl) ? new URL(inputUrl) : new URL(inputUrl, baseUrl);
  Object.keys(queryParams).forEach(k => {
    if (queryParams[k] !== undefined) {
      urlInstance.searchParams.set(k, queryParams[k]);
    }
  });
  return urlInstance;
};

export const isBlocked = (uri: string, config: TelemetryConfig) => {
  const blockList = [...(config.denyUrls ?? []), config.otlpEndpoint];
  return blockList.findIndex(blocked => uri.includes(blocked)) >= 0;
};

// List of meters in the application: e.g. "network", "webvitals", "app", etc
export const NETWORK_METER = 'network-meter';

const makeResource = (config: TelemetryConfig, extraAttributes?: ResourceAttributes) => {
  const uuid = uuidv4();
  let resource = new Resource({
    [ATTR_DEPLOYMENT_ENVIRONMENT_NAME]: config?.environment,
    [ATTR_SERVICE_NAME]: config?.name,
    [ATTR_SERVICE_VERSION]: config?.appVersion,
    [ATTR_SERVICE_INSTANCE_ID]: uuid,
    [ATTR_USER_AGENT_ORIGINAL]: typeof navigator !== 'undefined' ? navigator.userAgent : '',
    'session.instance.id': uuid,
    'screen.width': window.screen.width,
    'screen.height': window.screen.height,
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
  if (config.debug) {
    console.info('[INFO] Registering metrics provider');
  }

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
        exportIntervalMillis: config?.exportInterval || 30_000, // export metrics every 30s by default
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
  if (config.debug) {
    console.info('[INFO] Registering trace provider');
  }

  // This is common metadata sent with every trace
  const resource = makeResource(config, extraAttributes);
  const exporter = new OTLPTraceExporter({
    url: new URL('v1/traces', config.otlpEndpoint).href,
  });

  const processors: SpanProcessor[] = [];

  if (config.debug) {
    processors.push(new SimpleSpanProcessor(new ConsoleSpanExporter()));
  }

  // use the batch processor for better performance
  processors.push(
    new BatchSpanProcessor(exporter, { scheduledDelayMillis: config?.exportInterval || 5000 }), // export traces every 5s by default
    new BrowserAttributesSpanProcessor(),
    new UserInfoSpanProcessor(),
  );

  const provider = new WebTracerProvider({
    resource,
    spanProcessors: [...processors],
  });

  // set up context propagation
  provider.register({
    propagator: new W3CTraceContextPropagator(),
  });

  // set this TracerProvider to be global to the app
  trace.setGlobalTracerProvider(provider);
};
