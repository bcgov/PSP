import { createContext, useEffect, useState } from 'react';

import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useAppSelector } from '@/store/hooks';

export interface IAuthState {
  ready?: boolean;
}

export const AuthStateContext = createContext<IAuthState>({
  ready: false,
});

export const AuthStateContextProvider = (props: { children?: any }) => {
  const keycloak = useKeycloakWrapper();
  const [userInfo, setUserInfo] = useState<any>(null);
  const keycloakReady: boolean = useAppSelector(state => state.keycloakReady);

  useEffect(() => {
    const loadUserInfo = async () => {
      try {
        const user = await keycloak.obj?.loadUserInfo();
        setUserInfo(user);
      } catch (err) {
        // this error isn't recoverable, so just log it for debugging purposes.
        console.error(err);
      }
    };

    if (keycloak.obj.authenticated) loadUserInfo();
  }, [keycloak.obj]);

  return (
    <AuthStateContext.Provider
      value={{
        // if user info is not available when authenticated, then the auth state is not ready
        ready:
          keycloakReady &&
          (!keycloak.obj?.authenticated || (keycloak.obj?.authenticated && !!userInfo)),
      }}
    >
      {props.children}
    </AuthStateContext.Provider>
  );
};
