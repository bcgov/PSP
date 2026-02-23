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
import { TelemetryConfig } from '@/utils/config';
import getKeycloakEventHandler from '@/utils/getKeycloakEventHandler';

import App from './App';
import { NavigationIntentProvider } from './contexts/NavigationIntentContext';
import { DocumentViewerContextProvider } from './features/documents/context/DocumentViewerContext';
import { WorklistContextProvider } from './features/properties/worklist/context/WorklistContext';
import { ITenantConfig2 } from './hooks/pims-api/interfaces/ITenantConfig';
import { useRefreshSiteminder } from './hooks/useRefreshSiteminder';
import { initializeTelemetry } from './telemetry';
import { TelemetrySettings } from './telemetry/config';
import { ReactRouterSpanProcessor } from './telemetry/traces/ReactRouterSpanProcessor';
import { stringToNullableBoolean, stringToNumberOrNull } from './utils/formUtils';

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
        <NavigationIntentProvider>
          <Router>
            <TenantConsumer>{({ tenant }) => <InnerComponent tenant={tenant} />}</TenantConsumer>
          </Router>
        </NavigationIntentProvider>
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
  const isTelemetryEnabled = stringToNullableBoolean(TelemetryConfig.enabled) ?? false;
  const isDebugEnabled = stringToNullableBoolean(TelemetryConfig.debug) ?? false;

  if (isTelemetryEnabled) {
    const options: TelemetrySettings = {
      name: TelemetryConfig.appName || 'pims-frontend',
      appVersion: TelemetryConfig.appVersion || 'unknown',
      environment: TelemetryConfig.environment || 'local',
      otlpEndpoint: TelemetryConfig.telemetryUrl || '',
      debug: isDebugEnabled,
      exportInterval: stringToNumberOrNull(TelemetryConfig.exportInterval) ?? 30000,
      histogramBuckets: TelemetryConfig.histogramBuckets,
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
