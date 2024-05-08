import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as MOCK from '@/mocks/data.mock';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { networkSlice } from '@/store/slices/network/networkSlice';

import { useAddResearch } from './useAddResearch';

const testResearchFile: ApiGen_Concepts_ResearchFile = {
  id: 1,
  fileNumber: 'RFile-0123456789',
  roadName: null,
  roadAlias: null,
  fileProperties: null,
  requestDate: null,
  requestDescription: null,
  requestSourceDescription: null,
  researchResult: null,
  researchCompletionDate: null,
  isExpropriation: null,
  expropriationNotes: null,
  requestSourceType: null,
  requestorPerson: null,
  requestorOrganization: null,
  researchFilePurposes: null,
  researchFileProjects: null,
  fileName: null,
  fileStatusTypeCode: null,
  appCreateTimestamp: '',
  appLastUpdateTimestamp: '',
  appLastUpdateUserid: null,
  appCreateUserid: null,
  appLastUpdateUserGuid: null,
  appCreateUserGuid: null,
  rowVersion: null,
};

const dispatch = vi.fn();
const toastSuccessSpy = vi.spyOn(toast, 'success');
const requestSpy = vi.spyOn(networkSlice.actions, 'logRequest');
const successSpy = vi.spyOn(networkSlice.actions, 'logSuccess');
const errorSpy = vi.spyOn(networkSlice.actions, 'logError');
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
    vi.restoreAllMocks();
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

      await act(async () => {
        expect(async () => {
          await addResearchFile(testResearchFile);
        }).rejects.toThrow();
      });
    });
  });
});
