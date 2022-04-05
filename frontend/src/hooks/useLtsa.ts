import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { toast } from 'react-toastify';
import { handleAxiosResponse } from 'utils';

import { useApiLtsa } from './pims-api/useApiLtsa';

/**
 * hook retrieves data from ltsa
 */
export const useLtsa = () => {
  const { getLtsaOrders } = useApiLtsa();
  const dispatch = useDispatch();

  const getLtsaData = useCallback(
    async (pid: string) => {
      try {
        const response = await handleAxiosResponse(dispatch, 'getLtsaData', getLtsaOrders(pid));
        return response;
      } catch (axiosError) {
        toast.error(
          `Failed to get LTSA data. error from LTSA: ${axiosError?.response?.data.error}`,
        );
      }
    },
    [dispatch, getLtsaOrders],
  );

  return { getLtsaData };
};
