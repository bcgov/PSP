import { FeatureCollection, Geometry } from 'geojson';
import { useCallback } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
  TANTALIS_CrownSurveyParcels_Feature_Properties,
} from '@/models/layers/crownLand';
import { useTenant } from '@/tenants';
import { isValidString } from '@/utils/utils';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the set of Crown Land related layers.
 * @returns Object containing functions to make requests to the WFS layer.
 */

export const useCrownLandLayer = () => {
  const {
    crownLandLeasesUrl,
    crownLandLicensesUrl,
    crownLandTenuresUrl,
    crownLandInventoryUrl,
    crownLandInclusionsUrl,
    crownLandSurveyedParcelsUrl,
  } = useTenant();

  const {
    findMultipleWhereContainsBoundaryWrapped: {
      execute: findMultipleWhereContainsCrownLandLeasesExecute,
      loading: findMultipleWhereContainsCrownLandLeasesLoading,
    },
  } = useLayerQuery(crownLandLeasesUrl);

  const findMultipleCrownLandLease = useCallback(
    async (
      boundary: Geometry,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandLeasesExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandLeases_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsCrownLandLeasesExecute],
  );

  const {
    findMultipleWhereContainsBoundaryWrapped: {
      execute: findMultipleWhereContainsCrownLandLicensesExecute,
      loading: findMultipleWhereContainsCrownLandLicensesLoading,
    },
  } = useLayerQuery(crownLandLicensesUrl);

  const findMultipleCrownLandLicense = useCallback(
    async (
      boundary: Geometry,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandLicensesExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandLicenses_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsCrownLandLicensesExecute],
  );

  const {
    findMultipleWhereContainsBoundaryWrapped: {
      execute: findMultipleWhereContainsCrownLandTenuresExecute,
      loading: findMultipleWhereContainsCrownLandTenuresLoading,
    },
  } = useLayerQuery(crownLandTenuresUrl);

  const findMultipleCrownLandTenure = useCallback(
    async (
      boundary: Geometry,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandTenuresExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandTenures_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsCrownLandTenuresExecute],
  );

  const {
    findMultipleWhereContainsBoundaryWrapped: {
      execute: findMultipleWhereContainsCrownLandInventoryExecute,
      loading: findMultipleWhereContainsCrownLandInventoryLoading,
    },
  } = useLayerQuery(crownLandInventoryUrl);

  const findMultipleCrownLandInventory = useCallback(
    async (
      boundary: Geometry,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandInventoryExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandInventory_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsCrownLandInventoryExecute],
  );

  const {
    findMultipleWhereContainsBoundaryWrapped: {
      execute: findMultipleWhereContainsCrownLandInclusionsExecute,
      loading: findMultipleWhereContainsCrownLandInclusionsLoading,
    },
  } = useLayerQuery(crownLandInclusionsUrl);

  const findMultipleCrownLandInclusion = useCallback(
    async (
      boundary: Geometry,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandInclusionsExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandInclusions_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsCrownLandInclusionsExecute],
  );

  const {
    findMultipleRawWrapped: {
      execute: findMultipleWhereContainsCrownLandSurveyedExecute,
      loading: findMultipleWhereContainsCrownLandSurveyedLoading,
    },
  } = useLayerQuery(crownLandSurveyedParcelsUrl);

  const findMultipleSectionTownshipRange = useCallback(
    async (
      section?: number | string,
      township?: number | string,
      range?: number | string,
      district?: string,
    ): Promise<FeatureCollection<Geometry, TANTALIS_CrownSurveyParcels_Feature_Properties>> => {
      let sectionQuery = '';
      let townshipQuery = '';
      let rangeQuery = '';
      let districtQuery = '';
      if (isValidString(section?.toString())) {
        if (isValidString(range?.toString())) {
          sectionQuery = `(PARCEL_LEGAL_DESCRIPTION ilike '%SECTION ${section},%' OR (PARCEL_LEGAL_DESCRIPTION ilike '%SECTIONS%' AND PARCEL_LEGAL_DESCRIPTION ilike '%${section},%'))`;
        } else {
          sectionQuery = `PARCEL_LEGAL_DESCRIPTION ilike '%SECTION ${section},%'`;
        }
      }
      if (isValidString(township?.toString())) {
        townshipQuery = `PARCEL_LEGAL_DESCRIPTION ilike '%TOWNSHIP ${township},%'`;
      }
      if (isValidString(range?.toString())) {
        rangeQuery = `PARCEL_LEGAL_DESCRIPTION ilike '%RANGE ${range},%'`;
      }
      if (isValidString(district)) {
        const districtSearchString = district.replace('DISTRICT', 'DIST');
        districtQuery = `PARCEL_LEGAL_DESCRIPTION ilike '%${districtSearchString}%'`;
      }

      const query = [sectionQuery, townshipQuery, rangeQuery, districtQuery]
        .filter(x => isValidString(x))
        .join(' AND ');

      const searchParams = new URLSearchParams();

      searchParams.set('request', 'GetFeature');
      if (isValidString(query)) {
        searchParams.set('cql_filter', query);
      }

      const featureCollection = await findMultipleWhereContainsCrownLandSurveyedExecute(
        searchParams,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownSurveyParcels_Feature_Properties
      >;

      return forceCasted;
    },
    [findMultipleWhereContainsCrownLandSurveyedExecute],
  );

  return {
    findMultipleCrownLandLease,
    findMultipleCrownLandLeaseLoading: findMultipleWhereContainsCrownLandLeasesLoading,
    findMultipleCrownLandLicense: findMultipleCrownLandLicense,
    findMultipleCrownLandLicenseLoading: findMultipleWhereContainsCrownLandLicensesLoading,
    findMultipleCrownLandTenure,
    findMultipleCrownLandTenureLoading: findMultipleWhereContainsCrownLandTenuresLoading,
    findMultipleCrownLandInventory: findMultipleCrownLandInventory,
    findMultipleCrownLandInventoryLoading: findMultipleWhereContainsCrownLandInventoryLoading,
    findMultipleCrownLandInclusion: findMultipleCrownLandInclusion,
    findMultipleCrownLandInclusionsLoading: findMultipleWhereContainsCrownLandInclusionsLoading,
    findMultipleSurveyParcel: findMultipleSectionTownshipRange,
    findMultipleWhereContainsCrownLandSurveyedLoading:
      findMultipleWhereContainsCrownLandSurveyedLoading,
  };
};
