import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import find from 'lodash/find';
import { Provider } from 'react-redux';
import { toast } from 'react-toastify';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { NoteTypes } from '@/constants/index';
import * as MOCK from '@/mocks/data.mock';
import { mockEntityNote, mockNoteResponse } from '@/mocks/noteResponses.mock';
import { networkSlice } from '@/store/slices/network/networkSlice';

import { useNoteRepository } from './useNoteRepository';

const dispatch = jest.fn();
const toastSuccessSpy = jest.spyOn(toast, 'success');
const toastErrorSpy = jest.spyOn(toast, 'error');
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

describe('useNoteRepository hook', () => {
  const setup = (values?: any) => {
    const { result } = renderHook(useNoteRepository, { wrapper: getWrapper(getStore(values)) });
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

  let url: string;

  describe('addNote', () => {
    beforeEach(() => {
      url = `/notes/activity`;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onPost(url).reply(200, mockEntityNote(1));
      const { addNote } = setup();

      await act(async () => {
        const response = await addNote.execute(NoteTypes.Activity, mockEntityNote());
        expect(response).toEqual(mockEntityNote(1));
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
      expect(toastSuccessSpy).toHaveBeenCalledWith('Note saved');
    });

    it('Dispatches error with correct response when request fails', async () => {
      mockAxios.onPost(url).reply(400, MOCK.ERROR);
      const { addNote } = setup();

      await act(async () => {
        await addNote.execute(NoteTypes.Activity, mockEntityNote());
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });

  describe('getNote', () => {
    beforeEach(() => {
      url = `/notes/1`;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onGet(url).reply(200, mockNoteResponse(1));
      const { getNote } = setup();

      await act(async () => {
        const response = await getNote.execute(1);
        expect(response).toEqual(mockNoteResponse(1));
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
    });

    it('Dispatches error with correct response when request fails', async () => {
      mockAxios.onGet(url).reply(400, MOCK.ERROR);
      const { getNote } = setup();

      await act(async () => {
        await getNote.execute(mockEntityNote().id || -1);
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });

  describe('updateNote', () => {
    beforeEach(() => {
      url = `/notes/1`;
    });

    it('Dispatches success with correct response when request is successful', async () => {
      mockAxios.onPut(url).reply(200, mockNoteResponse(1));
      const { updateNote } = setup();

      await act(async () => {
        const response = await updateNote.execute(mockNoteResponse(1));
        expect(response).toEqual(mockNoteResponse(1));
      });

      expect(find(currentStore.getActions(), { type: 'loading-bar/SHOW' })).toBeDefined();
      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeUndefined();
      expect(toastSuccessSpy).toHaveBeenCalledWith('Note saved');
    });

    it('Dispatches error with correct response when request fails', async () => {
      mockAxios.onPut(url).reply(400, MOCK.ERROR);
      const { updateNote } = setup();

      await act(async () => {
        await updateNote.execute(mockNoteResponse(1));
      });

      expect(find(currentStore.getActions(), { type: 'network/logError' })).toBeDefined();
      expect(toastErrorSpy).toHaveBeenCalled();
    });
  });
});
