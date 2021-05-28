import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-datepicker/dist/react-datepicker.css';
import 'leaflet/dist/leaflet.css';
import './index.scss'; // should be loaded last to allow for overrides without having to resort to "!important"
import 'react-app-polyfill/ie11';
import 'react-app-polyfill/stable';
import 'react-toastify/dist/ReactToastify.css';

import { ReactKeycloakProvider } from '@react-keycloak/web';
import { AuthStateContextProvider } from 'contexts/authStateContext';
import LoginLoading from 'features/account/LoginLoading';
import Keycloak, { KeycloakInstance } from 'keycloak-js';
import EmptyLayout from 'layouts/EmptyLayout';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter as Router } from 'react-router-dom';
import { store } from 'store/store';
import { ThemeProvider } from 'styled-components';
import { TenantConsumer, TenantProvider } from 'tenants';
import getKeycloakEventHandler from 'utils/KeycloakEventHandler';

import App from './App';
import * as serviceWorker from './serviceWorker.ignore';

// eslint-disable-next-line import/no-webpack-loader-syntax
const css = require('sass-extract-loader?{"plugins": ["sass-extract-js"]}!./_variables.scss');

//@ts-ignore
const keycloak: KeycloakInstance = new Keycloak('/keycloak.json');
const Index = () => {
  return (
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <ThemeProvider theme={{ tenant, css }}>
            <ReactKeycloakProvider
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
                  <Router>
                    <App />
                  </Router>
                </AuthStateContextProvider>
              </Provider>
            </ReactKeycloakProvider>
          </ThemeProvider>
        )}
      </TenantConsumer>
    </TenantProvider>
  );
};

ReactDOM.render(<Index />, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
