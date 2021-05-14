import './App.scss';
import 'react-bootstrap-typeahead/css/Typeahead.css';
import React, { useEffect } from 'react';
import { Col } from 'react-bootstrap';

import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { AuthStateContext, IAuthState } from 'contexts/authStateContext';
import AppRouter from 'router';
import OnLoadActions from 'OnLoadActions';
import { ToastContainer } from 'react-toastify';
import PublicLayout from 'layouts/PublicLayout';
import FilterBackdrop from 'components/maps/leaflet/FilterBackdrop';
import { useLookupCodes } from 'store/slices/lookupCodes';
import { useFavicon } from 'hooks/useFavicon';
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
