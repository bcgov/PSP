import { cleanup, render } from '@testing-library/react';
import React from 'react';
import { TenantProvider } from 'tenants';

import { FeatureVisible } from '.';

const testRender = () =>
  render(
    <TenantProvider>
      {/* Show the content below only for MOTI */}
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

  describe('when using render props', () => {
    const withRenderProps = () =>
      render(
        <TenantProvider>
          {/* Show the content below only for MOTI */}
          <FeatureVisible tenant="MOTI">
            {(visible: boolean) => visible && <div data-testid="feature-toggle-testid"></div>}
          </FeatureVisible>
        </TenantProvider>,
      );

    it(`should show content for specified tenant`, () => {
      process.env.REACT_APP_TENANT = 'MOTI';
      const { queryByTestId } = withRenderProps();
      expect(queryByTestId('feature-toggle-testid')).toBeInTheDocument();
      delete process.env.REACT_APP_TENANT;
    });

    it(`should hide the content for other tenants`, () => {
      process.env.REACT_APP_TENANT = 'CITZ';
      const { queryByTestId } = withRenderProps();
      expect(queryByTestId('feature-toggle-testid')).not.toBeInTheDocument();
      delete process.env.REACT_APP_TENANT;
    });
  });
});
