import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IPagedItems } from 'interfaces';
import { AGENCIES, mockAccessRequest } from 'mocks/filterDataMock';

import { useApiAgencies } from './useApiAgencies';

const mockAxios = new MockAdapter(axios);
const mockAgency = AGENCIES[0];

describe('useApiAgencies api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Gets paged agencies', () => {
    renderHook(async () => {
      mockAxios.onPost(`/admin/agencies/filter`).reply(200, {
        items: [mockAccessRequest],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      } as IPagedItems);

      const api = useApiAgencies();
      const response = await api.getAgenciesPaged({ page: 1 });

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
  it('Get detailed agency', () => {
    renderHook(async () => {
      mockAxios.onGet(`/admin/agencies/${mockAgency.id}`).reply(200, mockAgency);

      const api = useApiAgencies();
      const response = await api.getAgency(mockAgency.id);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockAgency);
    });
  });

  it('Puts an updated agency', () => {
    const newAgency = { ...mockAgency, sendEmail: true, addressTo: '' };
    renderHook(async () => {
      mockAxios.onPut(`/admin/agencies/${mockAgency.id}`).reply(200, newAgency);
      const api = useApiAgencies();
      const response = await api.putAgency(newAgency);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(newAgency);
    });
  });

  it('Posts a new agency', () => {
    const newAgency = { ...mockAgency, id: 0, sendEmail: true, addressTo: '' };
    renderHook(async () => {
      mockAxios.onPost(`/admin/agencies`).reply(201, newAgency);
      const api = useApiAgencies();
      const response = await api.postAgency(newAgency);

      expect(response.status).toBe(201);
      expect(response.data).toStrictEqual(newAgency);
    });
  });

  it('Deletes a new agency', () => {
    mockAxios.onDelete(`/admin/agencies/${mockAgency.id}`).reply(200, mockAgency);
    renderHook(async () => {
      const api = useApiAgencies();
      const response = await api.deleteAgency(mockAgency);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockAgency);
    });
  });
});
