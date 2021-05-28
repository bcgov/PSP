import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockParcel } from 'components/maps/leaflet/InfoSlideOut/InfoContent.test';
import { IPagedItems } from 'interfaces';
import { mockBuilding } from 'mocks/filterDataMock';

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
      const response = await api.getParcels({} as any);

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
      mockAxios.onGet(`/properties/parcels?`).reply(200, mockParcel);

      const api = useApiProperties();
      const response = await api.getParcelsDetail({} as any);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockParcel);
    });
  });
  it('Gets a detailed parcel', () => {
    renderHook(async () => {
      mockAxios.onGet(`/properties/parcels/${mockParcel.id}`).reply(200, mockParcel);

      const api = useApiProperties();
      const response = await api.getParcelDetail(mockParcel.id as number);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockParcel);
    });
  });

  it('Puts an updated parcel', () => {
    renderHook(async () => {
      mockAxios.onPut(`/properties/parcels/${mockParcel.id}`).reply(200, mockParcel);
      const api = useApiProperties();
      const response = await api.putParcel(mockParcel);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockParcel);
    });
  });

  it('Posts a new parcel', () => {
    const newParcel = { ...mockParcel, id: 0 };
    renderHook(async () => {
      mockAxios.onPost(`/properties/parcels`).reply(201, newParcel);
      const api = useApiProperties();
      const response = await api.postParcel(newParcel);

      expect(response.status).toBe(201);
      expect(response.data).toStrictEqual(newParcel);
    });
  });

  it('Deletes a parcel', () => {
    mockAxios.onDelete(`/properties/parcels/${mockParcel.id}`).reply(200, mockParcel);
    renderHook(async () => {
      const api = useApiProperties();
      const response = await api.deleteParcel(mockParcel);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockParcel);
    });
  });

  it('Gets a detailed building', () => {
    renderHook(async () => {
      mockAxios.onGet(`/properties/buildings/${mockBuilding.id}`).reply(200, mockBuilding);

      const api = useApiProperties();
      const response = await api.getBuilding(mockBuilding.id as number);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockBuilding);
    });
  });

  it('Deletes a building', () => {
    mockAxios.onDelete(`/properties/buildings/${mockBuilding.id}`).reply(200, mockBuilding);
    renderHook(async () => {
      const api = useApiProperties();
      const response = await api.deleteBuilding(mockBuilding);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockBuilding);
    });
  });
});
