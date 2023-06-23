import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { OrderParent, ParcelInfoOrder, TitleSummary } from '@/interfaces/ltsaModels';

import { useApiLtsa } from './useApiLtsa';

const mockAxios = new MockAdapter(axios);

describe('useApiLtsa testing suite', () => {
  afterEach(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiLtsa);
    return result.current;
  };

  it('Get title summaries', async () => {
    mockAxios.onGet(`/tools/ltsa/summaries?pid=123456789`).reply(200, defaultTitleSummary);

    const { getTitleSummaries } = setup();
    const response = await getTitleSummaries(123456789);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(defaultTitleSummary);
  });

  it('post parcel info order', async () => {
    mockAxios.onPost(`/tools/ltsa/order/parcelInfo?pid=123-456-789`).reply(200, defaultParcelInfo);
    const { getParcelInfo } = setup();
    const response = await getParcelInfo('123-456-789');

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(defaultParcelInfo);
  });
});

const defaultTitleSummary: TitleSummary[] = [{ titleNumber: '1234' }];
const defaultParcelInfo: ParcelInfoOrder = {
  orderId: '4321',
  productType: OrderParent.ProductTypeEnum.ParcelInfo,
};
