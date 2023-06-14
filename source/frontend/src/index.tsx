import 'bootstrap/dist/css/bootstrap.min.css';
import 'leaflet/dist/leaflet.css';
import 'react-app-polyfill/ie11';
import 'react-app-polyfill/stable';
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
import * as serviceWorker from './serviceWorker.ignore';

function prepare() {
  if (process.env.NODE_ENV === 'development') {
    const { worker } = require('./mocks/msw/browser');
    return worker.start({ onUnhandledRequest: 'bypass' });
  }
  return Promise.resolve();
}

//@ts-ignore
const keycloak: KeycloakInstance = new Keycloak('/keycloak.json');
const Index = () => {
  return (
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <ThemeProvider theme={{ tenant, css }}>
            <ReactKeycloakProvider
              initOptions={{ pkceMethod: 'S256' }}
              authClient={keycloak}
              LoadingComponent={
                <EmptyLayout>
                  <LoginLoading />
                </EmptyLayout>
              }
              onEvent={getKeycloakEventHandler(keycloak)}
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
        )}
      </TenantConsumer>
    </TenantProvider>
  );
};

prepare().then(() => {
  const root = createRoot(document.getElementById('root') as Element);
  root.render(<Index />);
});

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
