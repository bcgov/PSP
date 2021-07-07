import { act, render, screen } from '@testing-library/react';
import renderer from 'react-test-renderer';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import HelpModalContentContainer from './HelpModalContentContainer';

const mockStore = configureMockStore([thunk]);
const store = mockStore({});

const renderHeader = () =>
  render(
    <TestCommonWrapper store={store}>
      <HelpModalContentContainer setMailto={jest.fn()} />
    </TestCommonWrapper>,
  );
it('HelpContainer renders correctly...', () => {
  // (useKeycloak as jest.Mock).mockReturnValue({ keycloak: { authenticated: false } });
  process.env.REACT_APP_TENANT = 'MOTI';
  const tree = renderer
    .create(
      <TestCommonWrapper store={store} roles={[{ id: '1', name: 'test' }]}>
        <HelpModalContentContainer setMailto={jest.fn()} />
      </TestCommonWrapper>,
    )
    .toJSON();
  expect(tree).toMatchSnapshot();
});

it('Populates from keycloak email correctly...', async () => {
  await act(async () => {
    renderHeader();
    const email = screen.getByDisplayValue('test@test.com');
    expect(email).toBeInTheDocument();
  });
});

it('Populates name from keycloak correctly...', async () => {
  await act(async () => {
    renderHeader();
    const name = screen.getByDisplayValue('Chester Tester');
    expect(name).toBeInTheDocument();
  });
});
