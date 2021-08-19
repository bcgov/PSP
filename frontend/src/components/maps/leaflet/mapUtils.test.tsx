import { PropertyTypes } from 'constants/index';
import { IProperty } from 'interfaces';
import { DivIcon, LatLngExpression, Marker } from 'leaflet';

import { ICluster, PointFeature } from '../types';
import {
  asProperty,
  buildingIcon,
  buildingIconSelect,
  createClusterMarker,
  createPoints,
  draftBuildingIcon,
  draftParcelIcon,
  generateKey,
  getMarkerIcon,
  parcelIconSelect,
  pointToLayer,
  subdivisionIconSelect,
} from './mapUtils';
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
        properties: {},
      };
      const latlng: LatLngExpression = { lat: 1, lng: 2 };

      expect(pointToLayer(feature, latlng)).toEqual(new Marker(latlng, { icon: buildingIcon }));
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
        propertyTypeId: PropertyTypes.DraftParcel,
      },
    };
    it('returns a draft parcel icon', () => {
      expect(
        getMarkerIcon({ ...feature, properties: { propertyTypeId: PropertyTypes.DraftParcel } }),
      ).toEqual(draftParcelIcon);
    });
    it('returns a draft building icon', () => {
      expect(
        getMarkerIcon({ ...feature, properties: { propertyTypeId: PropertyTypes.DraftBuilding } }),
      ).toEqual(draftBuildingIcon);
    });

    it('returns a selected default subdivision icon', () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: { propertyTypeId: PropertyTypes.Subdivision },
          },
          true,
        ),
      ).toEqual(subdivisionIconSelect);
    });
    it('returns a default building icon', () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: { propertyTypeId: PropertyTypes.Building },
          },
          true,
        ),
      ).toEqual(buildingIconSelect);
    });
    it('returns a default parcel icon', () => {
      expect(
        getMarkerIcon(
          {
            ...feature,
            properties: { propertyTypeId: PropertyTypes.Parcel },
          },
          true,
        ),
      ).toEqual(parcelIconSelect);
    });
    describe('convert feature to property', () => {
      const property: IProperty = {
        id: 1,
        organization: 'test organization',
        organizationId: 1,
        latitude: 1,
        longitude: 2,
        isSensitive: false,
      };
      it('generates building keys', () => {
        const building: IProperty = { ...property, propertyTypeId: PropertyTypes.Building };
        expect(generateKey(building)).toEqual('building-1');
      });
      it('generates parcel keys', () => {
        const parcel: IProperty = { ...property, propertyTypeId: PropertyTypes.Parcel };
        expect(generateKey(parcel)).toEqual('parcel-1');
      });
    });

    describe('convert feature to property', () => {
      const property: PointFeature = {
        type: 'Feature',
        geometry: { coordinates: [1, 2] } as any,
        properties: { id: 1, propertyTypeId: PropertyTypes.Parcel, name: 'name' },
      };
      it('does the conversion', () => {
        expect(asProperty(property)).toEqual({
          id: 1,
          latitude: 2,
          longitude: 1,
          name: 'name',
          propertyTypeId: PropertyTypes.Parcel,
        });
      });
    });

    describe('create points function', () => {
      const property: IProperty = {
        id: 1,
        organization: 'test organization',
        organizationId: 1,
        latitude: 1,
        longitude: 2,
        isSensitive: false,
      };
      it('converts properties to point features', () => {
        expect(createPoints([property, property])).toEqual([
          {
            geometry: { coordinates: [2, 1], type: 'Point' },
            properties: {
              organization: 'test organization',
              organizationId: 1,
              cluster: false,
              id: 1,
              isSensitive: false,
              latitude: 1,
              longitude: 2,
            },
            type: 'Feature',
          },
          {
            geometry: { coordinates: [2, 1], type: 'Point' },
            properties: {
              organization: 'test organization',
              organizationId: 1,
              cluster: false,
              id: 1,
              isSensitive: false,
              latitude: 1,
              longitude: 2,
            },
            type: 'Feature',
          },
        ]);
      });
    });
  });
});
