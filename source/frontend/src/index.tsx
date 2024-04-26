import 'bootstrap/dist/css/bootstrap.min.css';
import 'leaflet/dist/leaflet.css';
import 'react-datepicker/dist/react-datepicker.css';
import 'react-toastify/dist/ReactToastify.css';
import './assets/scss/index.scss'; // should be loaded last to allow for overrides without having to resort to "!important"

import { ReactKeycloakProvider } from '@react-keycloak/web';
import Keycloak, { KeycloakInstance } from 'keycloak-js';
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
import { ITenantConfig2 } from './hooks/pims-api/interfaces/ITenantConfig';
import { useRefreshSiteminder } from './hooks/useRefreshSiteminder';

async function prepare() {
  if (import.meta.env.DEV) {
    // eslint-disable-next-line @typescript-eslint/no-var-requires
    const { worker } = await import('./mocks/msw/browser');
    return worker.start({ onUnhandledRequest: 'bypass' });
  }
  return Promise.resolve();
}

// eslint-disable-next-line @typescript-eslint/ban-ts-comment
//@ts-ignore
const keycloak: KeycloakInstance = new Keycloak('/keycloak.json');
const Index = () => {
  return (
    <TenantProvider>
      <TenantConsumer>{({ tenant }) => <InnerComponent tenant={tenant} />}</TenantConsumer>
    </TenantProvider>
  );
};

const InnerComponent = ({ tenant }: { tenant: ITenantConfig2 }) => {
  const refresh = useRefreshSiteminder();
  return (
    <ThemeProvider theme={{ tenant, css }}>
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
            <ModalContextProvider>
              <Router>
                <App />
              </Router>
            </ModalContextProvider>
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
