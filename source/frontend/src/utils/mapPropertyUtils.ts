import {
  Feature,
  FeatureCollection,
  GeoJsonProperties,
  Geometry,
  MultiPolygon,
  Polygon,
} from 'geojson';
import { geoJSON, LatLngLiteral } from 'leaflet';
import { compact, isNumber } from 'lodash';
import polylabel from 'polylabel';
import { toast } from 'react-toastify';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IMapProperty } from '@/components/propertySelector/models';
import { DistrictCodes } from '@/constants/districtCodes';
import { RegionCodes } from '@/constants/regionCodes';
import { Api_PropertyFile } from '@/models/api/PropertyFile';
import { formatApiAddress, pidFormatter } from '@/utils';

import { Api_Geometry, Api_Property } from '../models/api/Property';

export enum NameSourceType {
  PID = 'PID',
  PIN = 'PIN',
  PLAN = 'Plan #',
  LOCATION = 'Location',
  NAME = 'Descriptive Name',
  NONE = 'n/a',
  ADDRESS = 'Address',
}

interface PropertyName {
  label: NameSourceType;
  value: string;
}

export const getPropertyName = (property: IMapProperty): PropertyName => {
  if (!!property.pid && property.pid?.toString().length > 0 && property.pid !== '0') {
    return { label: NameSourceType.PID, value: pidFormatter(property.pid.toString()) };
  } else if (!!property.pin && property.pin?.toString()?.length > 0 && property.pin !== '0') {
    return { label: NameSourceType.PIN, value: property.pin.toString() };
  } else if (!!property.planNumber && property.planNumber?.length > 0) {
    return { label: NameSourceType.PLAN, value: property.planNumber };
  } else if (!!property.latitude && !!property.longitude) {
    return {
      label: NameSourceType.LOCATION,
      value: compact([property.longitude?.toFixed(6), property.latitude?.toFixed(6)]).join(', '),
    };
  } else if (!!property.address) {
    return {
      label: NameSourceType.ADDRESS,
      value: property.address,
    };
  }
  return { label: NameSourceType.NONE, value: '' };
};

export const getPrettyLatLng = (location?: Api_Geometry) =>
  compact([
    location?.coordinate?.x?.toFixed(6) ?? 0,
    location?.coordinate?.y?.toFixed(6) ?? 0,
  ]).join(', ');

export const getLatLng = (location?: Api_Geometry | null): LatLngLiteral | null => {
  const coordinate = location?.coordinate;
  if (coordinate !== null && coordinate !== undefined) {
    return { lat: coordinate.y!, lng: coordinate.x! };
  }
  return null;
};

export const getFilePropertyName = (
  fileProperty?: Api_PropertyFile,
  skipName: boolean = false,
): PropertyName => {
  if (fileProperty === undefined) {
    return { label: NameSourceType.NONE, value: '' };
  }

  if (
    fileProperty.propertyName !== undefined &&
    fileProperty.propertyName !== '' &&
    skipName === false
  ) {
    return { label: NameSourceType.NAME, value: fileProperty.propertyName ?? '' };
  } else if (!!fileProperty.property) {
    const property = fileProperty.property;
    return getApiPropertyName(property);
  }
  return { label: NameSourceType.NONE, value: '' };
};

export const getApiPropertyName = (property?: Api_Property): PropertyName => {
  if (property === undefined) {
    return { label: NameSourceType.NONE, value: '' };
  }

  let mapProperty: IMapProperty = {
    pin: property.pin?.toString(),
    pid: property.pid?.toString(),
    latitude: property.latitude,
    longitude: property.longitude,
    planNumber: property.planNumber,
    address: property.address !== undefined ? formatApiAddress(property.address) : undefined,
  };
  return getPropertyName(mapProperty);
};

export const mapFeatureToProperty = (
  selectedFeature: Feature<Geometry, GeoJsonProperties>,
): IMapProperty => {
  const latLng = selectedFeature?.geometry
    ? geoJSON(selectedFeature.geometry).getBounds().getCenter()
    : undefined;
  const latitude = selectedFeature?.properties?.CLICK_LAT_LNG?.lat ?? latLng?.lat ?? undefined;
  const longitude = selectedFeature?.properties?.CLICK_LAT_LNG?.lng ?? latLng?.lng ?? undefined;
  return toMapProperty(selectedFeature, 'unknown', latitude, longitude);
};

