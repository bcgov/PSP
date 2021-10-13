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

    const { getPropertiesPaged } = setup();
    const response = await getPropertiesPaged({} as any);

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

    const { getPropertiesPaged } = setup();
    const response = await getPropertiesPaged({} as any);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockParcel);
  });
  it('Gets a detailed parcel', async () => {
    mockAxios.onGet(`/properties/${mockParcel.id}`).reply(200, mockParcel);

    const { getProperty } = setup();
    const response = await getProperty(mockParcel.id as number);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockParcel);
  });

  it('Puts an updated parcel', async () => {
    mockAxios.onPut(`/properties/${mockParcel.id}`).reply(200, mockParcel);

    const { putProperty } = setup();
    const response = await putProperty(mockParcel);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockParcel);
  });

  it('Posts a new parcel', async () => {
    const newParcel = { ...mockParcel, id: undefined };
    mockAxios.onPost(`/properties`).reply(201, newParcel);

    const { postProperty } = setup();
    const response = await postProperty(mockParcel);

    expect(response.status).toBe(201);
    expect(response.data).toEqual(newParcel);
  });

  it('Deletes a parcel', async () => {
    mockAxios.onDelete(`/properties/${mockParcel.id}`).reply(200, mockParcel);

    const { deleteProperty } = setup();
    const response = await deleteProperty(mockParcel);

    expect(response.status).toBe(200);
    expect(response.data).toEqual(mockParcel);
  });
});
