import { Attributes } from '@opentelemetry/api';
import {
  ATTR_CODE_FUNCTION,
  ATTR_EVENT_NAME,
  ATTR_EXCEPTION_MESSAGE,
  ATTR_EXCEPTION_TYPE,
} from '@opentelemetry/semantic-conventions/incubating';
import { AuthClientError, AuthClientEvent } from '@react-keycloak/core/lib/index';
import Keycloak from 'keycloak-js';
import { toast } from 'react-toastify';

import { clearJwt, saveJwt } from '@/store/slices/jwt/JwtSlice';
import { setKeycloakReady } from '@/store/slices/keycloakReady/keycloakReadySlice';
import { store } from '@/store/store';
import { Telemetry, UserTelemetry } from '@/telemetry';
import { SpanEnrichment } from '@/telemetry/traces/SpanEnrichment';
import { getUserDetailsFromKeycloakToken } from '@/telemetry/users/UserAPI';

const getKeycloakEventHandler = (keycloak: Keycloak, onRefresh: () => void) => {
  const errorMessage =
    'Received error from authentication provider. Refresh the application if you are unable to log in. If this error persists, contact a system administrator';
  const keycloakEventHandler = (
    eventType: AuthClientEvent,
    error?: AuthClientError | undefined,
  ) => {
    // Track keycloak authentication events with browser telemetry
    const spanAttributes: Attributes = {
      component: 'keycloak',
      [ATTR_EVENT_NAME]: eventType,
      [ATTR_CODE_FUNCTION]: 'getKeycloakEventHandler',
      'user.is_authenticated': keycloak?.authenticated ?? false,
    };

    if (eventType === 'onAuthSuccess') {
      onRefresh();
      store.dispatch(saveJwt(keycloak.token ?? ''));
      // store the currently logged user so that telemetry spans can be traced back to user actions
      const userDetails = getUserDetailsFromKeycloakToken(keycloak.tokenParsed);
      UserTelemetry.getUserManager().setUser(userDetails);
      SpanEnrichment.enrichWithKeycloakToken(spanAttributes, keycloak);
    } else if (eventType === 'onAuthRefreshSuccess') {
      onRefresh();
      store.dispatch(saveJwt(keycloak.token ?? ''));
      SpanEnrichment.enrichWithKeycloakToken(spanAttributes, keycloak);
    } else if (eventType === 'onAuthLogout' || eventType === 'onTokenExpired') {
      store.dispatch(clearJwt());
      UserTelemetry.getUserManager().clearUser();
      spanAttributes[ATTR_EXCEPTION_TYPE] = error?.error ?? '';
      spanAttributes[ATTR_EXCEPTION_MESSAGE] = error?.error_description ?? '';
    } else if (eventType === 'onReady') {
      store.dispatch(setKeycloakReady(true));
    } else {
      toast.error(errorMessage);
      console.debug(`keycloak event: ${eventType} error ${JSON.stringify(error)}`);
      spanAttributes[ATTR_EXCEPTION_TYPE] = error?.error ?? '';
      spanAttributes[ATTR_EXCEPTION_MESSAGE] = error?.error_description ?? '';
    }
    Telemetry.recordEvent(`keycloak_${eventType}`, spanAttributes);
  };

  return keycloakEventHandler;
};

export default getKeycloakEventHandler;
