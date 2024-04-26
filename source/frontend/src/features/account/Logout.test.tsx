import { useKeycloak } from '@react-keycloak/web';
import { cleanup, render, waitFor } from '@testing-library/react';
import { createMemoryHistory } from 'history';
import { Router } from 'react-router-dom/cjs/react-router-dom';

import { useConfiguration } from '@/hooks/useConfiguration';

import { LogoutPage } from './Logout';

vi.mock('@/hooks/useConfiguration');
vi.mock('@react-keycloak/web');
describe('logout', () => {
  const history = createMemoryHistory();
  const { location } = window;

  beforeAll(() => {
    delete (window as any).location;
    window.location = { replace: vi.fn() } as any;
  });

  afterAll(() => (window.location = location));

  afterEach(() => {
    cleanup();
  });

  it('should redirect to login page', () => {
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: { authenticated: false } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });
    vi.mocked(useConfiguration).mockReturnValue({
      siteMinderLogoutUrl: undefined,
    } as unknown as ReturnType<typeof useConfiguration>);

    render(
      <Router history={history}>
        <LogoutPage />
      </Router>,
    );

    expect(history.location.pathname).toBe('/login');
  });

  it('should redirect to siteminder logout page', async () => {
    vi.mocked(useKeycloak).mockReturnValue({
      keycloak: { authenticated: false } as unknown as Keycloak.KeycloakInstance,
      initialized: true,
    });
    vi.mocked(useConfiguration).mockReturnValue({
      siteMinderLogoutUrl: 'http://fakesiteminder.com',
    } as unknown as ReturnType<typeof useConfiguration>);

    render(
      <Router history={history}>
        <LogoutPage />
      </Router>,
    );

    await waitFor(() => expect(window.location.replace).toHaveBeenCalledTimes(1));
  });
});
