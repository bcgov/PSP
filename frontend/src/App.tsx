import './App.scss';
import './Stepper.scss';
import 'react-bootstrap-typeahead/css/Typeahead.css';

import AppRouter from 'AppRouter';
import FilterBackdrop from 'components/maps/leaflet/FilterBackdrop';
import { AuthStateContext, IAuthState } from 'contexts/authStateContext';
import { useFavicon } from 'hooks/useFavicon';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import PublicLayout from 'layouts/PublicLayout';
import OnLoadActions from 'OnLoadActions';
import React, { useEffect } from 'react';
import Col from 'react-bootstrap/Col';
import { ToastContainer } from 'react-toastify';
import { useLookupCodes } from 'store/slices/lookupCodes';
import { useUsers } from 'store/slices/users';

const App = () => {
  const keycloakWrapper = useKeycloakWrapper();
  const keycloak = keycloakWrapper.obj;
  const { fetchLookupCodes } = useLookupCodes();
  const { activateUser } = useUsers();
  useFavicon();

  useEffect(() => {
    if (keycloak?.authenticated) {
      activateUser();
      fetchLookupCodes();
    }
  }, [keycloak, fetchLookupCodes, activateUser]);

  return (
    <AuthStateContext.Consumer>
      {(context: IAuthState) => {
        if (!context.ready) {
          return (
            <PublicLayout>
              <Col>
                <FilterBackdrop show={true}></FilterBackdrop>
              </Col>
            </PublicLayout>
          );
        }

        return (
          <>
            <AppRouter />
            <OnLoadActions />
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
          </>
        );
      }}
    </AuthStateContext.Consumer>
  );
};

export default App;
