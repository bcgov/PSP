import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { mockApiAccessRequest } from '@/mocks/filterData.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_AccessRequest } from '@/models/api/generated/ApiGen_Concepts_AccessRequest';

import { useApiAccessRequests } from './useApiAccessRequests';

const mockAxios = new MockAdapter(axios);

describe('useApiAccessRequests api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiAccessRequests);
    return result.current;
  };

  it('Gets paged access requests', async () => {
    mockAxios
      .onGet('/admin/access/requests?page=1')
      .reply<ApiGen_Base_Page<ApiGen_Concepts_AccessRequest>>(200, {
        items: [mockApiAccessRequest],
        page: 1,
        quantity: 5,
        total: 10,
      });

    const { getAccessRequestsPaged } = setup();
    const response = await getAccessRequestsPaged({ page: 1 });

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual({
      items: [mockApiAccessRequest],
      page: 1,
      quantity: 5,
      total: 10,
    });
  });
  it('Gets current access request', async () => {
    mockAxios.onGet('/access/requests').reply(200, mockApiAccessRequest);

    const { getAccessRequest } = setup();
    const response = await getAccessRequest();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockApiAccessRequest);
  });

  it('Puts an updated access request', async () => {
    mockAxios.onPut('/keycloak/access/requests').reply(200, mockApiAccessRequest);
    const { putAccessRequest } = setup();
    const response = await putAccessRequest(mockApiAccessRequest);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockApiAccessRequest);
  });

  it('Posts a new access request', async () => {
    const newAccessRequest: ApiGen_Concepts_AccessRequest = { ...mockApiAccessRequest, id: 0 };
    mockAxios.onPost(`/access/requests`).reply(201, newAccessRequest);
    const { postAccessRequest } = setup();
    const response = await postAccessRequest(newAccessRequest);

    expect(response.status).toBe(201);
    expect(response.data).toEqual(newAccessRequest);
  });

  it('Puts an existing access request', async () => {
    mockAxios.onPut(`/access/requests/${mockApiAccessRequest.id}`).reply(200, mockApiAccessRequest);
    const { postAccessRequest } = setup();
    const response = await postAccessRequest(mockApiAccessRequest);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockApiAccessRequest);
  });

  it('Deletes a new access request', async () => {
    mockAxios
      .onDelete(`/admin/access/requests/${mockApiAccessRequest.id}`)
      .reply(200, mockApiAccessRequest);
    const { deleteAccessRequest } = setup();
    const response = await deleteAccessRequest(mockApiAccessRequest);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockApiAccessRequest);
  });
});
