import { LatLngLiteral } from 'leaflet';

import { cleanup } from '@/utils/test-utils';

import { DmsCoordinates, DmsDirection } from './models';

describe('coordinate search model tests', () => {
  afterEach(cleanup);

  it.each([
    [
      'South/East lat/long',
      [77, 30, 29.9988, DmsDirection.S] as const,
      [164, 45, 15.0012, DmsDirection.E] as const,
      { lat: -77.508333, lng: 164.754167 } as LatLngLiteral,
    ],
    [
      'North/West lat/long',
      [53, 46, 46.152, DmsDirection.N] as const,
      [124, 55, 51.008, DmsDirection.W] as const,
      { lat: 53.779487, lng: -124.930836 } as LatLngLiteral,
    ],
  ])(
    'converts degree, minutes and seconds to lat/long - %s',
    (
      _: string,
      latDms: readonly [number, number, number, '1' | '-1'],
      lngDms: readonly [number, number, number, '1' | '-1'],
      expectedValue: LatLngLiteral,
    ) => {
      const coords = new DmsCoordinates();
      coords.latitude.degrees = latDms[0];
      coords.latitude.minutes = latDms[1];
      coords.latitude.seconds = latDms[2];
      coords.latitude.direction = latDms[3];

      coords.longitude.degrees = lngDms[0];
      coords.longitude.minutes = lngDms[1];
      coords.longitude.seconds = lngDms[2];
      coords.longitude.direction = lngDms[3];

      const latLng = coords.toLatLng();
      expect(latLng).toStrictEqual(expectedValue);
    },
  );

  it('defaults to zero for invalid values', () => {
    const coords = new DmsCoordinates();
    coords.latitude.degrees = 53;
    coords.latitude.minutes = NaN;
    coords.latitude.seconds = 'not a number' as any;
    coords.latitude.direction = DmsDirection.N;

    expect(coords.latitude.toDecimalDegrees()).toBe(0);
  });
});
