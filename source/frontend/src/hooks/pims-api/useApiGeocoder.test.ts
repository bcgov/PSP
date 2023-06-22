import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { mockGeocoderOptions, mockGeocoderPidsResponse } from '@/mocks/index.mock';

import { useApiGeocoder } from './useApiGeocoder';

const mockAxios = new MockAdapter(axios);

describe('useApiGeocoder testing suite', () => {
  beforeEach(() => {
    mockAxios.onGet(`/tools/geocoder/parcels/pids/1234`).reply(200, mockGeocoderPidsResponse);
    mockAxios
      .onGet(
        `/tools/geocoder/addresses?address=1234 Fake&matchPrecisionNot=OCCUPANT,INTERSECTION,BLOCK,STREET,LOCALITY,PROVINCE,OCCUPANT`,
      )
      .reply(200, mockGeocoderOptions);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiGeocoder);
    return result.current;
  };

  it('Get SitePIds', async () => {
    const { getSitePidsApi: getSitePids } = setup();
    const response = await getSitePids('1234');

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockGeocoderPidsResponse);
  });

  it('Search address', async () => {
    const { searchAddressApi: searchAddress } = setup();
    const response = await searchAddress(
      '1234 Fake',
      'matchPrecisionNot=OCCUPANT,INTERSECTION,BLOCK,STREET,LOCALITY,PROVINCE,OCCUPANT',
    );

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockGeocoderOptions);
  });
});
