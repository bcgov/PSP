import 'assets/scss/App.scss';
import 'assets/scss/Stepper.scss';
import 'react-bootstrap-typeahead/css/Typeahead.css';

import AppRouter from 'AppRouter';
import { ModalContainer } from 'components/common/ModalContainer';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { AuthStateContext, IAuthState } from 'contexts/authStateContext';
import { useFavicon } from 'hooks/useFavicon';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import PublicLayout from 'layouts/PublicLayout';
import React, { useEffect } from 'react';
import Col from 'react-bootstrap/Col';
import { ToastContainer } from 'react-toastify';
import { useLookupCodes } from 'store/slices/lookupCodes';
import { useSystemConstants } from 'store/slices/systemConstants';

const App = () => {
  const keycloakWrapper = useKeycloakWrapper();
  const keycloak = keycloakWrapper.obj;
  const { fetchLookupCodes } = useLookupCodes();
  const { lookupCodes } = useLookupCodeHelpers();
  const { fetchSystemConstants } = useSystemConstants();
  useFavicon();

  useEffect(() => {
    if (keycloak?.authenticated) {
      fetchLookupCodes();
      fetchSystemConstants();
    }
  }, [keycloak, fetchLookupCodes, fetchSystemConstants]);

  return (
    <AuthStateContext.Consumer>
      {(context: IAuthState) => {
        if (!context.ready || !lookupCodes) {
          return (
            <PublicLayout>
              <Col>
                <LoadingBackdrop show={true}></LoadingBackdrop>
              </Col>
            </PublicLayout>
          );
        }

        return (
          <>
            <AppRouter />
            <ToastContainer
              position="top-right"
              autoClose={5000}
              hideProgressBar
              newestOnTop={false}
              closeOnClick={false}
              rtl={false}
              pauseOnFocusLoss={false}
              draggable
              pauseOnHover
            />
            <ModalContainer />
          </>
        );
      }}
    </AuthStateContext.Consumer>
  );
};

export default App;
