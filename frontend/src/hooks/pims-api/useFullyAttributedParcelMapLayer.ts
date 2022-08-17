import { useCallback } from 'react';

import { useWfsLayer } from './useWfsLayer';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the Fully Attributed ParcelMapBC layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric/resource/959af382-fb31-4f57-b8ea-e6dcb6ce2e0b
 */
export const useFullyAttributedParcelMapLayer = (url: string, name: string) => {
  const { getAllFeatures, getAllFeaturesLoading } = useWfsLayer(url, {
    name,
    withCredentials: true,
  });

  const findByLegalDescription = useCallback(
    async (legalDesc: string) => {
      const data = await getAllFeatures({ LEGAL_DESCRIPTION: legalDesc }, { timeout: 40000 });
      return data;
    },
    [getAllFeatures],
  );

  const findByPid = useCallback(
    async (pid: string) => {
      // Removes dashes to match expectations of the map layer.
      const formattedPid = pid.replace(/-/g, '');
      const data = await getAllFeatures(
        { PID: formattedPid },
        { forceSimplePid: true, timeout: 10000 },
      );

      return data;
    },
    [getAllFeatures],
  );

  const findByPin = useCallback(
    async (pin: string) => {
      const data = await getAllFeatures({ PIN: pin }, { timeout: 10000 });
      return data;
    },
    [getAllFeatures],
  );

  const findByPlanNumber = useCallback(
    async (planNumber: string) => {
      const data = await getAllFeatures({ PLAN_NUMBER: planNumber }, { timeout: 10000 });
      return data;
    },
    [getAllFeatures],
  );

  return {
    findByLegalDescription,
    findByPid,
    findByPin,
    findByPlanNumber,
    loadingIndicator: getAllFeaturesLoading,
  };
};
