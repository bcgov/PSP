import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { useWfsLayer } from '@/hooks/layer-api/useWfsLayer';
import { wfsAxios2 } from '@/hooks/layer-api/wfsAxios';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { useTenant } from '@/tenants';
import { isValidString } from '@/utils';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the Fully Attributed ParcelMapBC layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric-fully-attributed/resource/59d9964f-bc93-496f-8039-b83ab8f24a41
 */

export const useFullyAttributedParcelMapLayer = () => {
  const {
    parcelMapFullyAttributed,
    fullyAttributedParcelsLayerUrl,
    internalFullyAttributedParcelsLayerUrl,
  } = useTenant();

  const getAllFeaturesWrapper = useWfsLayer(parcelMapFullyAttributed.url, {
    name: parcelMapFullyAttributed.name,
    withCredentials: true,
  });

  const { findOneWhereContains, findMultipleWhereContainsWrapped } = useLayerQuery(
    fullyAttributedParcelsLayerUrl,
    false,
    true,
  );

  const { execute: getAllFeatures, loading: getAllFeaturesLoading } = getAllFeaturesWrapper;

  const handleError = useCallback(() => {
    toast.error('Unable to contact Parcel Map');
  }, []);

  const findByLegalDescription = useCallback(
    async (legalDesc: string) => {
      const data = await getAllFeatures({ LEGAL_DESCRIPTION: legalDesc }, { timeout: 40000 });

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      return data as
        | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
        | undefined;
    },
    [getAllFeatures],
  );

  const findByPid = useCallback(
    async (pid: string, forceExactMatch = false) => {
      // Removes dashes to match expectations of the map layer.
      const formattedPid = pid.replace(/[-\s]/g, '');
      try {
        const data = await getAllFeatures(
          { PID: formattedPid },
          { forceExactMatch: forceExactMatch, timeout: 30000 },
        );
        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        return data as
          | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
          | undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [getAllFeatures, handleError],
  );

  const findByPin = useCallback(
    async (pin: string, forceExactMatch = false) => {
      try {
        const data = await getAllFeatures(
          { PIN: pin },
          { forceExactMatch: forceExactMatch, timeout: 30000 },
        );
        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        return data as
          | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
          | undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [getAllFeatures, handleError],
  );

  const findByPlanNumber = useCallback(
    async (planNumber: string, forceExactMatch = false) => {
      try {
        const data = await getAllFeatures(
          { PLAN_NUMBER: planNumber },
          { forceExactMatch: forceExactMatch, timeout: 30000 },
        );
        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        return data as
          | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
          | undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [getAllFeatures, handleError],
  );

  const findOne = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      try {
        const featureCollection = await findOneWhereContains(
          latlng,
          geometryName,
          spatialReferenceId,
        );

        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        const forceCasted = featureCollection as
          | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
          | undefined;
        return forceCasted !== undefined && forceCasted.features.length > 0
          ? forceCasted.features[0]
          : undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [findOneWhereContains, handleError],
  );

  const findMany = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      try {
        const featureCollection = await findMultipleWhereContainsWrapped.execute(
          latlng,
          geometryName,
          spatialReferenceId,
        );

        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        return featureCollection as
          | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
          | undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [findMultipleWhereContainsWrapped, handleError],
  );

  const findBySectionTownshipRange = useCallback(
    async (
      section?: number | string,
      township?: number | string,
      range?: number | string,
    ): Promise<FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>> => {
      let sectionQuery = '';
      let townshipQuery = '';
      let rangeQuery = '';
      if (isValidString(section?.toString())) {
        sectionQuery = `(LEGAL_DESCRIPTION ilike '%SECTION ${section}%' OR (LEGAL_DESCRIPTION ilike '%SECTIONS%' AND LEGAL_DESCRIPTION ilike '%${section}%'))`;
      }
      if (isValidString(township?.toString())) {
        townshipQuery = `LEGAL_DESCRIPTION ilike '%TOWNSHIP ${township}%'`;
      }
      if (isValidString(range?.toString())) {
        rangeQuery = `LEGAL_DESCRIPTION ilike '%TOWNSHIP ${range}%'`;
      }

      debugger;
      const query = [sectionQuery, townshipQuery, rangeQuery]
        .filter(x => isValidString(x))
        .join(' AND ');

      const searchParams = new URLSearchParams();

      searchParams.set('request', 'GetFeature');
      if (isValidString(query)) {
        searchParams.set('cql_filter', query);
      }

      const response = await wfsAxios2({ authenticated: true }).get<
        FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
      >(`${internalFullyAttributedParcelsLayerUrl}&${searchParams.toString()}`);
      return response?.data;
    },
    [internalFullyAttributedParcelsLayerUrl],
  );

  return {
    findByLegalDescription,
    findByPid,
    findByPin,
    findByPlanNumber,
    findByLoading: getAllFeaturesLoading,
    findByWrapper: getAllFeaturesWrapper,
    findOne,
    findMany,
    findManyLoading: findMultipleWhereContainsWrapped.loading,
    findBySectionTownshipRange,
  };
};
