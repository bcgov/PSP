import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IPagedItems } from 'interfaces';
import { mockAccessRequest, mockApiAccessRequest } from 'mocks/filterDataMock';

import { useApiAccessRequests } from '.';

const mockAxios = new MockAdapter(axios);

describe('useApiAccessRequests api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiAccessRequests);
    return result.current;
  };

  it('Gets paged access requests', async () => {
    mockAxios.onGet('/admin/access/requests?page=1').reply(200, {
      items: [mockAccessRequest],
      pageIndex: 1,
      page: 1,
      quantity: 5,
      total: 10,
    } as IPagedItems);

    const { getAccessRequestsPaged } = setup();
    const response = await getAccessRequestsPaged({ page: 1 });

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual({
      items: [mockAccessRequest],
      pageIndex: 1,
      page: 1,
      quantity: 5,
      total: 10,
    });
  });
  it('Gets current access request', async () => {
    mockAxios.onGet('/access/requests').reply(200, mockAccessRequest);

    const { getAccessRequest } = setup();
    const response = await getAccessRequest();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockAccessRequest);
  });

  it('Puts an updated access request', async () => {
    mockAxios.onPut('/keycloak/access/requests').reply(200, mockAccessRequest);
    const { putAccessRequest } = setup();
    const response = await putAccessRequest(mockAccessRequest);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockAccessRequest);
  });

  it('Posts a new access request', async () => {
    const newAccessRequest = { ...mockApiAccessRequest, id: undefined };
    mockAxios.onPost(`/access/requests`).reply(201, newAccessRequest);
    const { postAccessRequest } = setup();
    const response = await postAccessRequest(newAccessRequest);

    expect(response.status).toBe(201);
    expect(response.data).toEqual(newAccessRequest);
  });

  it('Puts an existing access request', async () => {
    mockAxios.onPut(`/access/requests/${mockApiAccessRequest.id}`).reply(200, mockAccessRequest);
    const { postAccessRequest } = setup();
    const response = await postAccessRequest(mockApiAccessRequest);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockAccessRequest);
  });

  it('Deletes a new access request', async () => {
    mockAxios
      .onDelete(`/admin/access/requests/${mockAccessRequest.id}`)
      .reply(200, mockAccessRequest);
    const { deleteAccessRequest } = setup();
    const response = await deleteAccessRequest(mockAccessRequest);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockAccessRequest);
  });
});
