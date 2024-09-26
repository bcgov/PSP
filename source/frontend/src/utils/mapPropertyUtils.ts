import { booleanPointInPolygon, point } from '@turf/turf';
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
import { ONE_HUNDRED_METER_PRECISION } from '@/components/maps/constants';
import { IMapProperty } from '@/components/propertySelector/models';
import { AreaUnitTypes } from '@/constants';
import { DistrictCodes } from '@/constants/districtCodes';
import { RegionCodes } from '@/constants/regionCodes';
import { AddressForm } from '@/features/mapSideBar/shared/models';
import { ApiGen_CodeTypes_GeoJsonTypes } from '@/models/api/generated/ApiGen_CodeTypes_GeoJsonTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Geometry } from '@/models/api/generated/ApiGen_Concepts_Geometry';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { enumFromValue, exists, formatApiAddress, isValidId, pidFormatter } from '@/utils';

export enum NameSourceType {
  PID = 'PID',
  PIN = 'PIN',
  PLAN = 'Plan #',
  LOCATION = 'Location',
  NAME = 'Descriptive Name',
  NONE = 'n/a',
  ADDRESS = 'Address',
}

export interface PropertyName {
  label: NameSourceType;
  value: string;
}

export const getPropertyName = (property: IMapProperty): PropertyName => {
  if (!!property?.pid && property?.pid?.toString().length > 0 && property?.pid !== '0') {
    return { label: NameSourceType.PID, value: pidFormatter(property?.pid.toString()) };
  } else if (!!property?.pin && property?.pin?.toString()?.length > 0 && property?.pin !== '0') {
    return { label: NameSourceType.PIN, value: property.pin.toString() };
  } else if (!!property?.planNumber && property?.planNumber?.length > 0) {
    return { label: NameSourceType.PLAN, value: property.planNumber };
  } else if (!!property?.latitude && !!property?.longitude) {
    return {
      label: NameSourceType.LOCATION,
      value: compact([property.longitude?.toFixed(6), property.latitude?.toFixed(6)]).join(', '),
    };
  } else if (property?.address) {
    return {
      label: NameSourceType.ADDRESS,
      value: property.address,
    };
  }
  return { label: NameSourceType.NONE, value: '' };
};

export const getPrettyLatLng = (location: ApiGen_Concepts_Geometry | undefined | null) =>
  compact([
    location?.coordinate?.x?.toFixed(6) ?? 0,
    location?.coordinate?.y?.toFixed(6) ?? 0,
  ]).join(', ');

export const getLatLng = (
  location: ApiGen_Concepts_Geometry | undefined | null,
): LatLngLiteral | null => {
  const coordinate = location?.coordinate;
  if (exists(coordinate)) {
    return { lat: coordinate.y, lng: coordinate.x };
  }
  return null;
};

export function latLngToApiLocation(
  latitude?: number,
  longitude?: number,
): ApiGen_Concepts_Geometry | null {
  if (isNumber(latitude) && isNumber(longitude)) {
    return { coordinate: { x: longitude, y: latitude } };
  }
  return null;
}

export const getFilePropertyName = (
  fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
  skipName = false,
): PropertyName => {
  if (!exists(fileProperty)) {
    return { label: NameSourceType.NONE, value: '' };
  }

  if (exists(fileProperty.propertyName) && fileProperty.propertyName !== '' && skipName === false) {
    return { label: NameSourceType.NAME, value: fileProperty.propertyName ?? '' };
  } else if (exists(fileProperty.property)) {
    const property = fileProperty.property;
    return getApiPropertyName(property);
  }
  return { label: NameSourceType.NONE, value: '' };
};

export const getApiPropertyName = (
  property: ApiGen_Concepts_Property | undefined | null,
): PropertyName => {
  if (!exists(property)) {
    return { label: NameSourceType.NONE, value: '' };
  }

  const mapProperty: IMapProperty = {
    pin: property.pin?.toString(),
    pid: property.pid?.toString(),
    latitude: property.latitude ?? undefined,
    longitude: property.longitude ?? undefined,
    planNumber: property.planNumber ?? undefined,
    address: exists(property.address) ? formatApiAddress(property.address) : undefined,
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
        feature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.Polygon ||
        feature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.MultiPolygon,
    )
    .map((feature): IMapProperty => {
      const boundedCenter = getFeatureBoundedCenter(feature);
      return toMapProperty(feature, address, boundedCenter[1], boundedCenter[0]);
    });

export const getFeatureBoundedCenter = (feature: Feature<Geometry, GeoJsonProperties>) => {
  if (feature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.Polygon) {
    const boundedCenter = polylabel(
      (feature.geometry as Polygon).coordinates,
      ONE_HUNDRED_METER_PRECISION,
    );
    return boundedCenter;
  } else if (feature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.MultiPolygon) {
    const boundedCenter = polylabel(
      (feature.geometry as MultiPolygon).coordinates[0],
      ONE_HUNDRED_METER_PRECISION,
    );
    return boundedCenter;
  } else {
    toast.error(
      'Unsupported geometry type, unable to determine bounded center. You will need to drop a pin instead.',
    );
    throw Error(
      'Unsupported geometry type, unable to determine bounded center. You will need to drop a pin instead.',
    );
  }
};

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
    fileLocation: { lat: latitude, lng: longitude },
    planNumber: feature?.properties?.PLAN_NUMBER?.toString() ?? undefined,
    address: address,
    legalDescription: feature?.properties?.LEGAL_DESCRIPTION,
    region: feature?.properties?.REGION_NUMBER,
    regionName: feature?.properties?.REGION_NAME,
    district: feature?.properties?.DISTRICT_NUMBER,
    districtName: feature?.properties?.DISTRICT_NAME,
    landArea: feature?.properties?.FEATURE_AREA_SQM,
    areaUnit: AreaUnitTypes.SquareMeters,
  };
}

