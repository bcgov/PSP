import { IGeocoderPidsResponse, IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';

export const mockGeocoderPidsResponse: IGeocoderPidsResponse = {
  siteId: '1',
  pids: ['312312'],
};

export const mockGeocoderOptions: IGeocoderResponse[] = [
  {
    fullAddress: '1234 Fake St',
    siteId: '1',
    address1: '1234 Fake St',
    administrativeArea: 'Test Town',
    provinceCode: 'BC',
    latitude: 1,
    longitude: 1,
    score: 60,
  },
  {
    fullAddress: '5521 Test St',
    siteId: '2',
    address1: '5521 Test St',
    administrativeArea: 'Test Town',
    provinceCode: 'BC',
    latitude: 2,
    longitude: 2,
    score: 70,
  },
];
