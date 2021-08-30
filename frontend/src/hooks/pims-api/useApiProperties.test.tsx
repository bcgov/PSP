import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IPagedItems } from 'interfaces';
import { mockParcel } from 'mocks/filterDataMock';

import { useApiProperties } from './useApiProperties';

const mockAxios = new MockAdapter(axios);
describe('useApiProperties api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Gets paged parcels', () => {
    renderHook(async () => {
      mockAxios.onGet(`/properties/search?`).reply(200, {
        items: [mockParcel],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      } as IPagedItems);

      const api = useApiProperties();
      const response = await api.getProperties({} as any);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual({
        items: [mockParcel],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      });
    });
  });

  it('Gets detailed parcels', () => {
    renderHook(async () => {
      mockAxios.onGet(`/properties/search?`).reply(200, mockParcel);

      const api = useApiProperties();
      const response = await api.getProperties({} as any);

      expect(response.status).toBe(200);
      expect(response.data).toEqual(mockParcel);
    });
  });
  it('Gets a detailed parcel', () => {
    renderHook(async () => {
      mockAxios.onGet(`/properties/${mockParcel.id}`).reply(200, mockParcel);

      const api = useApiProperties();
      const response = await api.getProperty(mockParcel.id as number);

      expect(response.status).toBe(200);
      expect(response.data).toEqual(mockParcel);
    });
  });

  it('Puts an updated parcel', () => {
    renderHook(async () => {
      mockAxios.onPut(`/properties/${mockParcel.id}`).reply(200, mockParcel);
      const api = useApiProperties();
      const response = await api.putProperty(mockParcel);

      expect(response.status).toBe(200);
      expect(response.data).toEqual(mockParcel);
    });
  });

  it('Posts a new parcel', () => {
    const newParcel = { ...mockParcel, id: undefined };
    renderHook(async () => {
      mockAxios.onPost(`/properties`).reply(201, newParcel);
      const api = useApiProperties();
      const response = await api.postProperty(newParcel);

      expect(response.status).toBe(201);
      expect(response.data).toEqual(newParcel);
    });
  });

  it('Deletes a parcel', () => {
    mockAxios.onDelete(`/properties/${mockParcel.id}`).reply(200, mockParcel);
    renderHook(async () => {
      const api = useApiProperties();
      const response = await api.deleteProperty(mockParcel);

      expect(response.status).toBe(200);
      expect(response.data).toEqual(mockParcel);
    });
  });
});
