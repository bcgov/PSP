import { render } from '@testing-library/react';
import React from 'react';

import { TenantProvider } from '@/tenants';

import EmptyHeader from './EmptyHeader';

describe('Empty Header', () => {
  it('renders', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { container } = render(
      <TenantProvider>
        <EmptyHeader></EmptyHeader>
      </TenantProvider>,
    );
    expect(container.firstChild).toMatchSnapshot();
  });
});
