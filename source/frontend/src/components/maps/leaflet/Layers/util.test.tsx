import { DivIcon, LatLngExpression, Marker } from 'leaflet';
import { ClusterFeature, ClusterProperties } from 'supercluster';

import {
  PropertyAreaUnitTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from '@/constants/index';
import { toCqlFilterValue } from '@/hooks/layer-api/layerUtils';
import {
  emptyPropertyLocation,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';

import { createPoints } from '../../MapView.test';
import { ICluster } from '../../types';
import {
  createClusterMarker,
  disposedIcon,
  getMarkerIcon,
  otherInterestIcon,
  parcelIcon,
  pointToLayer,
  propertyOfInterestIcon,
} from './util';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
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
        properties: { ...emptyPropertyLocation, PROPERTY_ID: 1, IS_OTHER_INTEREST: true },
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

  describe('getMarkerIcon function', () => {
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
    it(`returns a valid map pin icon for 'Core Inventory'`, () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: { ...emptyPropertyLocation, PROPERTY_ID: 1, IS_OWNED: true },
          },
          false,
        ),
      ).toEqual(parcelIcon);
    });
    it(`returns a valid map pin icon for 'Property of Interest'`, () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: {
              ...emptyPropertyLocation,
              PROPERTY_ID: 1,
              HAS_ACTIVE_RESEARCH_FILE: true,
            },
          },
          false,
        ),
      ).toEqual(propertyOfInterestIcon);
    });
    it(`returns a valid map pin icon for 'Other Interest'`, () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: { ...emptyPropertyLocation, PROPERTY_ID: 1, IS_OTHER_INTEREST: true },
          },
          false,
        ),
      ).toEqual(otherInterestIcon);
    });
    it(`returns a valid map pin icon for 'Disposed'`, () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: { ...emptyPropertyLocation, PROPERTY_ID: 1, IS_DISPOSED: true },
          },
          false,
          true,
        ),
      ).toEqual(disposedIcon);
    });
    it(`follows precedence when property has multiple ownership flags`, () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: {
              ...emptyPropertyLocation,
              PROPERTY_ID: 1,
              IS_OWNED: true,
              HAS_ACTIVE_ACQUISITION_FILE: true,
            },
          },
          false,
        ),
      ).toEqual(parcelIcon);
    });
    it(`returns null when passed feature is not one of: 'Core Inventory', 'Property of Interest', 'Other Interest' or 'Disposed'`, () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: {
              ...emptyPropertyLocation,
              PROPERTY_ID: 1,
              IS_OWNED: null,
              HAS_ACTIVE_ACQUISITION_FILE: null,
              HAS_ACTIVE_RESEARCH_FILE: null,
              IS_OTHER_INTEREST: null,
              IS_DISPOSED: null,
            },
          },
          true,
        ),
      ).toBeNull();
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

    it('if property id is specified, exact search should always be used', () => {
      const cql = toCqlFilterValue({ PROPERTY_ID: '1' });
      expect(cql).toBe("PROPERTY_ID = '1'");
    });
  });
});
