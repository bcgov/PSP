import { act, render } from '@testing-library/react';
import React from 'react';
import { ThemeProvider } from 'styled-components';

import { TenantConsumer, TenantProvider } from '@/tenants';

import EmptyLayout from './EmptyLayout';

const mockGetVersion = jest.fn(async () => {
  return Promise.resolve({
    data: {
      environment: 'test',
      version: '11.1.1.1',
    },
  });
});

jest.mock('@/hooks/pims-api/useApiHealth', () => ({
  useApiHealth: () => ({
    getVersion: mockGetVersion,
  }),
}));

describe('Empty Layout', () => {
  process.env.REACT_APP_TENANT = 'MOTI';

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
