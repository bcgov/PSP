import { fireEvent, waitForElementToBeRemoved } from '@testing-library/dom';
import { render } from '@testing-library/react';
import { createMemoryHistory } from 'history';
import { enableFetchMocks } from 'jest-fetch-mock';
import { Route } from 'react-router-dom';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import MotiInventoryContainer from './MotiInventoryContainer';

enableFetchMocks();

const history = createMemoryHistory({
  getUserConfirmation: (message, callback) => {
    callback(true);
  },
});

const renderContainer = ({ store }: any) =>
  render(
    <TestCommonWrapper history={history}>
      <Route path="/mapview/:id?">
        <MotiInventoryContainer />
      </Route>
    </TestCommonWrapper>,
  );

describe('MotiInventoryContainer functionality', () => {
  beforeEach(() => {
    fetchMock.mockResponse(JSON.stringify({ status: 200, body: {} }));
    history.push('');
  });
  afterAll(() => {
    jest.restoreAllMocks();
  });
  it('displays as expected', () => {
    history.push('/mapview?sidebar=true');
    const { container } = renderContainer({});
    expect(container.firstChild).toMatchSnapshot();
  });
  it('does not display by default', () => {
    const { container } = renderContainer({});
    const sidebar = container.querySelector('.map-side-drawer');
    expect(sidebar).toBeNull();
  });

  it('displays when sidebar query param is true', () => {
    history.push('/mapview?sidebar=true');
    const { getByText } = renderContainer({});
    expect(getByText('Add to Inventory')).toBeInTheDocument();
  });
  it('does not display when close icon clicked', async () => {
    history.push('/mapview?sidebar=true');
    const { getByTitle, getByText } = renderContainer({});
    const closeTitleButton = getByTitle('close');
    fireEvent.click(closeTitleButton);
    waitForElementToBeRemoved(getByText('Add to Inventory'));
  });
});
