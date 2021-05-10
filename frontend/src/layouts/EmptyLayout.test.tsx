import React from 'react';
import EmptyLayout from './EmptyLayout';
import { render } from '@testing-library/react';
import { TenantProvider } from 'tenants';

describe('Empty Layout', () => {
  it('renders', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { container } = render(
      <TenantProvider>
        <EmptyLayout></EmptyLayout>
      </TenantProvider>,
    );
    expect(container.firstChild).toMatchSnapshot();
  });
});
