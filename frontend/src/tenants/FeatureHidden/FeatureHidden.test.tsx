import { cleanup, render } from '@testing-library/react';
import React from 'react';
import { TenantProvider } from 'tenants';

import { FeatureHidden } from '.';

const testRender = () =>
  render(
    <TenantProvider>
      {/* Hide the content below if tenant is MOTI */}
      <FeatureHidden tenant="MOTI">
        <div data-testid="feature-toggle-testid"></div>
      </FeatureHidden>
    </TenantProvider>,
  );

describe('FeatureHidden', () => {
  afterEach(() => {
    cleanup();
  });

  it(`should hide content for specified tenant`, () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const { queryByTestId } = testRender();
    expect(queryByTestId('feature-toggle-testid')).not.toBeInTheDocument();
    delete process.env.REACT_APP_TENANT;
  });

  it(`should show content for other tenants`, () => {
    process.env.REACT_APP_TENANT = 'CITZ';
    const { queryByTestId } = testRender();
    expect(queryByTestId('feature-toggle-testid')).toBeInTheDocument();
    delete process.env.REACT_APP_TENANT;
  });

  describe('when using render props', () => {
    const withRenderProps = () =>
      render(
        <TenantProvider>
          {/* Hide the content below if tenant is MOTI */}
          <FeatureHidden tenant="MOTI">
            {(hidden: boolean) => (hidden ? null : <div data-testid="feature-toggle-testid"></div>)}
          </FeatureHidden>
        </TenantProvider>,
      );

    it(`should hide content for specified tenant`, () => {
      process.env.REACT_APP_TENANT = 'MOTI';
      const { queryByTestId } = withRenderProps();
      expect(queryByTestId('feature-toggle-testid')).not.toBeInTheDocument();
      delete process.env.REACT_APP_TENANT;
    });

    it(`should show the content for other tenants`, () => {
      process.env.REACT_APP_TENANT = 'CITZ';
      const { queryByTestId } = withRenderProps();
      expect(queryByTestId('feature-toggle-testid')).toBeInTheDocument();
      delete process.env.REACT_APP_TENANT;
    });
  });
});
