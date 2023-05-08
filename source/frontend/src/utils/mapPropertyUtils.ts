import { IMapProperty } from 'components/propertySelector/models';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry, Polygon } from 'geojson';
import { geoJSON } from 'leaflet';
import { compact } from 'lodash';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import polylabel from 'polylabel';
import { formatApiAddress, pidFormatter } from 'utils';

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
  if (property.pid !== undefined && property.pid?.toString().length > 0 && property.pid !== '0') {
    return { label: NameSourceType.PID, value: pidFormatter(property.pid.toString()) };
  } else if (
    property.pin !== undefined &&
    property.pin?.toString()?.length > 0 &&
    property.pin !== '0'
  ) {
    return { label: NameSourceType.PIN, value: property.pin.toString() };
  } else if (property.planNumber !== undefined) {
    return { label: NameSourceType.PLAN, value: property.planNumber };
  } else if (property.latitude !== undefined && property.longitude !== undefined) {
    return {
      label: NameSourceType.LOCATION,
      value: compact([property.longitude?.toFixed(6), property.latitude?.toFixed(6)]).join(', '),
    };
  } else if (property.address !== undefined) {
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
    return { label: NameSourceType.NAME, value: fileProperty.propertyName };
  } else if (fileProperty.property !== undefined) {
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
    ?.filter(feature => feature?.geometry?.type === 'Polygon')
    .map((feature): IMapProperty => {
      const boundedCenter = polylabel((feature.geometry as Polygon).coordinates);
      return toMapProperty(feature, address, boundedCenter[1], boundedCenter[0]);
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
