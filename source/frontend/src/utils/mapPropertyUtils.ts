import { booleanPointInPolygon, point } from '@turf/turf';
import {
  Feature,
  FeatureCollection,
  GeoJsonProperties,
  Geometry,
  MultiPolygon,
  Point,
  Polygon,
} from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { chain, compact, isNumber } from 'lodash';
import polylabel from 'polylabel';
import { toast } from 'react-toastify';

import { MapFeatureData } from '@/components/common/mapFSM/models';
import {
  FeatureDataset,
  LocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import { ONE_HUNDRED_METER_PRECISION } from '@/components/maps/constants';
import { AddressForm, PropertyForm } from '@/features/mapSideBar/shared/models';
import { ApiGen_CodeTypes_GeoJsonTypes } from '@/models/api/generated/ApiGen_CodeTypes_GeoJsonTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Geometry } from '@/models/api/generated/ApiGen_Concepts_Geometry';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { exists, firstOrNull, formatApiAddress, formatSplitAddress, pidFormatter } from '@/utils';

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

export const getPropertyNameFromLocationFeatureSet = (
  selectedFeature: LocationFeatureDataset | null,
): PropertyName => {
  if (!exists(selectedFeature)) {
    return { label: NameSourceType.NONE, value: '' };
  }

  const pid = pidFromFeatureSet(selectedFeature);
  const pin = pinFromFeatureSet(selectedFeature);
  const planNumber = planFromFeatureSet(selectedFeature);
  const address = addressFromFeatureSet(selectedFeature);
  const location = selectedFeature.location;

  if (exists(pid) && pid?.toString()?.length > 0 && pid !== '0') {
    return { label: NameSourceType.PID, value: pidFormatter(pid.toString()) };
  } else if (exists(pin) && pin?.toString()?.length > 0 && pin !== '0') {
    return { label: NameSourceType.PIN, value: pin.toString() };
  } else if (exists(planNumber) && planNumber?.toString()?.length > 0) {
    return { label: NameSourceType.PLAN, value: planNumber.toString() };
  } else if (exists(location?.lat) && exists(location?.lng)) {
    return {
      label: NameSourceType.LOCATION,
      value: compact([location.lng?.toFixed(6), location.lat?.toFixed(6)]).join(', '),
    };
  } else if (exists(address) && address?.length > 0) {
    return { label: NameSourceType.ADDRESS, value: address ?? '' };
  }

  return { label: NameSourceType.NONE, value: '' };
};

export const getPropertyNameFromForm = (propertyForm: PropertyForm | null): PropertyName => {
  if (!exists(propertyForm)) {
    return { label: NameSourceType.NONE, value: '' };
  }

  const pid = propertyForm.pid;
  const pin = propertyForm.pin;
  const planNumber = propertyForm.planNumber;
  const address = propertyForm.address;
  const location = propertyForm.fileLocation;

  if (exists(pid) && pid?.toString()?.length > 0 && pid !== '0') {
    return { label: NameSourceType.PID, value: pidFormatter(pid.toString()) };
  } else if (exists(pin) && pin?.toString()?.length > 0 && pin !== '0') {
    return { label: NameSourceType.PIN, value: pin.toString() };
  } else if (exists(planNumber) && planNumber?.toString()?.length > 0) {
    return { label: NameSourceType.PLAN, value: planNumber.toString() };
  } else if (exists(location?.lat) && exists(location?.lng)) {
    return {
      label: NameSourceType.LOCATION,
      value: compact([location.lng?.toFixed(6), location.lat?.toFixed(6)]).join(', '),
    };
  } else if (exists(address)) {
    return {
      label: NameSourceType.ADDRESS,
      value:
        formatSplitAddress(
          address.streetAddress1,
          address.streetAddress1,
          address.streetAddress3,
          address.municipality,
          address.province,
          address.postalCode,
        ) ?? '',
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

  const pid = property.pid?.toString();
  const pin = property.pin?.toString();
  const planNumber = property.planNumber;
  const address = exists(property.address) ? formatApiAddress(property.address) : null;
  const latitude = property.latitude;
  const longitude = property.longitude;

  if (exists(pid) && pid.length > 0 && pid !== '0') {
    return { label: NameSourceType.PID, value: pidFormatter(pid.toString()) };
  } else if (exists(pin) && pin.length > 0 && pin !== '0') {
    return { label: NameSourceType.PIN, value: pin.toString() };
  } else if (exists(planNumber) && planNumber?.toString()?.length > 0) {
    return { label: NameSourceType.PLAN, value: planNumber.toString() };
  } else if (exists(latitude) && exists(longitude)) {
    return {
      label: NameSourceType.LOCATION,
      value: compact([longitude.toFixed(6), latitude.toFixed(6)]).join(', '),
    };
  } else if (exists(address) && address?.length > 0) {
    return { label: NameSourceType.ADDRESS, value: address ?? '' };
  }

  return { label: NameSourceType.NONE, value: '' };
};

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

export function pidFromFullyAttributedFeature(
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
): string | null {
  return exists(parcelFeature?.properties) ? parcelFeature?.properties?.PID?.toString() : null;
}

export function pinFromFullyAttributedFeature(
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
): string | null {
  return exists(parcelFeature?.properties) ? parcelFeature?.properties?.PIN?.toString() : null;
}

export function planFromFullyAttributedFeature(
  parcelFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> | null,
): string | null {
  return exists(parcelFeature?.properties)
    ? parcelFeature?.properties?.PLAN_NUMBER?.toString()
    : null;
}

export function pidFromFeatureSet(featureset: FeatureDataset): string | null {
  const pimsFeature = firstOrNull(featureset?.pimsFeatures);
  if (exists(pimsFeature?.properties)) {
    return exists(pimsFeature?.properties?.PID_PADDED)
      ? pimsFeature?.properties?.PID_PADDED?.toString()
      : null;
  }

  const parcelFeature = firstOrNull(featureset?.parcelFeatures);
  return pidFromFullyAttributedFeature(parcelFeature);
}

export function pinFromFeatureSet(featureset: FeatureDataset): string | null {
  const pimsFeature = firstOrNull(featureset?.pimsFeatures);
  if (exists(pimsFeature?.properties)) {
    return exists(pimsFeature?.properties?.PIN) ? pimsFeature?.properties?.PIN?.toString() : null;
  }

  const parcelFeature = firstOrNull(featureset?.parcelFeatures);
  return pinFromFullyAttributedFeature(parcelFeature);
}

export function planFromFeatureSet(featureset: FeatureDataset): string | null {
  const pimsFeature = firstOrNull(featureset?.pimsFeatures);
  if (exists(pimsFeature?.properties)) {
    return exists(pimsFeature?.properties?.SURVEY_PLAN_NUMBER)
      ? pimsFeature?.properties?.SURVEY_PLAN_NUMBER?.toString()
      : null;
  }

  const parcelFeature = firstOrNull(featureset?.parcelFeatures);
  return planFromFullyAttributedFeature(parcelFeature);
}

export function addressFromFeatureSet(featureset: FeatureDataset): string | null {
  const pimsFeature = firstOrNull(featureset?.pimsFeatures);
  if (exists(pimsFeature?.properties)) {
    return exists(pimsFeature?.properties?.STREET_ADDRESS_1)
      ? formatApiAddress(AddressForm.fromPimsView(pimsFeature?.properties).toApi())
      : null;
  }

  return null;
}

export function locationFromFileProperty(
  fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
): ApiGen_Concepts_Geometry | null {
  return fileProperty?.location ?? fileProperty?.property?.location ?? null;
}

export function boundaryFromFileProperty(
  fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
): Geometry | null {
  return (
    fileProperty?.property?.boundary ??
    pimsGeomeryToGeometry(fileProperty?.property?.location) ??
    null
  );
}

export function latLngLiteralToGeometry(latLng: LatLngLiteral | null | undefined): Point | null {
  if (exists(latLng)) {
    return { type: 'Point', coordinates: [latLng.lng, latLng.lat] };
  }
  return null;
}

export function pimsGeomeryToGeometry(
  pimsGeomery: ApiGen_Concepts_Geometry | null | undefined,
): Point | null {
  if (exists(pimsGeomery?.coordinate)) {
    return { type: 'Point', coordinates: [pimsGeomery.coordinate.x, pimsGeomery.coordinate.y] };
  }
  return null;
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
  featureset: FeatureDataset,
): boolean {
  // TODO: Make this work with the all of the features instead of just the first one
  const pimsFeature = firstOrNull(featureset?.pimsFeatures);
  const parcelFeature = firstOrNull(featureset?.parcelFeatures);

  const location = point([latLng.lng, latLng.lat]);
  const boundary = (pimsFeature?.geometry ?? parcelFeature?.geometry) as Polygon | MultiPolygon;

  return exists(boundary) && booleanPointInPolygon(location, boundary);
}

/**
 * Preserves the order of the properties within a file
 * @param fileProperties The file properties
 */
export function applyDisplayOrder<T extends ApiGen_Concepts_FileProperty>(fileProperties: T[]) {
  return fileProperties.map((fp, index) => {
    fp.displayOrder = index;
    return fp;
  });
}

/**
 * Sorts file properties based on their `displayOrder`
 * @param fileProperties The file properties to sort
 * @returns The sorted set of file properties
 */
export function sortFileProperties<T extends ApiGen_Concepts_FileProperty>(
  fileProperties: T[] | null,
): T[] | null {
  if (exists(fileProperties)) {
    return chain(fileProperties)
      .orderBy([fp => fp.displayOrder ?? Infinity], ['asc'])
      .value();
  }
  return null;
}

export function areFeatureDatasetsEqual(p1: FeatureDataset, p2: FeatureDataset): boolean {
  if (!exists(p1) || !exists(p2)) {
    return false;
  }

  const pid1 = pidFromFeatureSet(p1) ?? null;
  const pid2 = pidFromFeatureSet(p2) ?? null;
  if (exists(pid1) && pid1 === pid2) {
    return true;
  }

  const pin1 = pinFromFeatureSet(p1) ?? null;
  const pin2 = pinFromFeatureSet(p2) ?? null;

  if (exists(pin1) && pin1 === pin2) {
    return true;
  }

  // Some parcels are only identified by their plan-number (e.g. common strata, parks)
  // Only consider plan-number as an identifier when there are no PID/PIN
  const planNumber1 = planFromFeatureSet(p1) ?? null;
  const planNumber2 = planFromFeatureSet(p2) ?? null;
  if (
    exists(planNumber1) &&
    !exists(pid1) &&
    !exists(pin1) &&
    !exists(pid2) &&
    !exists(pin2) &&
    planNumber1 === planNumber2
  ) {
    return true;
  }

  return false;
}

export function areLocationFeatureDatasetsEqual(
  p1: LocationFeatureDataset,
  p2: LocationFeatureDataset,
): boolean {
  if (!exists(p1) || !exists(p2)) {
    return false;
  }

  if (areFeatureDatasetsEqual(p1, p2)) {
    return true;
  }

  // Only consider lat/long when there are no PID/PIN
  const location1 = p1.location;
  const location2 = p2.location;
  const pid1 = pidFromFeatureSet(p1) ?? null;
  const pid2 = pidFromFeatureSet(p2) ?? null;
  const pin1 = pinFromFeatureSet(p1) ?? null;
  const pin2 = pinFromFeatureSet(p2) ?? null;
  if (
    exists(location1) &&
    exists(location2) &&
    !exists(pid1) &&
    !exists(pin1) &&
    !exists(pid2) &&
    !exists(pin2) &&
    location1.lat === location2.lat &&
    location1.lng === location2.lng
  ) {
    return true;
  }

  return false;
}

export const isEmptyFeatureCollection = (collection: FeatureCollection) => {
  return !(exists(collection?.features) && collection.features.length > 0);
};

export const isEmptyMapFeatureData = (mapFeatureData: MapFeatureData) => {
  return (
    isEmptyFeatureCollection(mapFeatureData.pimsLocationFeatures) &&
    //isEmptyFeatureCollection(mapFeatureData.pimsLocationLiteFeatures) && TODO: For now this is loading always. Investigate if it needs to be removed completly
    isEmptyFeatureCollection(mapFeatureData.pimsBoundaryFeatures) &&
    isEmptyFeatureCollection(mapFeatureData.fullyAttributedFeatures) &&
    isEmptyFeatureCollection(mapFeatureData.surveyedParcelsFeatures) &&
    isEmptyFeatureCollection(mapFeatureData.highwayPlanFeatures)
  );
};

export const arePropertyFormsEqual = (lhs: PropertyForm, rhs: PropertyForm): boolean => {
  if (!exists(lhs) || !exists(rhs)) {
    return exists(lhs) === exists(rhs);
  }
  return areLocationFeatureDatasetsEqual(
    lhs.toLocationFeatureDataset(),
    rhs.toLocationFeatureDataset(),
  );
};

export interface RegionDistrictResult {
  regionResult: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties>;
  districtResult: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties>;
}

export function latLngToKey(location: LatLngLiteral | null | undefined) {
  if (exists(location)) {
    const key = `${location.lat}-${location.lng}`;

    return key;
  }
  return '0-0';
}

/**
 * Fetches region and district results for a list of properties.
 * @param properties The properties to search for region and district.
 * @param regionSearch Function to search for the region.
 * @param districtSearch Function to search for the district.
 * @returns A Map where keys are lat-lng strings and values are RegionDistrictResult objects.
 */
export async function getRegionAndDistrictsResults(
  properties: LocationFeatureDataset[],
  regionSearch: (
    latlng: LatLngLiteral,
    geometryName?: string | undefined,
    spatialReferenceId?: number | undefined,
  ) => Promise<Feature<Geometry, MOT_RegionalBoundary_Feature_Properties> | undefined>,
  districtSearch: (
    latlng: LatLngLiteral,
    geometryName?: string | undefined,
    spatialReferenceId?: number | undefined,
  ) => Promise<Feature<Geometry, MOT_DistrictBoundary_Feature_Properties> | undefined>,
): Promise<Map<string, RegionDistrictResult>> {
  const latLngMap = new Map<string, LatLngLiteral>();

  for (const property of properties) {
    if (!exists(property?.location?.lat) || !exists(property?.location?.lng)) {
      continue;
    }

    const key = latLngToKey(property.location);

    if (!latLngMap.has(key)) {
      latLngMap.set(key, property.location);
    }
  }

  // Prepare all parallel tasks
  const entries = Array.from(latLngMap.entries()).map(([key, latLng]) =>
    Promise.all([regionSearch(latLng, 'SHAPE'), districtSearch(latLng, 'SHAPE')]).then(
      ([regionResult, districtResult]): [string, RegionDistrictResult] => [
        key,
        {
          regionResult: regionResult ?? null,
          districtResult: districtResult ?? null,
        },
      ],
    ),
  );

  // Resolve in parallel
  const results = await Promise.all(entries);

  // Convert back into a Map
  return new Map(results);
}
