import { exists } from '@/utils';

import { MetricsConfig } from './config';
import { isBlocked, isBrowserEnvironment, MeterKind, registerMeterProvider } from './utils';

export const registerNetworkMetrics = (config: MetricsConfig) => {
  if (!isBrowserEnvironment() || !window.performance) {
    return;
  }

  if (config.debug) {
    console.info('Registering Network Request metrics');
  }

  const meter = registerMeterProvider(MeterKind.Network, config);

  // measure the time it takes to make an XHR request
  const timeSpentMetric = meter.createGauge('network_request_time_spent', {
    description: 'Time Spent on Request',
    unit: 'milliseconds',
  });

  // collect metrics regarding the payload size of network requests
  const sizeMetric = meter.createGauge('network_request_size', {
    description: 'Size of Network Request',
    unit: 'bytes',
  });

  // Use PerformanceObserver Browser API to track these things
  // https://developer.mozilla.org/en-US/docs/Web/API/PerformanceObserver
  const observer = new PerformanceObserver(list => {
    const entries = list.getEntries();
    entries.forEach(entry => {
      if (
        'initiatorType' in entry &&
        (entry.initiatorType === 'xmlhttprequest' || entry.initiatorType === 'fetch')
      ) {
        const uri = entry.name;
        const timeSpent = entry.duration;
        const size: number =
          ('transferSize' in entry ? (entry.transferSize as number) : 0) ||
          ('encodedBodySize' in entry ? (entry.encodedBodySize as number) : 0);

        if (!isBlocked(uri, config)) {
          timeSpentMetric.record(timeSpent, {
            uri,
            route: window.location.pathname,
          });
        }

        if (size !== 0) {
          sizeMetric.record(size, { uri, route: window.location.pathname });
        }

        if (config.debug) {
          console.info({ uri, timeSpent, size });
        }
      }
    });
  });

  observer.observe({ type: 'resource', buffered: true });
};

export const configureTelemetry = (config: MetricsConfig) => {
  try {
    if (!exists(config)) {
      throw Error('[ERR] No metrics configuration provided, it will not be initialized.');
    }

    if (!exists(config.otlpEndpoint)) {
      throw Error('[ERR] Invalid metrics endpoint provided, it will not be initialized.');
    }

    registerNetworkMetrics(config);
  } catch (error) {
    if (config.debug) {
      console.error(error);
    }
  }
};
