import '@/assets/scss/App.scss';
import '@/assets/scss/Stepper.scss';
import 'react-bootstrap-typeahead/css/Typeahead.css';

import React, { useEffect } from 'react';
import Col from 'react-bootstrap/Col';
import { ToastContainer } from 'react-toastify';

import AppRouter from '@/AppRouter';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { ModalContainer } from '@/components/common/ModalContainer';
import { RoleMismatchModal } from '@/components/modals/roleMismatch';
import { AuthStateContext, IAuthState } from '@/contexts/authStateContext';
import { useUsers } from '@/features/admin/users/hooks/useUsers';
import { useFavicon } from '@/hooks/useFavicon';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IUser } from '@/interfaces';
import PublicLayout from '@/layouts/PublicLayout';
import { useLookupCodes } from '@/store/slices/lookupCodes';
import { useSystemConstants } from '@/store/slices/systemConstants';

const App = () => {
  const keycloakWrapper = useKeycloakWrapper();
  const keycloak = keycloakWrapper.obj;
  const { fetchLookupCodes } = useLookupCodes();
  const { lookupCodes } = useLookupCodeHelpers();
  const { fetchSystemConstants } = useSystemConstants();
  const {
    activateUser: { execute: activate },
  } = useUsers();

  const [showRoleModal, setShowRoleModal] = React.useState(false);

  useFavicon();

  useEffect(() => {
    if (keycloak?.authenticated) {
      activate().then((user: IUser | undefined) => {
        if (user !== undefined && !user.hasValidClaims) {
          setShowRoleModal(true);
        }
      });
      fetchLookupCodes();
      fetchSystemConstants();
    }
  }, [keycloak, fetchLookupCodes, fetchSystemConstants, activate]);

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
            <RoleMismatchModal display={showRoleModal} setDisplay={setShowRoleModal} />
            <ModalContainer />
          </>
        );
      }}
    </AuthStateContext.Consumer>
  );
};

export default App;
