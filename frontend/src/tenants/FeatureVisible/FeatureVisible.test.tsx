import { cleanup, render } from '@testing-library/react';
import React from 'react';
import { TenantProvider } from 'tenants';

import { FeatureVisible } from '.';

const testRender = () =>
  render(
    <TenantProvider>
      {/* Hide the content below if tenant is MOTI */}
      <FeatureVisible tenant="MOTI">
        <div data-testid="feature-toggle-testid"></div>
      </FeatureVisible>
    </TenantProvider>,
  );

describe('FeatureVisible', () => {
  afterEach(() => {
    cleanup();
  });

  it(`should show content for specified tenant`, () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { queryByTestId } = testRender();
    expect(queryByTestId('feature-toggle-testid')).toBeInTheDocument();
    delete process.env.REACT_APP_TENANT;
  });

  it(`should hide the content for other tenants`, () => {
    process.env.REACT_APP_TENANT = 'CITZ';
    const { queryByTestId } = testRender();
    expect(queryByTestId('feature-toggle-testid')).not.toBeInTheDocument();
    delete process.env.REACT_APP_TENANT;
  });
});
