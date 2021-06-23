import { cleanup, render } from '@testing-library/react';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import MapSideBarContainer from './MapSideBarContainer';

const renderContainer = () =>
  render(
    <TestCommonWrapper agencies={[1 as any]} store={{ lookupCode: { lookupCodes: [] } }}>
      <MapSideBarContainer />
    </TestCommonWrapper>,
  );
describe('MapSideBarContainer functionality', () => {
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'CITZ';
  });
  afterEach(() => {
    delete process.env.REACT_APP_TENANT;
    cleanup();
  });
  afterAll(() => {
    jest.resetModules();
  });
  it('Displays CITZ sidebar data if CITZ is the tenant', () => {
    const { getByText } = renderContainer();
    expect(getByText('Submit a Property')).toBeInTheDocument();
  });
});
