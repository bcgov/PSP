import React from 'react';
import DocumentationStep from './DocumentationStep';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import MockAdapter from 'axios-mock-adapter';
import axios from 'axios';
import { render, wait, fireEvent, cleanup } from '@testing-library/react';
import { DisposeWorkflowStatus, IProjectTask } from '../../common/interfaces';
import { useKeycloak } from '@react-keycloak/web';
import { projectSlice, projectTasksSlice } from 'features/projects/common';
import { ProjectActions } from 'features/projects/common/slices/projectActions';
import { networkSlice } from 'store/slices/network/networkSlice';

const mockAxios = new MockAdapter(axios);
mockAxios.onAny().reply(200, {});
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

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const tasks: IProjectTask[] = [
  {
    projectNumber: 1,
    isCompleted: false,
    isOptional: false,
    completedOn: new Date(),
    taskId: 1,
    name: 'task-0',
    sortOrder: 0,
    description: 'Task #1',
    taskType: 1,
    statusId: 0,
    statusCode: DisposeWorkflowStatus.RequiredDocumentation,
  },
  {
    projectNumber: 1,
    isCompleted: false,
    isOptional: false,
    completedOn: new Date(),
    taskId: 2,
    name: 'task-1',
    sortOrder: 0,
    description: 'Task #2',
    taskType: 1,
    statusId: 0,
    statusCode: DisposeWorkflowStatus.RequiredDocumentation,
  },
];

const store = mockStore({
  [projectSlice.name]: { project: { tasks: tasks } },
  [projectTasksSlice.name]: tasks,
  [networkSlice.name]: {
    [ProjectActions.GET_PROJECT]: {},
  },
});

const uiElement = (
  <Provider store={store}>
    <Router history={history}>
      <DocumentationStep />
    </Router>
  </Provider>
);

describe('Documentation Step', () => {
  afterEach(() => {
    cleanup();
  });
  it('renders correctly', () => {
    const { container } = render(uiElement);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('renders correct labels', () => {
    const { getByText } = render(uiElement);
    expect(getByText('Task #1')).toBeInTheDocument();
    expect(getByText('Task #2')).toBeInTheDocument();
  });

  it('documentation validation works', async () => {
    const { getAllByText, container } = render(uiElement);
    const form = container.querySelector('form');
    await wait(() => {
      fireEvent.submit(form!);
    });
    expect(getAllByText('Required')).toHaveLength(2);
  });
});
