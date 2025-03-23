import { exists } from '@/utils';

import { TelemetryConfig } from './config';
import { registerNetworkMetrics } from './metrics';
import { registerMeterProvider } from './utils';

const configureTelemetry = (config: TelemetryConfig) => {
  try {
    if (!exists(config)) {
      throw Error('[ERR] No metrics configuration provided, it will not be initialized.');
    }

    if (!exists(config.otlpEndpoint)) {
      throw Error('[ERR] Invalid metrics endpoint provided, it will not be initialized.');
    }

    registerBrowserMetrics(config);
  } catch (error) {
    if (config.debug) {
      console.error(error);
    }
  }
};

const registerBrowserMetrics = (config: TelemetryConfig) => {
  // First we need to register global telemetry configuration
  registerMeterProvider(config);
  // Then we can register various meters to collect metrics/measurements
  registerNetworkMetrics(config);
};

const Telemetry = { init: configureTelemetry };
export default Telemetry;
