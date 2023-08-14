import { renderHook } from '@testing-library/react-hooks';
import { createMemoryHistory } from 'history';
import queryString from 'query-string';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { PropertyClassificationTypes } from '@/constants/propertyClassificationTypes';
import filterSlice from '@/store/slices/filter/filterSlice';

import { useRouterFilter } from './useRouterFilter';

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();
const getStore = (filter: any) =>
  mockStore({
    [filterSlice.name]: filter,
  });

const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    (
      <Provider store={store}>
        <Router history={history}>{children}</Router>
      </Provider>
    );

const emptyFilter = {
  address: '',
  latitude: '',
  longitude: '',
  page: undefined,
  pinOrPid: '',
  planNumber: '',
  quantity: undefined,
  searchBy: 'address',
};

const defaultFilter = {
  searchBy: 'address',
  pid: '1',
  pin: '',
  address: '2',
  administrativeArea: '3',
  organizations: '5',
  page: '',
  classificationId: `${PropertyClassificationTypes.Subdivided}`,
  minLotSize: '7',
  maxLotSize: '8',
  parcelId: '9',
  rentableArea: '',
  pinOrPid: '',
  planNumber: '',
  quantity: '',
  maxAssessedValue: '',
  maxMarketValue: '',
  maxNetBookValue: '',
  name: '',
  latitude: '',
  longitude: '',
};

let filter: any = defaultFilter;
const setFilter = (f: any) => {
  filter = f;
};

describe('useRouterFilter hook tests', () => {
  beforeEach(() => {
    filter = defaultFilter;
    history.push({});
  });

  it('will set the filter based on a query string', () => {
    const expectedFilter = { ...defaultFilter, pid: '2' };
    history.push({ search: new URLSearchParams(expectedFilter).toString() });

    const wrapper = getWrapper(getStore({}));
    renderHook(() => useRouterFilter({ filter, setFilter, key: 'test' }), { wrapper });
    expect(filter).toEqual(expectedFilter);
  });

  it('will not reset the query string', () => {
    const expectedFilter = { ...defaultFilter, pid: '2' };
    history.push({ search: new URLSearchParams(expectedFilter).toString() });

    const filterWithValues: any = { ...expectedFilter };
    Object.keys(filterWithValues).forEach(
      k => filterWithValues[k] === '' && delete filterWithValues[k],
    );

    const wrapper = getWrapper(getStore({}));
    renderHook(() => useRouterFilter({ filter, setFilter, key: 'test' }), { wrapper });
    expect(history.location.search).toEqual('?' + queryString.stringify(filterWithValues));
  });

  it('will not set the filter based on an invalid query string', () => {
    history.push({ search: new URLSearchParams({ searchBy: 'address' }).toString() });

    const wrapper = getWrapper(getStore({}));
    renderHook(() => useRouterFilter({ filter, setFilter, key: 'test' }), { wrapper });
    expect(filter).toEqual(emptyFilter);
  });

  it('will set the filter based on redux', () => {
    const expectedFilter = { ...defaultFilter, pid: '2' };
    const wrapper = getWrapper(getStore({ test: expectedFilter }));
    renderHook(() => useRouterFilter({ filter, setFilter, key: 'test' }), {
      wrapper,
    });
    expect(filter).toEqual(expectedFilter);
  });

  it.skip('will not set the filter based on redux if there is no matching key', () => {
    const wrapper = getWrapper(getStore({ test: defaultFilter }));
    renderHook(() => useRouterFilter({ filter, setFilter, key: 'mismatch' }), { wrapper });
    expect(filter).toEqual(defaultFilter);
  });

  it('will set the location based on a passed filter', () => {
    const wrapper = getWrapper(getStore({ test: defaultFilter }));
    renderHook(() => useRouterFilter({ filter: defaultFilter, setFilter, key: 'mismatch' }), {
      wrapper,
    });
    const filterWithValues: any = { ...defaultFilter };
    Object.keys(filterWithValues).forEach(
      k => filterWithValues[k] === '' && delete filterWithValues[k],
    );
    expect(history.location.search).toEqual('?' + queryString.stringify(filterWithValues));
  });
});
