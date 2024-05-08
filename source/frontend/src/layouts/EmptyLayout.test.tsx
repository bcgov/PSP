import { act, render } from '@testing-library/react';
import { ThemeProvider } from 'styled-components';

import { TenantConsumer, TenantProvider } from '@/tenants';

import EmptyLayout from './EmptyLayout';

const mockGetVersion = vi.fn(async () => {
  return Promise.resolve({
    data: {
      environment: 'test',
      version: '11.1.1.1',
    },
  });
});

vi.mock('@/hooks/pims-api/useApiHealth', () => ({
  useApiHealth: () => ({
    getVersion: mockGetVersion,
  }),
}));

describe('Empty Layout', () => {
  import.meta.env.VITE_TENANT = 'MOTI';

  it('renders', async () => {
    await act(async () => {
      const { container } = render(
        <TenantProvider>
          <TenantConsumer>
            {({ tenant }) => (
              <ThemeProvider theme={{ tenant, css: {} }}>
                <EmptyLayout></EmptyLayout>
              </ThemeProvider>
            )}
          </TenantConsumer>
        </TenantProvider>,
      );
      expect(container).toMatchSnapshot();
    });
  });
});
