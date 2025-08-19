import { polygon } from '@turf/turf';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IMapProperty } from '@/components/propertySelector/models';
import { AreaUnitTypes } from '@/constants';
import {
  mockFAParcelLayerResponse,
  mockFAParcelLayerResponseMultiPolygon,
} from '@/mocks/faParcelLayerResponse.mock';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { getEmptyFileProperty } from '@/mocks/fileProperty.mock';
import { getMockLatLng, getMockLocation, getMockPolygon } from '@/mocks/geometries.mock';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Geometry } from '@/models/api/generated/ApiGen_Concepts_Geometry';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

import { LocationBoundaryDataset } from '@/components/common/mapFSM/models';
import {
  featuresetToMapProperty,
  featuresToIdentifiedMapProperty,
  filePropertyToLocationBoundaryDataset,
  getFilePropertyName,
  getLatLng,
  getPrettyLatLng,
  getPropertyName,
  isLatLngInFeatureSetBoundary,
  latLngFromMapProperty,
  latLngToApiLocation,
  locationFromFileProperty,
  NameSourceType,
  pidFromFeatureSet,
  pinFromFeatureSet,
  PropertyName,
} from './mapPropertyUtils';

const expectedMapProperty = {
  address: '',
  areaUnit: 'M2',
  district: 2,
  districtName: 'Vancouver Island',
  landArea: 647.4646,
  latitude: 48.432802005,
  longitude: -123.310041775,
  legalDescription: undefined,
  name: undefined,
  pid: '000002500',
  pin: undefined,
  planNumber: 'VIP3881',
  polygon: {
    coordinates: [[[-123.31014591, 48.43274258]]],
    type: 'Polygon',
  },
  fileLocation: {
    lat: 48.432802005,
    lng: -123.310041775,
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
    'getPropertyName test with source %o expecting %o',
    (mapProperty: IMapProperty, expectedName: PropertyName) => {
      const actualName = getPropertyName(mapProperty);
      expect(actualName.label).toEqual(expectedName.label);
      expect(actualName.value).toEqual(expectedName.value);
    },
  );

  it.each([
    ['empty', undefined, ''],
    ['valued', { coordinate: { x: 1, y: 2 } }, '1.000000, 2.000000'],
  ])('getPrettyLatLng - %s', (_, value, expected) => {
    const prettyLatLng = getPrettyLatLng(value);
    expect(prettyLatLng).toEqual(expected);
  });

  it.each([
    ['empty', undefined, null],
    ['valued', { coordinate: { x: 1, y: 2 } }, { lat: 2, lng: 1 }],
  ])('getLatLng - %s', (_, value, expected) => {
    const latLng = getLatLng(value);
    expect(latLng).toEqual(expected);
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
    'getFilePropertyName test with ignore name flag %o expecting %s source %o',
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
          areaUnit: AreaUnitTypes.SquareMeters,
          district: undefined,
          districtName: undefined,
          landArea: 29217,
          latitude: 48.76613749999999,
          legalDescription:
            'THAT PART OF SECTION 13, RANGE 1, SOUTH SALT SPRING ISLAND, COWICHAN DISTRICT',
          longitude: -123.46163749999998,
          name: undefined,
          pid: '9727493',
          pin: undefined,
          planNumber: 'NO_PLAN',
          propertyId: undefined,
          region: undefined,
          regionName: undefined,
          fileLocation: {
            lat: 48.76613749999999,
            lng: -123.46163749999998,
          },
        },
      ],
    ],
    [
      mockFAParcelLayerResponseMultiPolygon,
      undefined,
      [
        {
          address: undefined,
          areaUnit: AreaUnitTypes.SquareMeters,
          district: undefined,
          districtName: undefined,
          landArea: 29217,
          latitude: 48.76613749999999,
          legalDescription:
            'THAT PART OF SECTION 13, RANGE 1, SOUTH SALT SPRING ISLAND, COWICHAN DISTRICT',
          longitude: -123.46163749999998,
          name: undefined,
          pid: '9727493',
          pin: undefined,
          planNumber: 'NO_PLAN',
          propertyId: undefined,
          region: undefined,
          regionName: undefined,
          fileLocation: {
            lat: 48.76613749999999,
            lng: -123.46163749999998,
          },
        },
      ],
    ],
  ])(
    'featuresToIdentifiedMapProperty test with feature values %o and address %o and expected map properties %o',
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
    [getMockSelectedFeatureDataset(), '', expectedMapProperty],
    [{ ...getMockSelectedFeatureDataset(), pimsFeatures: {} as any }, '', expectedMapProperty],
    [
      { ...getMockSelectedFeatureDataset(), parcelFeature: {} as any },
      '',
      {
        ...expectedMapProperty,
        pid: undefined,
        planNumber: undefined,
        polygon: undefined,
        landArea: 0,
      },
    ],
    [
      { ...getMockSelectedFeatureDataset(), pimsFeatures: {} as any },
      'address',
      { ...expectedMapProperty, address: 'address' },
    ],
  ])(
    'featuresetToMapProperty test with feature set %o address %o expectedPropertyFile %o',
    (
      featureSet: SelectedFeatureDataset,
      address: string = 'unknown',
      expectedPropertyFile?: IMapProperty,
    ) => {
      const mapProperty = featuresetToMapProperty(featureSet, address);
      expect(mapProperty).toEqual(expectedPropertyFile);
    },
  );

  it.each([
    [
      { ...getMockSelectedFeatureDataset(), pimsFeature: { properties: { PID_PADDED: '123-456-789' } } as any },
      '123-456-789',
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: { properties: { PID: '9999' } } as any,
      },
      '9999',
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: {} as any,
      },
      null,
    ],
  ])(
    'pidFromFeatureSet test with feature set %o - expected %s',
    (featureSet: SelectedFeatureDataset, expectedValue: string | null) => {
      const pid = pidFromFeatureSet(featureSet);
      expect(pid).toEqual(expectedValue);
    },
  );

  it.each([
    [
      { ...getMockSelectedFeatureDataset(), pimsFeature: { properties: { PIN: 1234 } } as any },
      '1234',
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: { properties: { PIN: 9999 } } as any,
      },
      '9999',
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: {} as any,
      },
      null,
    ],
    [
      {
        pimsFeature: { properties: { pin: '4321' } } as any,
        parcelFeature: { properties: { pid: 1234 } } as any,
      },
      null,
    ],
  ])(
    'pinFromFeatureSet test with feature set %o - expected %s',
    (featureSet: SelectedFeatureDataset, expectedValue: string | null) => {
      const pid = pinFromFeatureSet(featureSet);
      expect(pid).toEqual(expectedValue);
    },
  );

  it.each([
    [{ ...getEmptyFileProperty(), location: { ...getMockLocation() } }, { ...getMockLocation() }],
    [
      {
        ...getEmptyFileProperty(),
        location: null,
        property: { ...getEmptyProperty(), location: { ...getMockLocation() } },
      },
      { ...getMockLocation() },
    ],
    [{ ...getEmptyFileProperty(), location: null }, null],
  ])(
    'locationFromFileProperty test with file property %o - expected %o',
    (
      fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
      expectedValue: ApiGen_Concepts_Geometry | null,
    ) => {
      const location = locationFromFileProperty(fileProperty);
      expect(location).toEqual(expectedValue);
    },
  );

  it.each([
    [
      { ...getEmptyFileProperty(), location: getMockLocation() },
      { location: getMockLatLng(), boundary: null, isActive: null },
    ],
    [
      {
        ...getEmptyFileProperty(),
        location: null,
        property: {
          ...getEmptyProperty(),
          location: getMockLocation(),
          boundary: getMockPolygon(),
        },
      },
      { location: getMockLatLng(), boundary: getMockPolygon(), isActive: null },
    ],
    [{ ...getEmptyFileProperty(), location: null }, null],
  ])(
    'filePropertyToLocationBoundaryDataset test with file property %o - expected %o',
    (
      fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
      expectedValue: LocationBoundaryDataset | null,
    ) => {
      const dataset = filePropertyToLocationBoundaryDataset(fileProperty);
      expect(dataset).toEqual(expectedValue);
    },
  );

  it.each([
    [4, 5, { ...getMockLocation(4, 5) }],
    [null, null, null],
  ])(
    'latLngToApiLocation test with latitude %s, longitude %s - expected %o',
    (
      latitude: number | null,
      longitude: number | null,
      expectedValue: ApiGen_Concepts_Geometry | null,
    ) => {
      const apiGeometry = latLngToApiLocation(latitude, longitude);
      expect(apiGeometry).toEqual(expectedValue);
    },
  );

  it.each([
    [{ fileLocation: { lat: 4, lng: 5 } }, { lat: 4, lng: 5 }],
    [
      { latitude: 4, longitude: 5 },
      { lat: 4, lng: 5 },
    ],
    [undefined, { lat: 0, lng: 0 }],
  ])(
    'latLngFromMapProperty test with file property %o - expected %o',
    (mapProperty: IMapProperty | undefined | null, expectedValue: LatLngLiteral | null) => {
      const latLng = latLngFromMapProperty(mapProperty);
      expect(latLng).toEqual(expectedValue);
    },
  );

  it.each([
    [
      { lat: 44, lng: -77 },
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: polygon([
          [
            [-81, 41],
            [-81, 47],
            [-72, 47],
            [-72, 41],
            [-81, 41],
          ],
        ]) as Feature<Geometry, PIMS_Property_Location_View> | null,
      },
      true,
    ],
    [
      { lat: 44, lng: 80 },
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: polygon([
          [
            [-81, 41],
            [-81, 47],
            [-72, 47],
            [-72, 41],
            [-81, 41],
          ],
        ]) as Feature<Geometry, PIMS_Property_Location_View> | null,
      },
      false,
    ],
    [
      { lat: 44, lng: -77 },
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: null,
        parcelFeature: polygon([
          [
            [-81, 41],
            [-81, 47],
            [-72, 47],
            [-72, 41],
            [-81, 41],
          ],
        ]) as Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
      },
      true,
    ],
    [
      { lat: 44, lng: 80 },
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: null,
        parcelFeature: polygon([
          [
            [-81, 41],
            [-81, 47],
            [-72, 47],
            [-72, 41],
            [-81, 41],
          ],
        ]) as Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
      },
      false,
    ],
    [
      { lat: 44, lng: -77 },
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        fileLocation: null,
        pimsFeature: null,
        parcelFeature: null,
      },
      false,
    ],
  ])(
    'isLatLngInFeatureSetBoundary test with lat/long %o, feature set %o - expected %o',
    (latLng: LatLngLiteral, featureset: SelectedFeatureDataset, expectedValue: boolean) => {
      const result = isLatLngInFeatureSetBoundary(latLng, featureset);
      expect(result).toEqual(expectedValue);
    },
  );
});
