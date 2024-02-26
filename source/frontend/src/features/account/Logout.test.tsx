import { useKeycloak } from '@react-keycloak/web';
import { cleanup, render, waitFor } from '@testing-library/react';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Router } from 'react-router-dom';

import { useConfiguration } from '@/hooks/useConfiguration';

import { LogoutPage } from './Logout';
jest.mock('@react-keycloak/web');
jest.mock('@/hooks/useConfiguration');

describe('logout', () => {
  const history = createMemoryHistory();
  const { location } = window;

  beforeAll(() => {
    delete (window as any).location;
    window.location = { replace: jest.fn() } as any;
  });

  afterAll(() => (window.location = location));

  afterEach(() => {
    cleanup();
  });

  it('should redirect to login page', () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    (useConfiguration as jest.Mock).mockReturnValue({ siteMinderLogoutUrl: undefined });

    render(
      <Router history={history}>
        <LogoutPage />
      </Router>,
    );

    expect(history.location.pathname).toBe('/login');
  });

  it('should redirect to siteminder logout page', async () => {
    (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
    (useConfiguration as jest.Mock).mockReturnValue({
      siteMinderLogoutUrl: 'http://fakesiteminder.com',
    });

    render(
      <Router history={history}>
        <LogoutPage />
      </Router>,
    );

    await waitFor(() => expect(window.location.replace).toHaveBeenCalledTimes(1));
  });
});
