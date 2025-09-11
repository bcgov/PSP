import { polygon } from '@turf/turf';
import { Feature, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';

import { LocationBoundaryDataset } from '@/components/common/mapFSM/models';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { getEmptyFileProperty } from '@/mocks/fileProperty.mock';
import { getMockLatLng, getMockLocation, getMockPolygon } from '@/mocks/geometries.mock';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Geometry } from '@/models/api/generated/ApiGen_Concepts_Geometry';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

import {
  filePropertyToLocationBoundaryDataset,
  getFilePropertyName,
  getLatLng,
  getPrettyLatLng,
  getPropertyNameFromSelectedFeatureSet,
  isLatLngInFeatureSetBoundary,
  latLngToApiLocation,
  locationFromFileProperty,
  NameSourceType,
  pidFromFeatureSet,
  pinFromFeatureSet,
  PropertyName,
} from './mapPropertyUtils';

describe('mapPropertyUtils', () => {
  it.each([
    [{}, { label: NameSourceType.NONE, value: '' }],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: {} as any,
        parcelFeature: { properties: { PID: undefined } } as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: {} as any,
        parcelFeature: { properties: { PID: '' } } as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: { properties: { PID: '000-000-001' } } as any,
      },
      { label: NameSourceType.PID, value: '000-000-001' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: {
          properties: {
            PID: '000-000-001',
            PIN: 1,
            PLAN_NUMBER: 'PB1000',
          },
        } as any,
      },
      { label: NameSourceType.PID, value: '000-000-001' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: {} as any,
        parcelFeature: { properties: { PIN: undefined } } as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: {} as any,
        parcelFeature: { properties: { PIN: '' } } as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: { properties: { PIN: 111112 } } as any,
      },
      { label: NameSourceType.PIN, value: '111112' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: {
          properties: {
            PIN: 1,
            PLAN_NUMBER: 'PB1000',
          },
        } as any,
      },
      { label: NameSourceType.PIN, value: '1' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: {} as any,
        parcelFeature: { properties: { PLAN_NUMBER: undefined } } as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: {} as any,
        parcelFeature: { properties: { PLAN_NUMBER: '' } } as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: { properties: { PLAN_NUMBER: '1' } } as any,
      },
      { label: NameSourceType.PLAN, value: '1' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: {} as any,
        parcelFeature: { properties: { PLAN_NUMBER: 'PB1000' } } as any,
      },
      { label: NameSourceType.PLAN, value: 'PB1000' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: { lat: 1, lng: 2 },
        fileLocation: null,
        pimsFeature: {} as any,
        parcelFeature: {} as any,
      },
      { label: NameSourceType.LOCATION, value: '2.000000, 1.000000' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: { properties: { STREET_ADDRESS_1: undefined } } as any,
        parcelFeature: {} as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: { properties: { STREET_ADDRESS_1: '' } } as any,
        parcelFeature: {} as any,
      },
      { label: NameSourceType.NONE, value: '' },
    ],
    [
      {
        ...getMockSelectedFeatureDataset(),
        location: null,
        pimsFeature: { properties: { STREET_ADDRESS_1: '1234 fake st' } } as any,
        parcelFeature: {} as any,
      },
      { label: NameSourceType.ADDRESS, value: '1234 fake st' },
    ],
  ])(
    'getPropertyNameFromSelectedFeatureSet test with source %o expecting %o',
    (featureSet: SelectedFeatureDataset, expectedName: PropertyName) => {
      const actualName = getPropertyNameFromSelectedFeatureSet(featureSet);
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
    [
      {
        ...getMockSelectedFeatureDataset(),
        pimsFeature: { properties: { PID_PADDED: '123-456-789' } } as any,
      },
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
