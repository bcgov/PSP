import { render, screen, waitFor } from '@testing-library/react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import TestCommonWrapper from '@/utils/TestCommonWrapper';

import HelpModalContentContainer from './HelpModalContentContainer';

const mockStore = configureMockStore([thunk]);
const store = mockStore({});

const renderComponent = () =>
  render(
    <TestCommonWrapper store={store}>
      <HelpModalContentContainer setMailto={jest.fn()} />
    </TestCommonWrapper>,
  );
it('HelpContainer renders correctly...', async () => {
  process.env.REACT_APP_TENANT = 'MOTI';
  const { asFragment } = render(
    <TestCommonWrapper store={store} claims={['test']}>
      <HelpModalContentContainer setMailto={jest.fn()} />
    </TestCommonWrapper>,
  );
  const fragment = await waitFor(() => asFragment());
  expect(fragment).toMatchSnapshot();
});

it('Populates from keycloak email correctly...', async () => {
  renderComponent();
  const email = await waitFor(() => screen.getByDisplayValue('test@test.com'));
  expect(email).toBeInTheDocument();
});

it('Populates name from keycloak correctly...', async () => {
  renderComponent();
  const name = await waitFor(() => screen.getByDisplayValue('Chester Tester'));
  expect(name).toBeInTheDocument();
});
