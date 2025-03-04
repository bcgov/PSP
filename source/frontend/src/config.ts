declare const window: {
  VITE_DEPLOY_ENV: string;
  VITE_SERVICE_NAME: string;
  VITE_ANALYTICS_ENDPOINT: string;
  VITE_ANALYTICS_DEBUG: string;
  VITE_ANALYTICS_DISABLE: string;
} & Window;

const VITE_DEPLOY_ENV: string = import.meta.env.VITE_DEPLOY_ENV || window.VITE_DEPLOY_ENV || '';
const VITE_SERVICE_NAME: string =
  import.meta.env.VITE_SERVICE_NAME || window.VITE_SERVICE_NAME || '';
const VITE_ANALYTICS_ENDPOINT: string =
  import.meta.env.VITE_ANALYTICS_ENDPOINT || window.VITE_ANALYTICS_ENDPOINT || '';
const VITE_ANALYTICS_DEBUG: string =
  import.meta.env.VITE_ANALYTICS_DEBUG || window.VITE_ANALYTICS_DEBUG || '';
const VITE_ANALYTICS_DISABLE: string =
  import.meta.env.VITE_ANALYTICS_DISABLE || window.VITE_ANALYTICS_DISABLE || '';

export {
  VITE_ANALYTICS_DEBUG,
  VITE_ANALYTICS_DISABLE,
  VITE_ANALYTICS_ENDPOINT,
  VITE_DEPLOY_ENV,
  VITE_SERVICE_NAME,
};
