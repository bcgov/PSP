import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IPagedItems } from 'interfaces';
import { mockAccessRequest } from 'mocks/filterDataMock';

import { useApiAccessRequests } from '.';

const mockAxios = new MockAdapter(axios);

describe('useApiAccessRequests api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Gets paged access requests', () => {
    renderHook(async () => {
      mockAxios.onGet('/admin/access/requests?page=1').reply(200, {
        items: [mockAccessRequest],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      } as IPagedItems);

      const api = useApiAccessRequests();
      const response = await api.getAccessRequestsPaged({ page: 1 });

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual({
        items: [mockAccessRequest],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      });
    });
  });
  it('Gets current access request', () => {
    renderHook(async () => {
      mockAxios.onGet('/access/requests').reply(200, mockAccessRequest);

      const api = useApiAccessRequests();
      const response = await api.getAccessRequest();

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockAccessRequest);
    });
  });

  it('Puts an updated access request', () => {
    renderHook(async () => {
      mockAxios.onPut('/keycloak/access/requests').reply(200, mockAccessRequest);
      const api = useApiAccessRequests();
      const response = await api.putAccessRequest(mockAccessRequest);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockAccessRequest);
    });
  });

  it('Posts a new access request', () => {
    const newAccessRequest = { ...mockAccessRequest, id: undefined };
    renderHook(async () => {
      mockAxios.onPost(`/access/requests`).reply(201, newAccessRequest);
      const api = useApiAccessRequests();
      const response = await api.postAccessRequest(newAccessRequest);

      expect(response.status).toBe(201);
      expect(response.data).toEqual(newAccessRequest);
    });
  });

  it('Puts an existing access request', () => {
    renderHook(async () => {
      mockAxios.onPut(`/access/requests/${mockAccessRequest.id}`).reply(200, mockAccessRequest);
      const api = useApiAccessRequests();
      const response = await api.postAccessRequest(mockAccessRequest);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockAccessRequest);
    });
  });

  it('Deletes a new access request', () => {
    mockAxios
      .onDelete(`/admin/access/requests/${mockAccessRequest.id}`)
      .reply(200, mockAccessRequest);
    renderHook(async () => {
      const api = useApiAccessRequests();
      const response = await api.deleteAccessRequest(mockAccessRequest);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockAccessRequest);
    });
  });
});
