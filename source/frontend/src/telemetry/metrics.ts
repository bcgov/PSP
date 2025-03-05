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
  const timeSpentMetric = meter.createHistogram('network_request_time_spent', {
    description: 'Time Spent on Request',
    unit: 'milliseconds',
  });

  // collect metrics regarding the payload size of network requests
  const sizeMetric = meter.createHistogram('network_request_size', {
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

        // do not collect metrics from blacklisted sites
        if (!isBlocked(uri, config)) {
          const attributes = {
            uri,
            route: window.location.pathname,
            environment: config.environment,
          };

          timeSpentMetric.record(timeSpent, attributes);
          if (size !== 0) {
            sizeMetric.record(size, attributes);
          }
        }

        if (config.debug) {
          console.info({ uri, timeSpent, size });
        }
      }
    });
  });

  observer.observe({ type: 'resource', buffered: true });
};
