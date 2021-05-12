import './App.scss';
import 'react-bootstrap-typeahead/css/Typeahead.css';
import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Col } from 'react-bootstrap';

import { getActivateUserAction } from 'actionCreators/usersActionCreator';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { AuthStateContext, IAuthState } from 'contexts/authStateContext';
import AppRouter from 'router';
import OnLoadActions from 'OnLoadActions';
import { ToastContainer } from 'react-toastify';
import PublicLayout from 'layouts/PublicLayout';
import FilterBackdrop from 'components/maps/leaflet/FilterBackdrop';
import { useLookupCodes } from 'store/slices/lookupCodes';

const App = () => {
  const keycloakWrapper = useKeycloakWrapper();
  const keycloak = keycloakWrapper.obj;
  const dispatch = useDispatch();
  const { fetchLookupCodes } = useLookupCodes();

  useEffect(() => {
    if (keycloak?.authenticated) {
      dispatch(getActivateUserAction());
      fetchLookupCodes();
    }
  }, [dispatch, keycloak, fetchLookupCodes]);

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
