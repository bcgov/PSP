import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { mockApiProperty } from '@/mocks/filterData.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

import { useApiProperties } from './useApiProperties';

const mockAxios = new MockAdapter(axios);
describe('useApiProperties api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiProperties);
    return result.current;
  };

  it('Gets paged parcels', async () => {
    mockAxios.onGet(`/properties/search?`).reply<ApiGen_Base_Page<ApiGen_Concepts_Property>>(200, {
      items: [mockApiProperty],
      page: 1,
      quantity: 5,
      total: 10,
    });

    const { getPropertiesViewPagedApi } = setup();
    const response = await getPropertiesViewPagedApi({} as any);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual({
      items: [mockApiProperty],
      page: 1,
      quantity: 5,
      total: 10,
    });
  });

  it('Gets detailed parcels', async () => {
    mockAxios.onGet(`/properties/search?`).reply(200, mockApiProperty);

    const { getPropertiesViewPagedApi } = setup();
    const response = await getPropertiesViewPagedApi({} as any);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockApiProperty);
  });
  it('Gets a detailed parcel', async () => {
    mockAxios.onGet(`/properties/${mockApiProperty.id}`).reply(200, mockApiProperty);

    const { getPropertyConceptWithIdApi } = setup();
    const response = await getPropertyConceptWithIdApi(mockApiProperty.id);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockApiProperty);
  });
});
