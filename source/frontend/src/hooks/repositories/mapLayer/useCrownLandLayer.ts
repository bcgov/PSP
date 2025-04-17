import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
} from '@/models/layers/crownLand';
import { useTenant } from '@/tenants';

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
  } = useTenant();

  const {
    findMultipleWhereContainsWrapped: {
      execute: findMultipleWhereContainsCrownLandLeasesExecute,
      loading: findMultipleWhereContainsCrownLandLeasesLoading,
    },
  } = useLayerQuery(crownLandLeasesUrl);

  const findMultipleCrownLandLease = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandLeasesExecute(
        latlng,
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
    findMultipleWhereContainsWrapped: {
      execute: findMultipleWhereContainsCrownLandLicensesExecute,
      loading: findMultipleWhereContainsCrownLandLicensesLoading,
    },
  } = useLayerQuery(crownLandLicensesUrl);

  const findMultipleCrownLandLicense = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandLicensesExecute(
        latlng,
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
    findMultipleWhereContainsWrapped: {
      execute: findMultipleWhereContainsCrownLandTenuresExecute,
      loading: findMultipleWhereContainsCrownLandTenuresLoading,
    },
  } = useLayerQuery(crownLandTenuresUrl);

  const findMultipleCrownLandTenure = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandTenuresExecute(
        latlng,
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
    findMultipleWhereContainsWrapped: {
      execute: findMultipleWhereContainsCrownLandInventoryExecute,
      loading: findMultipleWhereContainsCrownLandInventoryLoading,
    },
  } = useLayerQuery(crownLandInventoryUrl);

  const findMultipleCrownLandInventory = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandInventoryExecute(
        latlng,
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
    findMultipleWhereContainsWrapped: {
      execute: findMultipleWhereContainsCrownLandInclusionsExecute,
      loading: findMultipleWhereContainsCrownLandInclusionsLoading,
    },
  } = useLayerQuery(crownLandInclusionsUrl);

  const findMultipleCrownLandInclusion = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsCrownLandInclusionsExecute(
        latlng,
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
  };
};
