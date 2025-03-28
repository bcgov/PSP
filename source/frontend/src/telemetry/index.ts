import { exists } from '@/utils';

import { TelemetryConfig } from './config';
import { registerMeterProvider, registerTracerProvider } from './utils';

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
