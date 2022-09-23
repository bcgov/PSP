import { AuthClientError, AuthClientEvent } from '@react-keycloak/core/lib/index';
import { KeycloakInstance } from 'keycloak-js';
import { clearJwt, saveJwt } from 'store/slices/jwt/JwtSlice';
import { setKeycloakReady } from 'store/slices/keycloakReady/keycloakReadySlice';
import { store } from 'store/store';

const getKeycloakEventHandler = (keycloak: KeycloakInstance) => {
  const keycloakEventHandler = (
    eventType: AuthClientEvent,
    error?: AuthClientError | undefined,
  ) => {
    if (eventType === 'onAuthSuccess') {
      store.dispatch(saveJwt(keycloak.token!));
    } else if (eventType === 'onAuthRefreshSuccess') {
      store.dispatch(saveJwt(keycloak.token!));
    } else if (eventType === 'onAuthLogout' || eventType === 'onTokenExpired') {
      store.dispatch(clearJwt());
    } else if (eventType === 'onReady') {
      store.dispatch(setKeycloakReady(true));
    } else {
      //TODO: log error properly
      console.debug(`keycloak event: ${eventType} error ${error}`);
    }
  };
  return keycloakEventHandler;
};

export default getKeycloakEventHandler;
