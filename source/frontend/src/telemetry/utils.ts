import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { Resource, ResourceAttributes } from '@opentelemetry/resources';
import { MeterProvider, PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import {
  ATTR_DEPLOYMENT_ENVIRONMENT_NAME,
  ATTR_SERVICE_NAME,
} from '@opentelemetry/semantic-conventions/incubating';

import { MetricsConfig } from './config';

export const isBrowserEnvironment = () => {
  return typeof window !== 'undefined';
};

export const isNodeEnvironment = () => {
  return typeof process !== 'undefined' && process.release && process.release.name === 'node';
};

export enum MeterKind {
  Network = 'network',
}

export const isBlocked = (uri: string, config: MetricsConfig) => {
  const blockList = [...(config.urlBlocklist ?? []), config.otlpEndpoint];
  return blockList.findIndex(blocked => uri.includes(blocked)) >= 0;
};

export const makeMeterName = (kind: MeterKind) => {
  return `${kind}-meter`;
};

export const registerMeterProvider = (
  kind: MeterKind,
  config: MetricsConfig,
  attributes?: ResourceAttributes,
) => {
  const resource = new Resource({
    [ATTR_SERVICE_NAME]: config?.name,
    [ATTR_DEPLOYMENT_ENVIRONMENT_NAME]: config?.environment,
    ...attributes,
  });
  const metricExporter = new OTLPMetricExporter({
    url: new URL('/v1/metrics', config.otlpEndpoint).href,
  });

  const meterProvider = new MeterProvider({
    resource: resource,
    readers: [
      new PeriodicExportingMetricReader({
        exporter: metricExporter,
        exportIntervalMillis: 10_000,
      }),
    ],
  });

  return meterProvider.getMeter(makeMeterName(kind));
};
