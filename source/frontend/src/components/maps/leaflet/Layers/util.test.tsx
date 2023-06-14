import { DivIcon, LatLngExpression, Marker } from 'leaflet';

import {
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from '@/constants/index';
import { IProperty } from '@/interfaces';

import { ICluster } from '../../types';
import {
  createClusterMarker,
  createPoints,
  getMarkerIcon,
  parcelIcon,
  parcelIconSelect,
  pointToLayer,
} from './util';
describe('mapUtils tests', () => {
  describe('pointToLayer function', () => {
    it('converts a feature and latlng expression into a layer', () => {
      const feature: ICluster = {
        id: 1,
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [1, 2],
        },
        properties: { PROPERTY_ID: 1 },
      };
      const latlng: LatLngExpression = { lat: 1, lng: 2 };

      expect(pointToLayer(feature, latlng)).toEqual(new Marker(latlng, { icon: parcelIcon }));
    });

    it('converts a cluster and latlng expression into a layer', () => {
      const feature: ICluster = {
        id: 1,
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [1, 2],
        },
        properties: { cluster: true, point_count_abbreviated: 100 },
      };
      const latlng: LatLngExpression = { lat: 1, lng: 2 };
      const icon = new DivIcon({
        html: `<div><span>100</span></div>`,
        className: `marker-cluster marker-cluster-large`,
        iconSize: [40, 40],
      });

      expect(pointToLayer(feature, latlng)).toEqual(new Marker(latlng, { icon }));
    });
  });
  describe('createClusterMarker function', () => {
    it('returns null when passed a feature that is not a cluster', () => {
      const feature: ICluster = {
        id: 1,
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [1, 2],
        },
        properties: { cluster: false, point_count_abbreviated: 100 },
      };
      const latlng: LatLngExpression = { lat: 1, lng: 2 };
      expect(createClusterMarker(feature, latlng)).toBeNull();
    });
  });
  describe('getMarkericon function', () => {
    const feature: ICluster = {
      id: 1,
      type: 'Feature',
      geometry: {
        type: 'Point',
        coordinates: [1, 2],
      },
      properties: {
        cluster: false,
        point_count_abbreviated: 100,
      },
    };
    it('returns a default parcel icon', () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: { PROPERTY_ID: 1 },
          },
          true,
        ),
      ).toEqual(parcelIconSelect);
    });

    describe('create points function', () => {
      const property: IProperty = {
        id: 1,
        pid: '000-000-001',
        statusId: PropertyStatusTypes.UnderAdmin,
        classificationId: PropertyClassificationTypes.CoreOperational,
        tenureId: PropertyTenureTypes.HighwayRoad,
        dataSourceId: PropertyDataSourceTypes.PAIMS,
        dataSourceEffectiveDate: '2021-08-30T18:11:13.883Z',
        address: {
          streetAddress1: '1243 St',
          provinceId: 1,
          municipality: '',
          postal: '',
        },
        regionId: 1,
        districtId: 1,
        areaUnitId: PropertyAreaUnitTypes.Hectare,
        landArea: 0,
        landLegalDescription: '',
        latitude: 1,
        longitude: 2,
        isSensitive: false,
      };
      it('converts properties to point features', () => {
        expect(createPoints([property, property])).toEqual([
          {
            geometry: { coordinates: [2, 1], type: 'Point' },
            properties: {
              PROPERTY_ID: 1,
              address: {
                streetAddress1: '1243 St',
                provinceId: 1,
                municipality: '',
                postal: '',
              },
              areaUnitId: PropertyAreaUnitTypes.Hectare,
              classificationId: PropertyClassificationTypes.CoreOperational,
              dataSourceId: PropertyDataSourceTypes.PAIMS,
              dataSourceEffectiveDate: property.dataSourceEffectiveDate,
              districtId: 1,
              cluster: false,
              id: 1,
              isSensitive: false,
              latitude: 1,
              longitude: 2,
              tenureId: PropertyTenureTypes.HighwayRoad,
              statusId: PropertyStatusTypes.UnderAdmin,
              regionId: 1,
              landArea: 0,
              landLegalDescription: '',
              pid: '000-000-001',
            },
            type: 'Feature',
          },
          {
            geometry: { coordinates: [2, 1], type: 'Point' },
            properties: {
              PROPERTY_ID: 1,
              address: {
                streetAddress1: '1243 St',
                provinceId: 1,
                municipality: '',
                postal: '',
              },
              areaUnitId: PropertyAreaUnitTypes.Hectare,
              classificationId: PropertyClassificationTypes.CoreOperational,
              dataSourceId: PropertyDataSourceTypes.PAIMS,
              dataSourceEffectiveDate: property.dataSourceEffectiveDate,
              districtId: 1,
              cluster: false,
              id: 1,
              isSensitive: false,
              latitude: 1,
              longitude: 2,
              tenureId: PropertyTenureTypes.HighwayRoad,
              statusId: PropertyStatusTypes.UnderAdmin,
              regionId: 1,
              landArea: 0,
              landLegalDescription: '',
              pid: '000-000-001',
            },
            type: 'Feature',
          },
        ]);
      });
    });
  });
});
