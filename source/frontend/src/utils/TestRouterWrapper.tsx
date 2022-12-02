import { createMemoryHistory, MemoryHistory } from 'history';
import React from 'react';
import { Router } from 'react-router-dom';

interface TestRouterWrapperParams {
  children: React.ReactNode;
  history?: MemoryHistory;
}

const history = createMemoryHistory({
  getUserConfirmation: (message, callback) => {
    callback(true);
  },
});

/** Simple Wrapper that provides all required boilerplate to include a mocked memory history router for a given component test. */
const TestRouterWrapper: React.FunctionComponent<
  React.PropsWithChildren<TestRouterWrapperParams>
> = ({ children, history: paramHistory }) => {
  return <Router history={paramHistory ?? history}>{children}</Router>;
};

export default TestRouterWrapper;
