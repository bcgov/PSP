import { KeycloakInstance } from 'keycloak-js';
import { clearJwt, saveJwt } from 'store/slices/jwt/JwtSlice';
import { setKeycloakReady } from 'store/slices/keycloakReady/keycloakReadySlice';
import { store } from 'store/store';

import getKeycloakEventHandler from './getKeycloakEventHandler';

jest.mock('store/slices/jwt/JwtSlice', () => ({
  saveJwt: jest.fn(),
  clearJwt: jest.fn(),
}));
jest.mock('store/slices/keycloakReady/keycloakReadySlice');
jest.mock('store/store', () => ({
  store: {
    dispatch: jest.fn(),
  },
}));

const keycloak = ({
  subject: 'test',
  userInfo: {
    roles: [],
    organizations: ['1'],
  },
  token: '123456789',
} as any) as KeycloakInstance;
const keyclockEventHandler = getKeycloakEventHandler(keycloak);
describe('KeycloakEventHandler ', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });
  it('saves the token when onAuthSuccess event is fired', () => {
    keyclockEventHandler('onAuthSuccess');
    expect(saveJwt).toHaveBeenCalledWith(keycloak.token);
  });
  it('saves the token when onAuthRefreshSuccess event is fired', () => {
    keyclockEventHandler('onAuthRefreshSuccess');
    expect(saveJwt).toHaveBeenCalledWith(keycloak.token);
  });
  it('clears the token when onAuthLogout event is fired', () => {
    keyclockEventHandler('onAuthLogout');
    expect(clearJwt).toHaveBeenCalled();
  });
  it('clears the token when onTokenExpired event is fired', () => {
    keyclockEventHandler('onTokenExpired');
    expect(clearJwt).toHaveBeenCalled();
  });
  it('sets the ready flag when onReady event is fired', () => {
    keyclockEventHandler('onReady');
    expect(setKeycloakReady).toHaveBeenCalledWith(true);
  });
  it('does nothing when an unexpected event is fired', () => {
    keyclockEventHandler('onInitError');
    expect(store.dispatch).not.toHaveBeenCalled();
    expect(saveJwt).not.toHaveBeenCalled();
    expect(clearJwt).not.toHaveBeenCalled();
    expect(setKeycloakReady).not.toHaveBeenCalled();
  });
});
