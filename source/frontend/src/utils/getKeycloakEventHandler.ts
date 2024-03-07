import { AuthClientError, AuthClientEvent } from '@react-keycloak/core/lib/index';
import { KeycloakInstance } from 'keycloak-js';
import { toast } from 'react-toastify';

import { clearJwt, saveJwt } from '@/store/slices/jwt/JwtSlice';
import { setKeycloakReady } from '@/store/slices/keycloakReady/keycloakReadySlice';
import { store } from '@/store/store';

const getKeycloakEventHandler = (keycloak: KeycloakInstance, onRefresh: () => void) => {
  const errorMessage =
    'Received error from authentication provider. Refresh the application if you are unable to log in. If this error persists, contact a system administrator';
  const keycloakEventHandler = (
    eventType: AuthClientEvent,
    error?: AuthClientError | undefined,
  ) => {
    if (eventType === 'onAuthSuccess') {
      store.dispatch(saveJwt(keycloak.token ?? ''));
    } else if (eventType === 'onAuthRefreshSuccess') {
      onRefresh();
      store.dispatch(saveJwt(keycloak.token ?? ''));
    } else if (eventType === 'onAuthLogout' || eventType === 'onTokenExpired') {
      store.dispatch(clearJwt());
    } else if (eventType === 'onReady') {
      store.dispatch(setKeycloakReady(true));
    } else {
      toast.error(errorMessage);
      console.debug(`keycloak event: ${eventType} error ${JSON.stringify(error)}`);
    }
  };
  return keycloakEventHandler;
};

export default getKeycloakEventHandler;
