import React from 'react';
import EmptyLayout from './EmptyLayout';
import { act, render } from '@testing-library/react';
import { TenantProvider } from 'tenants';

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
          <EmptyLayout></EmptyLayout>
        </TenantProvider>,
      );
      expect(container).toMatchSnapshot();
    });
  });
});
