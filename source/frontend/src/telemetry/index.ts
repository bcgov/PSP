import { exists } from '@/utils';

import { MetricsConfig } from './config';
import { registerNetworkMetrics } from './metrics';
import { registerMeterProvider } from './utils';

export const configureTelemetry = (config: MetricsConfig) => {
  try {
    if (!exists(config)) {
      throw Error('[ERR] No metrics configuration provided, it will not be initialized.');
    }

    if (!exists(config.otlpEndpoint)) {
      throw Error('[ERR] Invalid metrics endpoint provided, it will not be initialized.');
    }
    // First need to register global telemetry configuration
    registerMeterProvider(config);

    // Then can register various meters to collect metrics/measurements
    registerNetworkMetrics(config);
  } catch (error) {
    if (config.debug) {
      console.error(error);
    }
  }
};
