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
import { geoJSON, LatLngLiteral } from 'leaflet';
import { chain, compact, isNumber } from 'lodash';
import polylabel from 'polylabel';
import { toast } from 'react-toastify';

import { LocationBoundaryDataset } from '@/components/common/mapFSM/models';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { ONE_HUNDRED_METER_PRECISION } from '@/components/maps/constants';
import { IMapProperty } from '@/components/propertySelector/models';
import { AreaUnitTypes } from '@/constants';
import { DistrictCodes } from '@/constants/districtCodes';
import { RegionCodes } from '@/constants/regionCodes';
import { AddressForm, PropertyForm } from '@/features/mapSideBar/shared/models';
import { ApiGen_CodeTypes_GeoJsonTypes } from '@/models/api/generated/ApiGen_CodeTypes_GeoJsonTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Geometry } from '@/models/api/generated/ApiGen_Concepts_Geometry';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { enumFromValue, exists, formatApiAddress, pidFormatter } from '@/utils';

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

export const getPropertyNameFromSelectedFeatureSet = (
  selectedFeature: SelectedFeatureDataset | null,
): PropertyName => {
  if (!exists(selectedFeature)) {
    return { label: NameSourceType.NONE, value: '' };
  }

  const pid = pidFromFeatureSet(selectedFeature);
  const pin = pinFromFeatureSet(selectedFeature);
  const planNumber = planFromFeatureSet(selectedFeature);

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
  featureSet: SelectedFeatureDataset,
  address?: string,
): IMapProperty {
  if (!exists(featureSet)) {
    return undefined;
  }
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

  const commonFeature = {
    propertyId: propertyId ? Number.parseInt(propertyId?.toString()) : undefined,
    pid: pid ?? undefined,
    pin: pin ?? undefined,
    latitude: featureSet?.location?.lat,
    longitude: featureSet?.location?.lng,
    fileLocation: featureSet?.fileLocation ?? featureSet?.location ?? undefined,
    region: isNumber(regionFeature?.properties?.REGION_NUMBER)
      ? regionFeature?.properties?.REGION_NUMBER
      : RegionCodes.Unknown,
    regionName: regionFeature?.properties?.REGION_NAME ?? 'Cannot determine',
    district: isNumber(districtFeature?.properties?.DISTRICT_NUMBER)
      ? districtFeature?.properties?.DISTRICT_NUMBER
      : DistrictCodes.Unknown,
    districtName: districtFeature?.properties?.DISTRICT_NAME ?? 'Cannot determine',
  };
  if (exists(pimsFeature?.properties)) {
    return {
      ...commonFeature,
      polygon:
        pimsFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.Polygon
          ? (pimsFeature.geometry as Polygon)
          : pimsFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.MultiPolygon
          ? (pimsFeature.geometry as MultiPolygon)
          : parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.Polygon
          ? (parcelFeature.geometry as Polygon)
          : parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.MultiPolygon
          ? (parcelFeature.geometry as MultiPolygon)
          : undefined,
      planNumber: pimsFeature?.properties?.SURVEY_PLAN_NUMBER ?? undefined,
      address: address ?? formattedAddress ?? undefined,
      legalDescription: pimsFeature?.properties?.LAND_LEGAL_DESCRIPTION ?? undefined,
      areaUnit: pimsFeature?.properties?.PROPERTY_AREA_UNIT_TYPE_CODE
        ? enumFromValue(pimsFeature?.properties?.PROPERTY_AREA_UNIT_TYPE_CODE, AreaUnitTypes)
        : AreaUnitTypes.SquareMeters,
      landArea: pimsFeature?.properties?.LAND_AREA ? +pimsFeature?.properties?.LAND_AREA : 0,
    };
  } else {
    return {
      ...commonFeature,
      polygon:
        parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.Polygon
          ? (parcelFeature.geometry as Polygon)
          : parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.MultiPolygon
          ? (parcelFeature.geometry as MultiPolygon)
          : undefined,
      planNumber: parcelFeature?.properties?.PLAN_NUMBER ?? undefined,
      address: address ?? formattedAddress ?? undefined,
      legalDescription: parcelFeature?.properties?.LEGAL_DESCRIPTION ?? undefined,
      areaUnit: AreaUnitTypes.SquareMeters,
      landArea: parcelFeature?.properties?.FEATURE_AREA_SQM ?? 0,
    };
  }
}

