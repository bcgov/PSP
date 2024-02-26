import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IMapProperty } from '@/components/propertySelector/models';
import {
  mockFAParcelLayerResponse,
  mockFAParcelLayerResponseMultiPolygon,
} from '@/mocks/faParcelLayerResponse.mock';
import { getMockLocationFeatureDataset } from '@/mocks/featureset.mock';

import {
  featuresetToMapProperty,
  featuresToIdentifiedMapProperty,
  getFilePropertyName,
  getLatLng,
  getPrettyLatLng,
  getPropertyName,
  NameSourceType,
  PropertyName,
} from './mapPropertyUtils';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { getEmptyFileProperty } from '@/mocks/fileProperty.mock';
import { getEmptyProperty } from '@/models/defaultInitializers';

const expectedMapProperty = {
  address: '',
  district: 2,
  districtName: 'Vancouver Island',
  latitude: 48.432802005,
  longitude: -123.310041775,
  name: undefined,
  pid: '000002500',
  pin: undefined,
  planNumber: 'VIP3881',
  polygon: {
    coordinates: [[[-123.31014591, 48.43274258]]],
    type: 'Polygon',
  },
  propertyId: undefined,
  region: 1,
  regionName: 'South Coast',
} as IMapProperty;

describe('mapPropertyUtils', () => {
  it.each([
    [{}, { label: NameSourceType.NONE, value: '' }],
    [{ pid: undefined }, { label: NameSourceType.NONE, value: '' }],
    [{ pid: '' }, { label: NameSourceType.NONE, value: '' }],
    [{ pid: '0' }, { label: NameSourceType.NONE, value: '' }],
    [{ pid: '000-000-001' }, { label: NameSourceType.PID, value: '000-000-001' }],
    [
      {
        pid: '000-000-001',
        pin: '1',
        planNumber: 'PB1000',
        latitude: 1,
        longitude: 1,
        address: '1234 fake st',
      },
      { label: NameSourceType.PID, value: '000-000-001' },
    ],
    [{ pin: undefined }, { label: NameSourceType.NONE, value: '' }],
    [{ pin: '' }, { label: NameSourceType.NONE, value: '' }],
    [{ pin: '0' }, { label: NameSourceType.NONE, value: '' }],
    [{ pin: '1' }, { label: NameSourceType.PIN, value: '1' }],
    [
      { pin: '1', planNumber: 'PB1000', latitude: 1, longitude: 1, address: '1234 fake st' },
      { label: NameSourceType.PIN, value: '1' },
    ],
    [{ planNumber: undefined }, { label: NameSourceType.NONE, value: '' }],
    [{ planNumber: '' }, { label: NameSourceType.NONE, value: '' }],
    [{ planNumber: '1' }, { label: NameSourceType.PLAN, value: '1' }],
    [
      { planNumber: 'PB1000', latitude: 1, longitude: 1, address: '1234 fake st' },
      { label: NameSourceType.PLAN, value: 'PB1000' },
    ],
    [
      { latitude: undefined, longitude: undefined },
      { label: NameSourceType.NONE, value: '' },
    ],
    [{ planNumber: '' }, { label: NameSourceType.NONE, value: '' }],
    [
      { latitude: 1, longitude: 2, address: '1234 fake st' },
      { label: NameSourceType.LOCATION, value: '2.000000, 1.000000' },
    ],
    [{ address: undefined }, { label: NameSourceType.NONE, value: '' }],
    [{ address: '' }, { label: NameSourceType.NONE, value: '' }],
    [{ address: '1234 fake st' }, { label: NameSourceType.ADDRESS, value: '1234 fake st' }],
  ])(
    'getPropertyName test with source %p expecting %p',
    (mapProperty: IMapProperty, expectedName: PropertyName) => {
      const actualName = getPropertyName(mapProperty);
      expect(actualName.label).toEqual(expectedName.label);
      expect(actualName.value).toEqual(expectedName.value);
    },
  );

  it('getPrettyLatLng, empty', () => {
    const prettyLatLng = getPrettyLatLng(undefined);
    expect(prettyLatLng).toEqual('');
  });

  it('getPrettyLatLng, valued', () => {
    const prettyLatLng = getPrettyLatLng({ coordinate: { x: 1, y: 2 } });
    expect(prettyLatLng).toEqual('1.000000, 2.000000');
  });

  it('getLatLng, empty', () => {
    const latLng = getLatLng(undefined);
    expect(latLng).toEqual(null);
  });

  it('getLatLng, valued', () => {
    const latLng = getLatLng({ coordinate: { x: 1, y: 2 } });
    expect(latLng).toEqual({ lat: 2, lng: 1 });
  });

  it.each([
    [false, { label: NameSourceType.NONE, value: '' }, { ...getEmptyFileProperty() }],
    [false, { label: NameSourceType.NONE, value: '' }, undefined],
    [
      false,
      { label: NameSourceType.NONE, value: '' },
      { ...getEmptyFileProperty(), propertyName: '' },
    ],
    [
      false,
      { label: NameSourceType.NONE, value: '' },
      { ...getEmptyFileProperty(), propertyName: null },
    ],
    [
      false,
      { label: NameSourceType.NAME, value: 'name' },
      { ...getEmptyFileProperty(), propertyName: 'name' },
    ],
    [
      true,
      { label: NameSourceType.NONE, value: '' },
      { ...getEmptyFileProperty(), propertyName: 'name' },
    ],
    [
      false,
      { label: NameSourceType.NONE, value: '' },
      { ...getEmptyFileProperty(), property: null },
    ],
    [
      false,
      { label: NameSourceType.PID, value: '000-000-001' },
      { ...getEmptyFileProperty(), property: { ...getEmptyProperty(), pid: 1 } },
    ],
  ])(
    'getFilePropertyName test with ignore name flag %p expecting %p source %p',
    (skipName: boolean, expectedName: PropertyName, mapProperty?: ApiGen_Concepts_FileProperty) => {
      const fileName = getFilePropertyName(mapProperty, skipName);
      expect(fileName.label).toEqual(expectedName.label);
      expect(fileName.value).toEqual(expectedName.value);
    },
  );

  it.each([
    [{ type: 'FeatureCollection', features: [] } as FeatureCollection, undefined, []],
    [
      mockFAParcelLayerResponse,
      undefined,
      [
        {
          address: undefined,
          district: undefined,
          districtName: undefined,
          latitude: 48.76613749999999,
          longitude: -123.46163749999998,
          name: undefined,
          pid: '9727493',
          pin: undefined,
          planNumber: 'NO_PLAN',
          propertyId: undefined,
          region: undefined,
          regionName: undefined,
        },
      ],
    ],
    [
      mockFAParcelLayerResponseMultiPolygon,
      undefined,
      [
        {
          address: undefined,
          district: undefined,
          districtName: undefined,
          latitude: 48.76613749999999,
          longitude: -123.46163749999998,
          name: undefined,
          pid: '9727493',
          pin: undefined,
          planNumber: 'NO_PLAN',
          propertyId: undefined,
          region: undefined,
          regionName: undefined,
        },
      ],
    ],
  ])(
    'featuresToIdentifiedMapProperty test with feature values %p and address %p and expected map properties %p',
    (
      values: FeatureCollection<Geometry, GeoJsonProperties> | undefined,
      address?: string,
      expected?: IMapProperty[],
    ) => {
      const mapProperties = featuresToIdentifiedMapProperty(values, address);
      expect(mapProperties).toEqual(expected);
    },
  );

  it.each([
    [getMockLocationFeatureDataset(), '', expectedMapProperty],
    [{ ...getMockLocationFeatureDataset(), pimsFeature: {} as any }, '', expectedMapProperty],
    [
      { ...getMockLocationFeatureDataset(), parcelFeature: {} as any },
      '',
      { ...expectedMapProperty, pid: undefined, planNumber: undefined, polygon: undefined },
    ],
    [
      { ...getMockLocationFeatureDataset(), pimsFeature: {} as any },
      'address',
      { ...expectedMapProperty, address: 'address' },
    ],
  ])(
    'featuresetToMapProperty test with feature set %p address %p expectedPropertyFile %p',
    (
      featureSet: LocationFeatureDataset,
      address: string = 'unknown',
      expectedPropertyFile?: IMapProperty,
    ) => {
      const mapProperty = featuresetToMapProperty(featureSet, address);
      expect(mapProperty).toEqual(expectedPropertyFile);
    },
  );
});
