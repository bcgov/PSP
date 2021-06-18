import { cleanup, render } from '@testing-library/react';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import MapSideBarContainer from './MapSideBarContainer';

const renderContainer = () =>
  render(
    <TestCommonWrapper>
      <MapSideBarContainer />
    </TestCommonWrapper>,
  );
describe('MapSideBarContainer functionality', () => {
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'MOTI';
  });
  afterEach(() => {
    delete process.env.REACT_APP_TENANT;
    cleanup();
  });
  afterAll(() => {
    jest.resetModules();
  });
  it('Displays MOTI sidebar data if MOTI is the tenant', () => {
    const { getByText } = renderContainer();
    expect(getByText('Add Titled Property to Inventory')).toBeInTheDocument();
  });
});
