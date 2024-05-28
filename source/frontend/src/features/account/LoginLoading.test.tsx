import { render } from '@testing-library/react';
import { ThemeProvider } from 'styled-components';

import { TenantConsumer, TenantProvider } from '@/tenants';

import LoginLoading from './LoginLoading';

const TestLoginLoading = () => {
  return (
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <ThemeProvider theme={{ tenant, css: {} }}>
            <LoginLoading></LoginLoading>
          </ThemeProvider>
        )}
      </TenantConsumer>
    </TenantProvider>
  );
};

describe('Empty Header', () => {
  it('MOTI Login Loading', () => {
    import.meta.env.VITE_TENANT = 'MOTI';
    const { container } = render(<TestLoginLoading />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('CITZ Login Loading', () => {
    import.meta.env.VITE_TENANT = 'CITZ';
    const { container } = render(<TestLoginLoading />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
