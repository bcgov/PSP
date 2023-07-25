import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { IPagedItems } from '@/interfaces';
import { mockParcel } from '@/mocks/filterData.mock';

import { useApiProperties } from './useApiProperties';

const mockAxios = new MockAdapter(axios);
describe('useApiProperties api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiProperties);
    return result.current;
  };

  it('Gets paged parcels', async () => {
    mockAxios.onGet(`/properties/search?`).reply(200, {
      items: [mockParcel],
      pageIndex: 1,
      page: 1,
      quantity: 5,
      total: 10,
    } as IPagedItems);

    const { getPropertiesPagedApi } = setup();
    const response = await getPropertiesPagedApi({} as any);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual({
      items: [mockParcel],
      pageIndex: 1,
      page: 1,
      quantity: 5,
      total: 10,
    });
  });

  it('Gets detailed parcels', async () => {
    mockAxios.onGet(`/properties/search?`).reply(200, mockParcel);

    const { getPropertiesPagedApi } = setup();
    const response = await getPropertiesPagedApi({} as any);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockParcel);
  });
  it('Gets a detailed parcel', async () => {
    mockAxios.onGet(`/properties/${mockParcel.id}`).reply(200, mockParcel);

    const { getPropertyConceptWithIdApi } = setup();
    const response = await getPropertyConceptWithIdApi(mockParcel.id as number);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockParcel);
  });
});
