import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IPagedItems } from 'interfaces';
import { mockAccessRequest, mockOrganization } from 'mocks/filterDataMock';

import { useApiOrganizations } from './useApiOrganizations';

const mockAxios = new MockAdapter(axios);

describe('useApiOrganizations api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Gets paged organizations', () => {
    renderHook(async () => {
      mockAxios.onPost(`/admin/organizations/filter`).reply(200, {
        items: [mockAccessRequest],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      } as IPagedItems);

      const api = useApiOrganizations();
      const response = await api.getOrganizationsPaged({ page: 1 });

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
  it('Get detailed organization', () => {
    renderHook(async () => {
      mockAxios.onGet(`/admin/organizations/${mockOrganization.id}`).reply(200, mockOrganization);

      const api = useApiOrganizations();
      const response = await api.getOrganization(mockOrganization.id ?? 0);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockOrganization);
    });
  });

  it('Puts an updated organization', () => {
    const newOrganization = { ...mockOrganization, sendEmail: true, addressTo: '' };
    renderHook(async () => {
      mockAxios.onPut(`/admin/organizations/${mockOrganization.id}`).reply(200, newOrganization);
      const api = useApiOrganizations();
      const response = await api.putOrganization(newOrganization);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(newOrganization);
    });
  });

  it('Posts a new organization', () => {
    const newOrganization = { ...mockOrganization, id: 0, sendEmail: true, addressTo: '' };
    renderHook(async () => {
      mockAxios.onPost(`/admin/organizations`).reply(201, newOrganization);
      const api = useApiOrganizations();
      const response = await api.postOrganization(newOrganization);

      expect(response.status).toBe(201);
      expect(response.data).toStrictEqual(newOrganization);
    });
  });

  it('Deletes a new organization', () => {
    mockAxios.onDelete(`/admin/organizations/${mockOrganization.id}`).reply(200, mockOrganization);
    renderHook(async () => {
      const api = useApiOrganizations();
      const response = await api.deleteOrganization(mockOrganization);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockOrganization);
    });
  });
});
