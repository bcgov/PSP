import { metrics } from '@opentelemetry/api';
import {
  ATTR_HTTP_RESPONSE_STATUS_CODE,
  METRIC_HTTP_CLIENT_REQUEST_DURATION,
  METRIC_HTTP_CLIENT_RESPONSE_BODY_SIZE,
} from '@opentelemetry/semantic-conventions/incubating';

import { exists, isNumber } from '@/utils';

import { TelemetryConfig } from './config';
import { isBlocked, isBrowserEnvironment, NETWORK_METER } from './utils';

// These values are lifted from suggested defaults in open-telemetry documentation:
// https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/metrics/sdk.md#explicit-bucket-histogram-aggregation
const defaultHistogramBucketsInSeconds = [
  0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10,
];

export const registerNetworkMetrics = (config: TelemetryConfig) => {
  if (!isBrowserEnvironment() || !window.performance) {
    return;
  }

  if (config.debug) {
    console.info('[INFO] Registering network request metrics');
  }

  const meter = metrics.getMeter(NETWORK_METER);

  // measure the time it takes to make an XHR request
  const buckets =
    exists(config.histogramBuckets) && config.histogramBuckets.length > 0
      ? [...config.histogramBuckets]
      : [...defaultHistogramBucketsInSeconds];

  const timeSpentMetric = meter.createHistogram(METRIC_HTTP_CLIENT_REQUEST_DURATION, {
    description: 'The duration of an outgoing HTTP request.',
    unit: 's',
    advice: {
      explicitBucketBoundaries: buckets,
    },
  });

  // collect metrics regarding the payload size of network requests
  const sizeMetric = meter.createGauge(METRIC_HTTP_CLIENT_RESPONSE_BODY_SIZE, {
    description: 'Size of network response',
    unit: 'byte',
  });

  // Use PerformanceObserver Browser API to track these things
  // https://developer.mozilla.org/en-US/docs/Web/API/PerformanceObserver
  const observer = new PerformanceObserver(list => {
    const entries = list.getEntriesByType('resource');
    entries.forEach((entry: PerformanceResourceTiming) => {
      if (
        'initiatorType' in entry &&
        (entry.initiatorType === 'xmlhttprequest' || entry.initiatorType === 'fetch')
      ) {
        // Exit early if entry.name is null
        if (!exists(entry.name) || !isNumber(entry.duration)) {
          return;
        }

        const uri = new URL(entry.name);
        uri.search = '';
        const sanitizedUri = uri.toString();

        // do not collect metrics from blacklisted sites
        if (isBlocked(sanitizedUri, config)) {
          return;
        }

        // convert from ms to seconds
        const timeSpent = entry.duration / 1000;
        const size: number = entry.transferSize || entry.encodedBodySize || 0;
        const statusCode: number = (entry as any).responseStatus;

        const attributes = {
          uri: sanitizedUri,
          route: window.location.pathname,
          environment: config.environment,
          [ATTR_HTTP_RESPONSE_STATUS_CODE]: statusCode,
        };

        timeSpentMetric.record(timeSpent, attributes);
        if (size !== 0) {
          sizeMetric.record(size, attributes);
        }

        if (config.debug) {
          console.info({ uri, timeSpent, size, attributes });
        }
      }
    });
  });

  observer.observe({ type: 'resource', buffered: true });
};
