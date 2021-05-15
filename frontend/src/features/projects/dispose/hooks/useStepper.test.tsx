import React from 'react';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import { renderHook } from '@testing-library/react-hooks';
import useStepper, {
  isStatusCompleted,
  isStatusNavigable,
  getLastCompletedStatus,
  getNextWorkflowStatus,
} from './useStepper';
import { IProject, projectSlice } from 'features/projects/common';
import { mockWorkflow } from '../testUtils';
import { ProjectActions } from 'features/projects/common/slices/projectActions';
import projectWorkflowSlice from 'features/projects/common/slices/projectWorkflowSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const store = mockStore({
  [projectSlice.name]: {},
  [projectWorkflowSlice.name]: mockWorkflow,
  [networkSlice.name]: {
    [ProjectActions.GET_PROJECT]: {
      isFetching: false,
    },
  },
  [lookupCodesSlice.name]: { lookupCodes: [] },
});

describe('useStepper hook functionality', () => {
  beforeAll(() => {
    renderHook(() => useStepper(), {
      wrapper: ({ children }) => (
        <Provider store={store}>
          <Router history={history}>{children}</Router>
        </Provider>
      ),
    });
  });
  afterAll(() => {
    jest.clearAllMocks();
  });
  describe('getNextWorkflowStatus', () => {
    it('draft step', () => {
      const nextStep = getNextWorkflowStatus(mockWorkflow, mockWorkflow[0]);
      expect(nextStep).toBe(mockWorkflow[1]);
    });

    it('review step', () => {
      const nextStep = getNextWorkflowStatus(mockWorkflow, mockWorkflow[5]);
      expect(nextStep).toBe(undefined);
    });

    it('undefined step', () => {
      const nextStep = getNextWorkflowStatus(mockWorkflow, undefined);
      expect(nextStep).toBe(mockWorkflow[0]);
    });
  });
  describe('isStatusCompleted', () => {
    it('true if current status less then project status', () => {
      const isComplete = isStatusCompleted(mockWorkflow, mockWorkflow[0], {
        statusId: 2,
      } as IProject);
      expect(isComplete).toBe(true);
    });
    it('false if current status equal to project status', () => {
      const isComplete = isStatusCompleted(mockWorkflow, mockWorkflow[0], {
        statusId: 1,
      } as IProject);
      expect(isComplete).toBe(false);
    });

    it('false if current status greater then project status', () => {
      const isComplete = isStatusCompleted(mockWorkflow, mockWorkflow[1], {
        statusId: 1,
      } as IProject);
      expect(isComplete).toBe(false);
    });

    it('false if passed invalid data', () => {
      const isComplete = isStatusCompleted(mockWorkflow, mockWorkflow[1], undefined);
      expect(isComplete).toBe(false);
    });
  });
  describe('isStatusNavigable', () => {
    it('true if status is equal to project status', () => {
      const isNavigable = isStatusNavigable(mockWorkflow, mockWorkflow[0], {
        statusId: 1,
      } as IProject);
      expect(isNavigable).toBe(true);
    });

    it('false if status is greater then project status', () => {
      const isNavigable = isStatusNavigable(mockWorkflow, mockWorkflow[2], {
        statusId: 1,
      } as IProject);
      expect(isNavigable).toBe(false);
    });

    it('true if status is less then project status', () => {
      const isNavigable = isStatusNavigable(mockWorkflow, mockWorkflow[0], {
        statusId: 2,
      } as IProject);
      expect(isNavigable).toBe(true);
    });

    it('false if project is undefined and not at draft status', () => {
      const isNavigable = isStatusNavigable(mockWorkflow, mockWorkflow[1], undefined);
      expect(isNavigable).toBe(false);
    });

    it('true if project is undefined and at draft status', () => {
      const isNavigable = isStatusNavigable(mockWorkflow, mockWorkflow[0], undefined);
      expect(isNavigable).toBe(true);
    });
  });
  describe('getLastCompletedStatus', () => {
    it('returns current status if status is equal to project status', () => {
      const lastCompletedStatus = getLastCompletedStatus(mockWorkflow, mockWorkflow[0], {
        statusId: 1,
      } as IProject);
      expect(lastCompletedStatus).toBe(mockWorkflow[0]);
    });

    it('returns project status if status is less then project status', () => {
      const lastCompletedStatus = getLastCompletedStatus(mockWorkflow, mockWorkflow[0], {
        statusId: 2,
      } as IProject);
      expect(lastCompletedStatus).toBe(mockWorkflow[1]);
    });

    it('returns current status if status is greater then project status', () => {
      const lastCompletedStatus = getLastCompletedStatus(mockWorkflow, mockWorkflow[1], {
        statusId: 1,
      } as IProject);
      expect(lastCompletedStatus).toBe(mockWorkflow[1]);
    });

    it('returns undefined if current status is undefined', () => {
      const lastCompletedStatus = getLastCompletedStatus(
        mockWorkflow,
        undefined as any,
        {
          statusId: 1,
        } as IProject,
      );
      expect(lastCompletedStatus).toBe(undefined);
    });
  });
});
