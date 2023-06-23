import React from 'react';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';

interface TestProviderWrapperParams {
  children: React.ReactNode;
  store?: any;
}

/** Simple Wrapper that provides all required boilerplate to include a mocked store for a given component test. */
const TestProviderWrapper: React.FunctionComponent<
  React.PropsWithChildren<TestProviderWrapperParams>
> = ({ children, store }) => {
  const mockStore = configureMockStore([thunk]);
  const mockedStore = !!store?.getState
    ? store
    : mockStore(store ?? { [lookupCodesSlice.name]: mockLookups });

  return <Provider store={mockedStore}>{children}</Provider>;
};

export default TestProviderWrapper;
