import { useCallback } from 'react';

import { useWfsLayer } from './useWfsLayer';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the Fully Attributed ParcelMapBC layer.
 * @returns Object containing functions to make requests to the WFS layer.
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

  return { findByLegalDescription, findByLegalDescriptionLoading: getAllFeaturesLoading };
};
