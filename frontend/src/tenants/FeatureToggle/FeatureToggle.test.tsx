import React from 'react';
import { cleanup, render } from '@testing-library/react';
import { TenantProvider } from 'tenants';
import { FeatureToggle } from './';

const renderFeatureToggle = (tenant: string, hide: boolean = false) =>
  render(
    <TenantProvider>
      <FeatureToggle tenant={tenant} hide={hide}>
        <div data-testid="feature-toggle-testid"></div>
      </FeatureToggle>
    </TenantProvider>,
  );

describe('FeatureToggle', () => {
  describe('when tenant matches the current tenant', () => {
    beforeEach(() => {
      process.env.REACT_APP_TENANT = 'MOTI';
    });
    afterEach(() => {
      delete process.env.REACT_APP_TENANT;
      cleanup();
    });

    it('renders content', async () => {
      const { findByTestId } = renderFeatureToggle('MOTI');
      expect(await findByTestId('feature-toggle-testid')).toBeInTheDocument();
    });

    describe(`when 'hide' prop is set to true`, () => {
      it(`doesn't render content`, () => {
        const { queryByTestId } = renderFeatureToggle('MOTI', true);
        expect(queryByTestId('feature-toggle-testid')).not.toBeInTheDocument();
      });
    });
  });

  describe(`when tenant doesn't match the current tenant`, () => {
    beforeEach(() => {
      process.env.REACT_APP_TENANT = 'CITZ';
    });
    afterEach(() => {
      delete process.env.REACT_APP_TENANT;
      cleanup();
    });

    it(`doesn't render content`, () => {
      const { queryByTestId } = renderFeatureToggle('MOTI');
      expect(queryByTestId('feature-toggle-testid')).not.toBeInTheDocument();
    });

    describe(`when 'hide' prop is set to true`, () => {
      it(`renders content`, () => {
        const { queryByTestId } = renderFeatureToggle('MOTI', true);
        expect(queryByTestId('feature-toggle-testid')).toBeInTheDocument();
      });
    });
  });
});
