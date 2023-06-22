import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as MOCK from '@/mocks/data.mock';
import { Api_ResearchFile } from '@/models/api/ResearchFile';
import { networkSlice } from '@/store/slices/network/networkSlice';

import { useAddResearch } from './useAddResearch';

const testResearchFile: Api_ResearchFile = {
  id: 1,
  fileNumber: 'RFile-0123456789',
};

const dispatch = jest.fn();
const toastSuccessSpy = jest.spyOn(toast, 'success');
const requestSpy = jest.spyOn(networkSlice.actions, 'logRequest');
const successSpy = jest.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = jest.spyOn(networkSlice.actions, 'logError');
const mockAxios = new MockAdapter(axios);

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

describe('useAddResearch functions', () => {
  const setup = (values?: any) => {
    const { result } = renderHook(useAddResearch, { wrapper: getWrapper(getStore(values)) });
    return result.current;
  };

  beforeEach(() => {
    mockAxios.reset();
    dispatch.mockClear();
    requestSpy.mockClear();
    successSpy.mockClear();
    errorSpy.mockClear();
  });

  afterAll(() => {
    jest.restoreAllMocks();
  });

  describe('addResearch', () => {
    const url = /researchFiles.*/;
    it('Request successful, dispatches success with correct response', async () => {
      mockAxios.onPost(url).reply(200, testResearchFile);
      const { addResearchFile } = setup();

      await act(async () => {
        const response = await addResearchFile(testResearchFile);
        expect(response).toEqual(testResearchFile);
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
      expect(toastSuccessSpy).toHaveBeenCalled();
    });

    it('Request failure, dispatches error with correct response', async () => {
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      const { addResearchFile } = setup();

      expect(async () => {
        await addResearchFile(testResearchFile);
      }).rejects.toThrow();
    });
  });
});