export const featuresToIdentifiedMapProperty = (
  values: FeatureCollection<Geometry, GeoJsonProperties> | undefined,
  address?: string,
) =>
  values?.features
    ?.filter(
      feature =>
        feature?.geometry?.type === 'Polygon' || feature?.geometry?.type === 'MultiPolygon',
    )
    .map((feature): IMapProperty => {
      if (feature?.geometry?.type === 'Polygon') {
        const boundedCenter = polylabel((feature.geometry as Polygon).coordinates);
        return toMapProperty(feature, address, boundedCenter[1], boundedCenter[0]);
      } else if (feature?.geometry?.type === 'MultiPolygon') {
        const boundedCenter = polylabel((feature.geometry as MultiPolygon).coordinates[0]);
        return toMapProperty(feature, address, boundedCenter[1], boundedCenter[0]);
      } else {
        toast.error(
          'Unsupported geometry type, unable to determine bounded center. You will need to drop a pin instead.',
        );
        throw Error(
          'Unsupported geometry type, unable to determine bounded center. You will need to drop a pin instead.',
        );
      }
    });

function toMapProperty(
  feature: Feature<Geometry, GeoJsonProperties>,
  address?: string,
  latitude?: number,
  longitude?: number,
): IMapProperty {
  return {
    propertyId: feature?.properties?.PROPERTY_ID,
    pid: feature?.properties?.PID?.toString() ?? undefined,
    pin: feature?.properties?.PIN?.toString() ?? undefined,
    latitude: latitude,
    longitude: longitude,
    planNumber: feature?.properties?.PLAN_NUMBER?.toString() ?? undefined,
    address: address,
    legalDescription: feature?.properties?.LEGAL_DESCRIPTION,
    region: feature?.properties?.REGION_NUMBER,
    regionName: feature?.properties?.REGION_NAME,
    district: feature?.properties?.DISTRICT_NUMBER,
    districtName: feature?.properties?.DISTRICT_NAME,
    name: feature?.properties?.NAME,
  };
}

export function featuresetToMapProperty(
  featureSet: LocationFeatureDataset,
  address: string = 'unknown',
): IMapProperty {
  const pimsFeature = featureSet.pimsFeature;
  const parcelFeature = featureSet.parcelFeature;
  const regionFeature = featureSet.regionFeature;
  const districtFeature = featureSet.districtFeature;

  const propertyId = pimsFeature?.properties.PROPERTY_ID;
  const pid = pidFromFeatureSet(featureSet);
  const pin = pinFromFeatureSet(featureSet);
  return {
    propertyId: propertyId ? Number.parseInt(propertyId?.toString()) : undefined,
    pid: pid ?? undefined,
    pin: pin ?? undefined,
    latitude: featureSet.location.lat,
    longitude: featureSet.location.lng,
    polygon:
      parcelFeature?.geometry.type === 'Polygon' ? (parcelFeature.geometry as Polygon) : undefined,
    planNumber: parcelFeature?.properties.PLAN_NUMBER?.toString() ?? undefined,
    address: address,
    legalDescription: parcelFeature?.properties.LEGAL_DESCRIPTION ?? undefined,
    region: isNumber(regionFeature?.properties.REGION_NUMBER)
      ? regionFeature?.properties.REGION_NUMBER
      : RegionCodes.Unknown,
    regionName: regionFeature?.properties.REGION_NAME ?? 'Cannot determine',
    district: isNumber(districtFeature?.properties.DISTRICT_NUMBER)
      ? districtFeature?.properties.DISTRICT_NUMBER
      : DistrictCodes.Unknown,
    districtName: districtFeature?.properties.DISTRICT_NAME ?? 'Cannot determine',
    name: pimsFeature?.properties.NAME ?? undefined,
  };
}

export function pidFromFeatureSet(featureset: LocationFeatureDataset): string | null {
  if (featureset.pimsFeature !== null) {
    return featureset.pimsFeature.properties.PID;
  } else if (featureset.parcelFeature !== null) {
    return featureset.parcelFeature.properties.PID;
  } else {
    return null;
  }
}

export function pinFromFeatureSet(featureset: LocationFeatureDataset): string | null {
  if (featureset.pimsFeature !== null) {
    return featureset.pimsFeature.properties.PIN;
  } else if (
    featureset.parcelFeature !== null &&
    featureset.parcelFeature.properties.PIN !== null
  ) {
    return featureset.parcelFeature.properties.PIN.toString();
  } else {
    return null;
  }
}
