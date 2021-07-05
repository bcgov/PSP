import React from 'react';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

interface TestProviderWrapperParams {
  children: React.ReactNode;
  store?: any;
}

/** Simple Wrapper that provides all required boilerplate to include a mocked store for a given component test. */
const TestProviderWrapper: React.FunctionComponent<TestProviderWrapperParams> = ({
  children,
  store,
}) => {
  const mockStore = configureMockStore([thunk]);
  const getStore = () => (!!store?.getState ? store : mockStore(store ?? {}));

  return <Provider store={getStore()}>{children}</Provider>;
};

export default TestProviderWrapper;