export const featuresetToLocationBoundaryDataset = (
  featureSet: SelectedFeatureDataset,
): LocationBoundaryDataset => {
  return {
    location: featureSet?.fileLocation ?? featureSet?.location,
    boundary: featureSet?.pimsFeature?.geometry ?? featureSet?.parcelFeature?.geometry ?? null,
    isActive: featureSet.isActive,
  };
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

export function pidFromFeatureSet(featureset: SelectedFeatureDataset): string | null {
  if (exists(featureset.pimsFeature?.properties)) {
    return exists(featureset.pimsFeature?.properties?.PID_PADDED)
      ? featureset.pimsFeature?.properties?.PID_PADDED?.toString()
      : null;
  }

  return pidFromFullyAttributedFeature(featureset.parcelFeature);
}

export function pinFromFeatureSet(featureset: SelectedFeatureDataset): string | null {
  if (exists(featureset.pimsFeature?.properties)) {
    return exists(featureset.pimsFeature?.properties?.PIN)
      ? featureset.pimsFeature?.properties?.PIN?.toString()
      : null;
  }

  return pinFromFullyAttributedFeature(featureset.parcelFeature);
}

export function planFromFeatureSet(featureset: SelectedFeatureDataset): string | null {
  if (exists(featureset.pimsFeature?.properties)) {
    return exists(featureset.pimsFeature?.properties?.SURVEY_PLAN_NUMBER)
      ? featureset.pimsFeature?.properties?.SURVEY_PLAN_NUMBER?.toString()
      : null;
  }

  return planFromFullyAttributedFeature(featureset.parcelFeature);
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

export function latLngFromMapProperty(
  mapProperty: IMapProperty | undefined | null,
): LatLngLiteral | null {
  return {
    lat: Number(mapProperty?.fileLocation?.lat ?? mapProperty?.latitude ?? 0),
    lng: Number(mapProperty?.fileLocation?.lng ?? mapProperty?.longitude ?? 0),
  };
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

export function filePropertyToLocationBoundaryDataset(
  fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
): LocationBoundaryDataset | null {
  const geom = locationFromFileProperty(fileProperty);
  const location = getLatLng(geom);
  return exists(location)
    ? {
        location,
        boundary: fileProperty?.property?.boundary ?? null,
        isActive: fileProperty.isActive,
      }
    : null;
}

export function propertyToLocationBoundaryDataset(
  property: ApiGen_Concepts_Property | undefined | null,
): LocationBoundaryDataset | null {
  const location = getLatLng(property.location);
  return exists(location)
    ? {
        location,
        boundary: property?.boundary ?? null,
      }
    : null;
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
  featureset: SelectedFeatureDataset,
): boolean {
  const location = point([latLng.lng, latLng.lat]);
  const boundary = (featureset?.pimsFeature?.geometry ?? featureset?.parcelFeature?.geometry) as
    | Polygon
    | MultiPolygon;

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

export const areSelectedFeaturesEqual = (
  lhs: SelectedFeatureDataset,
  rhs: SelectedFeatureDataset,
) => {
  const lhsName = getPropertyName(featuresetToMapProperty(lhs));
  const rhsName = getPropertyName(featuresetToMapProperty(rhs));
  if (
    (lhsName.label === rhsName.label &&
      lhsName.label !== NameSourceType.NONE &&
      lhsName.label !== NameSourceType.PLAN) ||
    (lhsName.label === NameSourceType.PLAN &&
      lhs.location.lat === rhs.location.lat &&
      lhs.location.lng === rhs.location.lng)
  ) {
    return lhsName.value === rhsName.value;
  }
  return false;
};

export const arePropertyFormsEqual = (lhs: PropertyForm, rhs: PropertyForm): boolean => {
  return areSelectedFeaturesEqual(lhs.toFeatureDataset(), rhs.toFeatureDataset());
};

export interface RegionDistrictResult {
  regionResult: Feature<Geometry, MOT_RegionalBoundary_Feature_Properties>;
  districtResult: Feature<Geometry, MOT_DistrictBoundary_Feature_Properties>;
}

export function featureSetToLatLngKey(featureSet: SelectedFeatureDataset | null | undefined) {
  if (exists(featureSet.location)) {
    const latLng: LatLngLiteral = {
      lat: featureSet.location.lat,
      lng: featureSet.location.lng,
    };

    const key = `${latLng.lat}-${latLng.lng}`;

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
  properties: SelectedFeatureDataset[],
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

    const latLng: LatLngLiteral = {
      lat: property.location.lat,
      lng: property.location.lng,
    };

    const key = featureSetToLatLngKey(property);

    if (!latLngMap.has(key)) {
      latLngMap.set(key, latLng);
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
