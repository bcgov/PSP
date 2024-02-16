import { DivIcon, LatLngExpression, Marker } from 'leaflet';
import { ClusterFeature, ClusterProperties } from 'supercluster';

import {
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from '@/constants/index';
import { toCqlFilterValue } from '@/hooks/layer-api/layerUtils';
import { IProperty } from '@/interfaces';
import {
  EmptyPropertyLocation,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';

import { ICluster } from '../../types';
import {
  createClusterMarker,
  createPoints,
  getMarkerIcon,
  otherInterestIcon,
  otherInterestIconSelect,
  pointToLayer,
} from './util';
describe('mapUtils tests', () => {
  describe('pointToLayer function', () => {
    it('converts a feature and latlng expression into a layer', () => {
      const feature: ICluster<PIMS_Property_Location_View> = {
        id: 'PIMS_PROPERTY_LOCATION_VW.1234',
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [1, 2],
        },
        properties: { ...EmptyPropertyLocation, PROPERTY_ID: '1' },
      };
      const latlng: LatLngExpression = { lat: 1, lng: 2 };

      expect(pointToLayer(feature, latlng)).toEqual(
        new Marker(latlng, { icon: otherInterestIcon }),
      );
    });

    it('converts a cluster and latlng expression into a layer', () => {
      const feature: ClusterFeature<ClusterProperties> = {
        id: 1,
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [1, 2],
        },
        properties: {
          cluster: true,
          point_count_abbreviated: 1000,
          cluster_id: 1,
          point_count: 1000,
        },
      };
      const latlng: LatLngExpression = { lat: 1, lng: 2 };
      const icon = new DivIcon({
        html: `<div><span>1000</span></div>`,
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
            properties: { ...EmptyPropertyLocation, PROPERTY_ID: '1' },
          },
          true,
        ),
      ).toEqual(otherInterestIconSelect);
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
  describe('toCqlFilter function', () => {
    it('by default, joins multiple filters with and and inserts ilike', () => {
      const cql = toCqlFilterValue({ PID: '12345678', PIN: '54321' });
      expect(cql).toBe("PID ilike '%12345678%' AND PIN ilike '%54321%'");
    });

    it('will join multiple filters with or if force param specified', () => {
      const cql = toCqlFilterValue({ PID: '12345678', PIN: '54321' }, { useCqlOr: true });
      expect(cql).toBe("PID ilike '%12345678%' OR PIN ilike '%54321%'");
    });

    it('if pid is 9 characters will automatically use = instead of ilike', () => {
      const cql = toCqlFilterValue({ PID: '123456789' });
      expect(cql).toBe("PID = '123456789'");
    });

    it('if force exact param is specified will automatically use = instead of ilike', () => {
      const cql = toCqlFilterValue({ PID: '12345678', PIN: '54321' }, { forceExactMatch: true });
      expect(cql).toBe("PID = '12345678' AND PIN='54321'");
    });
  });
});
