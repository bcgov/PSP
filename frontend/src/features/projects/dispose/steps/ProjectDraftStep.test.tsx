import React from 'react';
import ProjectDraftStep from './ProjectDraftStep';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import MockAdapter from 'axios-mock-adapter';
import axios from 'axios';
import { fillInput } from 'utils/testUtils';
import useStepper from '../hooks/useStepper';
import { noop } from 'lodash';
import { render, screen, cleanup, wait } from '@testing-library/react';
import { useKeycloak } from '@react-keycloak/web';
import { projectSlice } from 'features/projects/common';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';
import { ProjectActions } from 'features/projects/common/slices/projectActions';

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      agencies: [1],
      roles: [],
    },
    subject: 'test',
  },
});

const mockAxios = new MockAdapter(axios);
jest.mock('../hooks/useStepper');
(useStepper as jest.Mock).mockReturnValue({
  currentStatus: {},
  project: { agencyId: 1, projectNumber: 'TEST-NUMBER', properties: [] },
  projectStatusCompleted: noop,
  canGoToStatus: noop,
  getLastCompletedStatus: jest.fn(),
  getNextStep: jest.fn(),
});
mockAxios.onAny().reply(200, {});

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const store = mockStore({
  [projectSlice.name]: { agencyId: 1 },
  [lookupCodesSlice.name]: { lookupCodes: [] },
  [networkSlice.name]: {
    [ProjectActions.GET_PROJECT]: {},
  },
});

const uiElement = (
  <Provider store={store}>
    <Router history={history}>
      <ProjectDraftStep />
    </Router>
  </Provider>
);

describe('Project Draft Step', () => {
  afterEach(() => {
    cleanup();
  });
  it('renders correctly', () => {
    const { container } = render(uiElement);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('requires name', async () => {
    const { container, findByText } = render(uiElement);
    container.querySelector('form');
    await fillInput(container, 'name', '');
    await fillInput(container, 'description', 'description', 'textarea');
    await wait(async () => {
      expect(await findByText('Required')).toBeInTheDocument();
    });
  });

  it('can be submitted after required filled', async () => {
    const { container, findByText } = render(uiElement);
    container.querySelector('form');
    await fillInput(container, 'name', '');
    await fillInput(container, 'description', 'description', 'textarea');
    await wait(async () => {
      expect(await findByText('Required')).toBeInTheDocument();
    });
  });

  it('loads the projectNumber', () => {
    render(uiElement);
    expect(screen.getByDisplayValue('TEST-NUMBER')).toBeInTheDocument();
  });
});
