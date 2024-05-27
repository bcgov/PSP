import axios from 'axios';
import { KeycloakInstance } from 'keycloak-js';
import { useCallback } from 'react';

import { useTenant } from '@/tenants';

export const useRefreshSiteminder = (keycloak: KeycloakInstance) => {
  const { parcelMapFullyAttributed } = useTenant();
  const logout = keycloak.logout;

  const refresh = useCallback(async () => {
    const response = await axios.get(
      parcelMapFullyAttributed.url +
        `?outputFormat=application%2Fjson&request=GetFeature&maxFeatures=0&typeName=${parcelMapFullyAttributed.name}&service=WFS&version=1.0.0`,
      { withCredentials: true },
    );
    if (response.status !== 200) {
      console.error('Unable to refresh Siteminder cookie');
      logout();
    }
  }, [parcelMapFullyAttributed, logout]);

  return refresh;
};
