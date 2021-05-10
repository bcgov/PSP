import React from 'react';
import { render } from '@testing-library/react';
import { TenantProvider } from 'tenants';
import LoginLoading from './LoginLoading';

describe('Empty Header', () => {
  it('renders', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { container } = render(
      <TenantProvider>
        <LoginLoading></LoginLoading>
      </TenantProvider>,
    );
    expect(container.firstChild).toMatchSnapshot();
  });
});
