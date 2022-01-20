import axios from 'axios';
import * as actionTypes from 'constants/actionTypes';
import { catchAxiosError } from 'customAxios';
import { IPaginateLeases, useApiLeases } from 'hooks/pims-api/useApiLeases';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { logRequest, logSuccess } from 'store/slices/network/networkSlice';
import { downloadFile } from 'utils/download';

/**
 * hook that allows the user to export the currently filtered lease data.
 */
export const useLeaseExport = () => {
  const { exportLeases: apiExportLeases } = useApiLeases();
  const dispatch = useDispatch();

  const exportLeases = useCallback(
    async (
      filter: IPaginateLeases,
      outputFormat: 'csv' | 'excel' = 'excel',
      fileName = `pims-leases.${outputFormat === 'csv' ? 'csv' : 'xlsx'}`,
      requestId = 'properties-report',
    ) => {
      dispatch(logRequest(requestId));
      dispatch(showLoading());
      try {
        const { data, status } = await apiExportLeases(filter, outputFormat);
        dispatch(logSuccess({ name: requestId, status }));
        dispatch(hideLoading());
        // trigger file download in client browser
        downloadFile(fileName, data);
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, apiExportLeases],
  );

  return { exportLeases };
};
