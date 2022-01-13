import './App.scss';
import './Stepper.scss';
import 'react-bootstrap-typeahead/css/Typeahead.css';

import AppRouter from 'AppRouter';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { AuthStateContext, IAuthState } from 'contexts/authStateContext';
import { useFavicon } from 'hooks/useFavicon';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import PublicLayout from 'layouts/PublicLayout';
import OnLoadActions from 'OnLoadActions';
import { useEffect } from 'react';
import Col from 'react-bootstrap/Col';
import { ToastContainer } from 'react-toastify';
import { useLookupCodes } from 'store/slices/lookupCodes';
import { useSystemConstants } from 'store/slices/systemConstants';
import { useUsers } from 'store/slices/users';

const App = () => {
  const keycloakWrapper = useKeycloakWrapper();
  const keycloak = keycloakWrapper.obj;
  const { fetchLookupCodes } = useLookupCodes();
  const { fetchSystemConstants } = useSystemConstants();
  const { activateUser } = useUsers();
  useFavicon();

  useEffect(() => {
    if (keycloak?.authenticated) {
      activateUser();
      fetchLookupCodes();
      fetchSystemConstants();
    }
  }, [keycloak, fetchLookupCodes, fetchSystemConstants, activateUser]);

  return (
    <AuthStateContext.Consumer>
      {(context: IAuthState) => {
        if (!context.ready) {
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
