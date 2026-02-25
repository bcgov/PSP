import { defaultHistogramBuckets } from '@/telemetry/config';

import { exists } from '.';
import { stringToNull, stringToNullableBoolean } from './formUtils';

declare global {
  interface Window {
    config: {
      VITE_DISABLE_TELEMERY: string;
      VITE_TELEMERY_DEBUG: string;
      VITE_TELEMERY_ENVIRONMENT: string;
      VITE_TELEMERY_SERVICE_NAME: string;
      VITE_TELEMERY_URL: string;
      VITE_TELEMERY_METRIC_EXPORT_INTERVAL: string;
      VITE_TELEMERY_TRACE_EXPORT_INTERVAL: string;
      VITE_TELEMERY_HISTOGRAM_BUCKETS: string;
    };
  }
}

// Global toggle to disable telemetry, can be set via environment variable VITE_DISABLE_TELEMERY or window.config.VITE_DISABLE_TELEMERY in production.
const DISABLE_TELEMETRY: string =
  window.config?.VITE_DISABLE_TELEMERY || import.meta.env.VITE_DISABLE_TELEMERY || '';

const APP_TELEMETRY_DEBUG: string =
  window.config?.VITE_TELEMERY_DEBUG || import.meta.env.VITE_TELEMERY_DEBUG || '';

const APP_TELEMETRY_ENVIRONMENT: string =
  window.config?.VITE_TELEMERY_ENVIRONMENT || import.meta.env.VITE_TELEMERY_ENVIRONMENT || '';

const APP_NAME: string =
  window.config?.VITE_TELEMERY_SERVICE_NAME || import.meta.env.VITE_TELEMERY_SERVICE_NAME || '';

const APP_VERSION: string = import.meta.env.VITE_PACKAGE_VERSION || '';

const APP_TELEMETRY_URL: string =
  window.config?.VITE_TELEMERY_URL || import.meta.env.VITE_TELEMERY_URL || '';

const APP_TELEMETRY_METRIC_EXPORT_INTERVAL: string =
  window.config?.VITE_TELEMERY_METRIC_EXPORT_INTERVAL ||
  import.meta.env.VITE_TELEMERY_METRIC_EXPORT_INTERVAL ||
  '';

const APP_TELEMETRY_TRACE_EXPORT_INTERVAL: string =
  window.config?.VITE_TELEMERY_TRACE_EXPORT_INTERVAL ||
  import.meta.env.VITE_TELEMERY_TRACE_EXPORT_INTERVAL ||
  '';

const APP_TELEMETRY_HISTOGRAM_BUCKETS: string =
  window.config?.VITE_TELEMERY_HISTOGRAM_BUCKETS ||
  import.meta.env.VITE_TELEMERY_HISTOGRAM_BUCKETS ||
  '';

const jsonValues = stringToNull(APP_TELEMETRY_HISTOGRAM_BUCKETS);
const buckets: number[] = exists(jsonValues) ? JSON.parse(jsonValues) : defaultHistogramBuckets;

// See: https://www.justinpolidori.it/posts/20210913_containerize_fe_react/
// These global config values will come from:
//   - .env file in development (npm start)
//   - window.config is set in index.html, populated by env variables in production (npm run build)
export const TelemetryConfig = {
  enabled: DISABLE_TELEMETRY,
  debug: APP_TELEMETRY_DEBUG,
  environment: APP_TELEMETRY_ENVIRONMENT,
  appName: APP_NAME,
  appVersion: APP_VERSION,
  telemetryUrl: APP_TELEMETRY_URL,
  metricExportIntervalMs: APP_TELEMETRY_METRIC_EXPORT_INTERVAL,
  traceExportIntervalMs: APP_TELEMETRY_TRACE_EXPORT_INTERVAL,
  histogramBuckets: buckets,
};

export const DisableTelemetry: boolean = stringToNullableBoolean(DISABLE_TELEMETRY) === true;
