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
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
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
import { DocumentViewerContextProvider } from './features/documents/context/DocumentViewerContext';
import { ITenantConfig2 } from './hooks/pims-api/interfaces/ITenantConfig';
import { useRefreshSiteminder } from './hooks/useRefreshSiteminder';
import { initializeTelemetry } from './telemetry';
import { TelemetryConfig } from './telemetry/config';
import { ReactRouterSpanProcessor } from './telemetry/traces/ReactRouterSpanProcessor';

async function prepare() {
  if (process.env.NODE_ENV === 'development') {
    // eslint-disable-next-line @typescript-eslint/no-var-requires
    const { worker } = await import('./mocks/msw/browser');
    return worker.start({ onUnhandledRequest: 'bypass' });
  }
  return Promise.resolve();
}

// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-ignore
const keycloak: Keycloak = new Keycloak('/keycloak.json');
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 600000, // when this is set, a query is set stale after 10 minutes. Stale queries are refreshed when useQuery is mounted.
    },
  },
}); // set various defaults to control the caching and refetching of queries. Could be set by tenant.
const Index = () => {
  return (
    <TenantProvider>
      <ModalContextProvider>
        <QueryClientProvider client={queryClient}>
          <Router>
            <TenantConsumer>{({ tenant }) => <InnerComponent tenant={tenant} />}</TenantConsumer>
          </Router>
        </QueryClientProvider>
      </ModalContextProvider>
    </TenantProvider>
  );
};

const InnerComponent = ({ tenant }: { tenant: ITenantConfig2 }) => {
  const refresh = useRefreshSiteminder();

  // get telemetry configuration from tenant json
  if (tenant?.telemetry?.enabled) {
    const config: TelemetryConfig = {
      name: tenant?.telemetry?.serviceName ?? 'frontend',
      appVersion: import.meta.env.VITE_PACKAGE_VERSION ?? '',
      environment: tenant?.telemetry?.environment || 'local',
      otlpEndpoint: tenant?.telemetry?.endpoint || '',
      debug: tenant?.telemetry?.debug ?? false,
      exportInterval: tenant?.telemetry?.exportInterval ?? 30_000,
      histogramBuckets: tenant?.telemetry?.histogramBuckets ?? [],
    };

    // configure browser telemetry (if enabled via dynamic config-map)
    initializeTelemetry(config);

    console.log('[INFO] Telemetry enabled');
    if (tenant?.telemetry?.debug) {
      console.log(config);
    }
  } else {
    console.log('[INFO] Telemetry disabled');
  }

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
              <App />
              <ReactRouterSpanProcessor />
            </DocumentViewerContextProvider>
          </AuthStateContextProvider>
        </Provider>
      </ReactKeycloakProvider>
    </ThemeProvider>
  );
};

prepare().then(() => {
  const root = createRoot(document.getElementById('root') as Element);
  root.render(<Index />);
});
