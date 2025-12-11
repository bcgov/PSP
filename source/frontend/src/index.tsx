import '@bcgov/design-tokens/css/variables.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'leaflet/dist/leaflet.css';
import 'react-datepicker/dist/react-datepicker.css';
import 'react-toastify/dist/ReactToastify.css';
import 'yet-another-react-lightbox/plugins/captions.css';
import 'yet-another-react-lightbox/plugins/counter.css';
import 'yet-another-react-lightbox/styles.css';
import './assets/scss/index.scss'; // should be loaded last to allow for overrides without having to resort to "!important"

import * as bcTokens from '@bcgov/design-tokens/js/variables.js';
import { ReactKeycloakProvider } from '@react-keycloak/web';
import Keycloak from 'keycloak-js';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { BrowserRouter as Router } from 'react-router-dom';
import { ThemeProvider } from 'styled-components';

import css from '@/assets/scss/_variables.module.scss';
import { AuthStateContextProvider } from '@/contexts/authStateContext';
import { ModalContextProvider } from '@/contexts/modalContext';
import LoginLoading from '@/features/account/LoginLoading';
import EmptyLayout from '@/layouts/EmptyLayout';
import { store } from '@/store/store';
import { TenantConsumer, TenantProvider } from '@/tenants';
import getKeycloakEventHandler from '@/utils/getKeycloakEventHandler';

import App from './App';
import { config } from './config';
import { DocumentViewerContextProvider } from './features/documents/context/DocumentViewerContext';
import { WorklistContextProvider } from './features/properties/worklist/context/WorklistContext';
import { ITenantConfig2 } from './hooks/pims-api/interfaces/ITenantConfig';
import { useRefreshSiteminder } from './hooks/useRefreshSiteminder';
import { initializeTelemetry } from './telemetry';
import { defaultHistogramBuckets, TelemetryConfig } from './telemetry/config';
import { ReactRouterSpanProcessor } from './telemetry/traces/ReactRouterSpanProcessor';
import { exists } from './utils';
import { stringToNull, stringToNullableBoolean, stringToNumberOrNull } from './utils/formUtils';

async function prepare() {
  if (process.env.NODE_ENV === 'development') {
    // eslint-disable-next-line @typescript-eslint/no-var-requires
    const { worker } = await import('./mocks/msw/browser');
    return worker.start({ onUnhandledRequest: 'bypass' });
  }
  return Promise.resolve();
}

const keycloak: Keycloak = new Keycloak('/keycloak.json');

const Index = () => {
  return (
    <TenantProvider>
      <ModalContextProvider>
        <Router>
          <TenantConsumer>{({ tenant }) => <InnerComponent tenant={tenant} />}</TenantConsumer>
        </Router>
      </ModalContextProvider>
    </TenantProvider>
  );
};

const InnerComponent = ({ tenant }: { tenant: ITenantConfig2 }) => {
  const refresh = useRefreshSiteminder();

  return (
    <ThemeProvider theme={{ tenant, css, bcTokens }}>
      <ReactKeycloakProvider
        initOptions={{ pkceMethod: 'S256' }}
        authClient={keycloak}
        LoadingComponent={
          <EmptyLayout>
            <LoginLoading />
          </EmptyLayout>
        }
        onEvent={getKeycloakEventHandler(keycloak, refresh)}
      >
        <Provider store={store}>
          <AuthStateContextProvider>
            <DocumentViewerContextProvider>
              <WorklistContextProvider>
                <App />
                <ReactRouterSpanProcessor />
              </WorklistContextProvider>
            </DocumentViewerContextProvider>
          </AuthStateContextProvider>
        </Provider>
      </ReactKeycloakProvider>
    </ThemeProvider>
  );
};

// get telemetry options from global configuration.
// window.config is set in index.html, populated by env variables.
const setupTelemetry = () => {
  const isTelemetryEnabled = stringToNullableBoolean(config.VITE_TELEMERY_ENABLED) ?? false;
  const isDebugEnabled = stringToNullableBoolean(config.VITE_TELEMERY_DEBUG) ?? false;

  if (isTelemetryEnabled) {
    const jsonValues = stringToNull(config.VITE_TELEMERY_HISTOGRAM_BUCKETS);
    const buckets: number[] = exists(jsonValues) ? JSON.parse(jsonValues) : defaultHistogramBuckets;
    const options: TelemetryConfig = {
      name: config.VITE_TELEMERY_SERVICE_NAME ?? 'frontend',
      appVersion: import.meta.env.VITE_PACKAGE_VERSION ?? '',
      environment: config.VITE_TELEMERY_ENVIRONMENT || 'local',
      otlpEndpoint: config.VITE_TELEMERY_URL || '',
      debug: isDebugEnabled,
      exportInterval: stringToNumberOrNull(config.VITE_TELEMERY_EXPORT_INTERVAL) ?? 30000,
      histogramBuckets: buckets,
    };

    // configure browser telemetry (if enabled via dynamic config-map)
    initializeTelemetry(options);

    console.log('[INFO] Telemetry enabled');
    if (isDebugEnabled) {
      console.log(options);
    }
  } else {
    console.log('[INFO] Telemetry disabled');
  }
};

prepare().then(() => {
  setupTelemetry();
  const root = createRoot(document.getElementById('root') as Element);
  root.render(<Index />);
});
