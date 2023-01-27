import { cleanup } from '@testing-library/react';
import renderer from 'react-test-renderer';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import HelpContainer from './HelpContainer';

const mockStore = configureMockStore([thunk]);
const store = mockStore({});
const mockData = {
  config: { settings: { helpDeskEmail: 'test@test.com' } },
};

jest.mock('store/hooks', () => ({
  useAppSelector: () => mockData,
  useAppDispatch: () => jest.fn(),
}));

afterEach(() => {
  cleanup();
});
describe('HelpContainer tests', () => {
  beforeEach(() => {
    jest.resetModules();
  });

  it('HelpContainer renders correctly...', () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    const tree = renderer
      .create(
        <TestCommonWrapper store={store} claims={['test']}>
          <HelpContainer />
        </TestCommonWrapper>,
      )
      .toJSON();
    expect(tree).toMatchSnapshot();
  });
});
