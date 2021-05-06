import MockAdapter from 'axios-mock-adapter';
import axios from 'axios';
import * as genericActions from 'actions/genericActions';
import * as API from 'constants/API';
import * as MOCK from 'mocks/dataMocks';
import { ENVIRONMENT } from 'constants/environment';
import { createAgency, getAgenciesAction, deleteAgency } from './agencyActionCreator';

const dispatch = jest.fn();
const requestSpy = jest.spyOn(genericActions, 'request');
const successSpy = jest.spyOn(genericActions, 'success');
const errorSpy = jest.spyOn(genericActions, 'error');
const mockAxios = new MockAdapter(axios);

afterEach(() => {
  mockAxios.reset();
  dispatch.mockClear();
  requestSpy.mockClear();
  successSpy.mockClear();
  errorSpy.mockClear();
});

describe('getAgenciesAction action creator', () => {
  it('Request successful, dispatches `success` with correct response', () => {
    const url = ENVIRONMENT.apiUrl + API.POST_AGENCIES();
    const mockResponse = { data: { success: true } };
    mockAxios.onPost(url).reply(200, mockResponse);
    return getAgenciesAction({} as any)(dispatch).then(() => {
      expect(requestSpy).toHaveBeenCalledTimes(1);
      expect(successSpy).toHaveBeenCalledTimes(1);
      expect(dispatch).toHaveBeenCalledTimes(6);
    });
  });

  it('Request failure, dispatches `error` with correct response', () => {
    const url = ENVIRONMENT.apiUrl + API.POST_AGENCIES();
    mockAxios.onGet(url).reply(400, MOCK.ERROR);
    return getAgenciesAction({} as any)(dispatch).then(() => {
      expect(requestSpy).toHaveBeenCalledTimes(1);
      expect(errorSpy).toHaveBeenCalledTimes(1);
      expect(dispatch).toHaveBeenCalledTimes(4);
    });
  });
});

describe('createAgency action creator', () => {
  it('Request successful, dispatches `success` with correct response', () => {
    const agency = { id: 0 } as any;
    const url = ENVIRONMENT.apiUrl + API.AGENCY_ROOT();
    const mockResponse = { data: { success: true } };

    mockAxios.onPost(url).reply(200, mockResponse);
    return createAgency(agency)(dispatch).then(() => {
      expect(requestSpy).toHaveBeenCalledTimes(1);
      expect(successSpy).toHaveBeenCalledTimes(1);
      expect(dispatch).toHaveBeenCalledTimes(4);
    });
  });

  it('Request failure, dispatches `error` with correct response', () => {
    const agency = { id: 0 } as any;
    const url = ENVIRONMENT.apiUrl + API.AGENCY_ROOT();
    mockAxios.onPost(url).reply(400, MOCK.ERROR);
    return createAgency(agency)(dispatch).catch(() => {
      expect(requestSpy).toHaveBeenCalledTimes(1);
      expect(errorSpy).toHaveBeenCalledTimes(1);
      expect(dispatch).toHaveBeenCalledTimes(4);
    });
  });
});

describe('deleteAgency action creator', () => {
  it('Request successful, dispatches `success` with correct response', () => {
    const agency = { id: 0 } as any;
    const url = ENVIRONMENT.apiUrl + API.AGENCY_ROOT() + '0';
    const mockResponse = { data: { success: true } };

    mockAxios.onDelete(url).reply(200, mockResponse);
    return deleteAgency(agency)(dispatch).then(() => {
      expect(requestSpy).toHaveBeenCalledTimes(1);
      expect(successSpy).toHaveBeenCalledTimes(1);
      expect(dispatch).toHaveBeenCalledTimes(5);
    });
  });

  it('Request failure, dispatches `error` with correct response', () => {
    const agency = { id: 0 } as any;
    const url = ENVIRONMENT.apiUrl + API.AGENCY_ROOT() + '0';
    mockAxios.onDelete(url).reply(400, MOCK.ERROR);
    return deleteAgency(agency)(dispatch).then(() => {
      expect(requestSpy).toHaveBeenCalledTimes(1);
      expect(errorSpy).toHaveBeenCalledTimes(1);
      expect(dispatch).toHaveBeenCalledTimes(4);
    });
  });
});