export function featuresetToMapProperty(
  featureSet: LocationFeatureDataset,
  address?: string,
): IMapProperty {
  const pimsFeature = featureSet?.pimsFeature;
  const parcelFeature = featureSet?.parcelFeature;
  const regionFeature = featureSet?.regionFeature;
  const districtFeature = featureSet?.districtFeature;

  const propertyId = pimsFeature?.properties?.PROPERTY_ID;
  const pid = pidFromFeatureSet(featureSet);
  const pin = pinFromFeatureSet(featureSet);
  const formattedAddress = pimsFeature?.properties?.STREET_ADDRESS_1
    ? formatApiAddress(AddressForm.fromPimsView(pimsFeature.properties).toApi())
    : undefined;
  if (featureSet === undefined) {
    return undefined;
  }
  return {
    propertyId: propertyId ? Number.parseInt(propertyId?.toString()) : undefined,
    pid: pid ?? undefined,
    pin: pin ?? undefined,
    latitude: featureSet?.location?.lat,
    longitude: featureSet?.location?.lng,
    fileLocation: featureSet?.fileLocation ?? featureSet?.location ?? undefined,
    polygon:
      parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.Polygon
        ? (parcelFeature.geometry as Polygon)
        : parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.MultiPolygon
        ? (parcelFeature.geometry as MultiPolygon)
        : undefined,
    planNumber:
      pimsFeature?.properties?.SURVEY_PLAN_NUMBER ??
      parcelFeature?.properties?.PLAN_NUMBER ??
      undefined,
    address: address ?? formattedAddress ?? undefined,
    legalDescription:
      pimsFeature?.properties?.LAND_LEGAL_DESCRIPTION ??
      parcelFeature?.properties?.LEGAL_DESCRIPTION ??
      undefined,
    region: isNumber(regionFeature?.properties?.REGION_NUMBER)
      ? regionFeature?.properties?.REGION_NUMBER
      : RegionCodes.Unknown,
    regionName: regionFeature?.properties?.REGION_NAME ?? 'Cannot determine',
    district: isNumber(districtFeature?.properties?.DISTRICT_NUMBER)
      ? districtFeature?.properties?.DISTRICT_NUMBER
      : DistrictCodes.Unknown,
    districtName: districtFeature?.properties?.DISTRICT_NAME ?? 'Cannot determine',
    areaUnit: pimsFeature?.properties?.PROPERTY_AREA_UNIT_TYPE_CODE
      ? enumFromValue(pimsFeature?.properties?.PROPERTY_AREA_UNIT_TYPE_CODE, AreaUnitTypes)
      : AreaUnitTypes.SquareMeters,
    landArea: pimsFeature?.properties?.LAND_AREA
      ? +pimsFeature?.properties?.LAND_AREA
      : parcelFeature?.properties?.FEATURE_AREA_SQM ?? 0,
  };
}

export function pidFromFeatureSet(featureset: LocationFeatureDataset): string | null {
  return (
    featureset?.pimsFeature?.properties?.PID?.toString() ??
    featureset?.parcelFeature?.properties?.PID ??
    null
  );
}

export function pinFromFeatureSet(featureset: LocationFeatureDataset): string | null {
  return isValidId(featureset?.pimsFeature?.properties?.PIN)
    ? featureset?.pimsFeature?.properties?.PIN?.toString()
    : isValidId(featureset?.parcelFeature?.properties?.PIN)
    ? featureset?.parcelFeature?.properties?.PIN?.toString()
    : null;
}

export function locationFromFileProperty(
  fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
): ApiGen_Concepts_Geometry | null {
  return fileProperty?.location ?? fileProperty?.property?.location ?? null;
}

export function latLngFromMapProperty(
  mapProperty: IMapProperty | undefined | null,
): LatLngLiteral | null {
  return {
    lat: Number(mapProperty?.fileLocation?.lat ?? mapProperty?.latitude ?? 0),
    lng: Number(mapProperty?.fileLocation?.lng ?? mapProperty?.longitude ?? 0),
  };
}

/**
 * Takes a (Lat, Long) value and a FeatureSet and determines if the point resides inside the polygon.
 * The polygon can be convex or concave. The function accounts for holes.
 *
 * @param latLng The input lat/long
 * @param featureset The input featureset
 * @returns true if the Point is inside the FeatureSet boundary; false if the Point is not inside the boundary
 */
export function isLatLngInFeatureSetBoundary(
  latLng: LatLngLiteral,
  featureset: LocationFeatureDataset,
): boolean {
  const location = point([latLng.lng, latLng.lat]);
  const boundary = (featureset?.pimsFeature?.geometry ?? featureset?.parcelFeature?.geometry) as
    | Polygon
    | MultiPolygon;

  return exists(boundary) && booleanPointInPolygon(location, boundary);
}
