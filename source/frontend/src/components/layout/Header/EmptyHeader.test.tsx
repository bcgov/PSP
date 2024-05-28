import { render } from '@testing-library/react';
import { TenantProvider } from '@/tenants';

import EmptyHeader from './EmptyHeader';

describe('Empty Header', () => {
  it('renders', () => {
    import.meta.env.VITE_TENANT = 'MOTI';
    const { container } = render(
      <TenantProvider>
        <EmptyHeader></EmptyHeader>
      </TenantProvider>,
    );
    expect(container.firstChild).toMatchSnapshot();
  });
});
