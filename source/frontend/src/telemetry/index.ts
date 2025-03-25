import { exists } from '@/utils';

import { TelemetryConfig } from './config';
import { registerNetworkMetrics } from './metrics';
import { getTracer } from './traces';
import { registerMeterProvider, registerTracerProvider } from './utils';

export const initializeTelemetry = (config: TelemetryConfig) => {
  try {
    if (!exists(config)) {
      throw Error('[ERR] No metrics configuration provided, it will not be initialized.');
    }

    if (!exists(config.otlpEndpoint)) {
      throw Error('[ERR] Invalid metrics endpoint provided, it will not be initialized.');
    }

    registerMetrics(config);
    registerTraces(config);
  } catch (error) {
    if (config.debug) {
      console.error(error);
    }
  }
};

const registerMetrics = (config: TelemetryConfig) => {
  // First we need to register global telemetry configuration
  registerMeterProvider(config);

  // Then we can register various meters to collect metrics/measurements
  registerNetworkMetrics(config);
};

const registerTraces = (config: TelemetryConfig) => {
  registerTracerProvider(config);

  // start span when navigating to page
  getTracer().startActiveSpan('document_load', span => {
    span.setAttribute('page.url', window.location.href);
    window.onload = () => {
      // once page is loaded, end the span
      span.end();
    };
  });
};
