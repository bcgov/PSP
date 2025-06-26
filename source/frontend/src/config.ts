declare global {
  interface Window {
    config: any;
  }
}

export interface AppGlobalConfig {
  VITE_TELEMERY_ENABLED: string;
  VITE_TELEMERY_DEBUG: string;
  VITE_TELEMERY_ENVIRONMENT: string;
  VITE_TELEMERY_SERVICE_NAME: string;
  VITE_TELEMERY_URL: string;
  VITE_TELEMERY_EXPORT_INTERVAL: string;
  VITE_TELEMERY_HISTOGRAM_BUCKETS: string;
}

// See: https://www.justinpolidori.it/posts/20210913_containerize_fe_react/
// These global config values will come from:
//   - .env file in development (npm start)
//   - window.config is set in index.html, populated by env variables in production (npm run build)
export const config: AppGlobalConfig = { ...import.meta.env, ...window.config } as AppGlobalConfig;
