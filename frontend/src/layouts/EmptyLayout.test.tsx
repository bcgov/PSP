import React from 'react';
import EmptyLayout from './EmptyLayout';
import { act, render } from '@testing-library/react';
import { TenantConsumer, TenantProvider } from 'tenants';
import { ThemeProvider } from 'styled-components';

const mockGetVersion = jest.fn(async () => {
  return Promise.resolve({
    data: {
      environment: 'test',
      version: '11.1.1.1',
    },
  });
});

jest.mock('hooks/pims-api', () => ({
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
