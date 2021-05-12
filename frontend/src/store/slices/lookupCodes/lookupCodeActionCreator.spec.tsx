import React from 'react';
import MockAdapter from 'axios-mock-adapter';
import axios from 'axios';
import * as genericActions from 'actions/genericActions';
import * as API from 'constants/API';
import * as MOCK from 'mocks/dataMocks';
import { ENVIRONMENT } from 'constants/environment';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { useLookupCodes } from 'store/slices/lookupCodes/useLookupCodes';
import { renderHook } from '@testing-library/react-hooks';
import { AGENCY_CODE_SET_NAME } from 'constants/API';
import { STORE_LOOKUP_CODE_RESULTS } from 'constants/actionTypes';
import { Provider } from 'react-redux';

const requestSpy = jest.spyOn(genericActions, 'request');
const successSpy = jest.spyOn(genericActions, 'success');
const errorSpy = jest.spyOn(genericActions, 'error');
const mockAxios = new MockAdapter(axios);

beforeEach(() => {
  mockAxios.reset();
  requestSpy.mockClear();
  successSpy.mockClear();
  errorSpy.mockClear();
});

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper = (store: any) => ({ children }: any) => (
  <Provider store={store}>{children}</Provider>
);

describe('getFetchLookupCodeAction action creator', () => {
  it('gets all codes when paramaters contains all', () => {
    const url = ENVIRONMENT.apiUrl + API.LOOKUP_CODE_SET('all');
    const mockResponse = {
      data: [
        {
          code: 'AEST',
          id: '1',
          isDisabled: false,
          name: 'Ministry of Advanced Education',
          type: AGENCY_CODE_SET_NAME,
        },
      ],
    };
    mockAxios.onGet(url).reply(200, mockResponse);
    renderHook(
      () =>
        useLookupCodes()
          .fetchLookupCodes()
          .then(() => {
            expect(requestSpy).toHaveBeenCalledTimes(1);
            expect(successSpy).toHaveBeenCalledTimes(1);
            expect(currentStore.getActions()).toContainEqual({
              payload: mockResponse,
              type: 'lookupCode/storeLookupCodes',
            });
          }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
  });

  it('Request failure, dispatches error with correct response', () => {
    const url = ENVIRONMENT.apiUrl + API.LOOKUP_CODE_SET('all');
    mockAxios.onGet(url).reply(400, MOCK.ERROR);
    const mockResponse = {
      data: [
        {
          code: 'AEST',
          id: '1',
          isDisabled: false,
          name: 'Ministry of Advanced Education',
          type: AGENCY_CODE_SET_NAME,
        },
      ],
    };
    renderHook(
      () =>
        useLookupCodes()
          .fetchLookupCodes()
          .then(() => {
            expect(requestSpy).toHaveBeenCalledTimes(1);
            expect(errorSpy).toHaveBeenCalledTimes(1);
            expect(currentStore.getActions()).not.toContainEqual({
              payload: mockResponse,
              type: STORE_LOOKUP_CODE_RESULTS,
            });
          }),
      {
        wrapper: getWrapper(getStore()),
      },
    );
  });
});
