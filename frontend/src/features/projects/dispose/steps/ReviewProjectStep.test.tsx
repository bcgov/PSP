import { useKeycloak } from '@react-keycloak/web';
import { ProjectActions } from 'features/projects/common/slices/projectActions';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import renderer from 'react-test-renderer';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';

import {
  DisposeWorkflowStatus,
  IProject,
  IProjectTask,
  ITask,
  projectSlice,
  projectTasksSlice,
} from '../../common';
import ReviewProjectStep from './ReviewProjectStep';

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

const mockTasks: IProjectTask[] = [
  {
    projectNumber: 123,
    taskId: 1,
    isOptional: true,
    isCompleted: true,
    name: 'task-0',
    description: 'one',
    taskType: 1,
    sortOrder: 0,
    completedOn: new Date(),
    statusId: 0,
    statusCode: DisposeWorkflowStatus.RequiredDocumentation,
  },
  {
    projectNumber: 123,
    taskId: 2,
    isOptional: true,
    isCompleted: true,
    name: 'task-1',
    description: 'two',
    taskType: 1,
    sortOrder: 0,
    completedOn: new Date(),
    statusId: 0,
    statusCode: DisposeWorkflowStatus.RequiredDocumentation,
  },
];

const mockProject: IProject = {
  projectNumber: 'test-01',
  name: 'my project',
  description: 'my project description',
  properties: [],
  agencyId: 1,
  statusId: 0,
  statusCode: DisposeWorkflowStatus.RequiredDocumentation,
  tierLevelId: 1,
  tasks: mockTasks,
  note: 'my notes',
  id: 1,
  fiscalYear: 2020,
  projectAgencyResponses: [],
  publicNote: 'public',
  privateNote: 'private',
  notes: [],
};

export const tasks: ITask[] = [
  {
    taskId: 1,
    name: 'task-0',
    sortOrder: 0,
    description: 'test',
    taskType: 1,
  },
  {
    taskId: 2,
    name: 'task-1',
    sortOrder: 0,
    description: 'test',
    taskType: 1,
  },
];

const store = mockStore({
  [lookupCodesSlice.name]: { lookupCodes: [] },
  [projectSlice.name]: mockProject,
  [projectTasksSlice.name]: tasks,
  [networkSlice.name]: {
    [ProjectActions.GET_PROJECT]: {},
  },
});

const uiElement = (
  <Provider store={store}>
    <Router history={history}>
      <ReviewProjectStep />
    </Router>
  </Provider>
);

it('renders correctly', () => {
  const tree = renderer.create(uiElement).toJSON();
  expect(tree).toMatchSnapshot();
});
