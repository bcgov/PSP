import { exists } from '@/utils';

import { TelemetryConfig } from './config';
import { UserAPI } from './users/UserAPI';
import { registerMeterProvider, registerTracerProvider } from './utils';

// Main entry-point to configure the collection of application telemetry
export const initializeTelemetry = (config: TelemetryConfig) => {
  try {
    if (!exists(config)) {
      throw Error('[ERR] No metrics configuration provided, it will not be initialized.');
    }

    if (!exists(config.otlpEndpoint)) {
      throw Error('[ERR] Invalid metrics endpoint provided, it will not be initialized.');
    }

    registerTracerProvider(config);
    registerMeterProvider(config);
  } catch (error) {
    if (config.debug) {
      console.error(error);
    }
  }
};

// Global provider to capture logged-in user in traces
export const user = UserAPI.getInstance();
