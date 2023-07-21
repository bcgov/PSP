import axios from 'axios';
import fileDownload from 'js-file-download';
import moment from 'moment';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import * as actionTypes from '@/constants/actionTypes';
import { catchAxiosError } from '@/customAxios';
import { IPaginateLeases, useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { logRequest, logSuccess } from '@/store/slices/network/networkSlice';

/**
 * hook that allows the user to export the currently filtered lease data.
 */
export const useLeaseExport = () => {
  const {
    exportLeases: apiExportLeases,
    exportAggregatedLeases: apiExportAggregatedLeases,
    exportLeasePayments: apiExportLeasePayments,
  } = useApiLeases();
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
        fileDownload(data, fileName);
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, apiExportLeases],
  );

  const exportAggregatedLeases = useCallback(
    async (fiscalYearStart: number, requestId = 'leases-aggregated-report') => {
      dispatch(logRequest(requestId));
      dispatch(showLoading());
      try {
        const { data, status } = await apiExportAggregatedLeases(fiscalYearStart);
        dispatch(logSuccess({ name: requestId, status }));
        dispatch(hideLoading());
        // trigger file download in client browser
        fileDownload(data, `pims-aggregated-leases-${fiscalYearStart}-${fiscalYearStart + 1}.xlsx`);
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, apiExportAggregatedLeases],
  );

  const exportLeasePayments = useCallback(
    async (fiscalYearStart: number, requestId = 'lease-payments-report') => {
      dispatch(logRequest(requestId));
      dispatch(showLoading());
      try {
        const { data, status } = await apiExportLeasePayments(fiscalYearStart);
        dispatch(logSuccess({ name: requestId, status }));
        dispatch(hideLoading());
        // trigger file download in client browser
        fileDownload(
          data,
          `LeaseLicense_Payment details-${fiscalYearStart}-${fiscalYearStart + 1}_${moment().format(
            'DD-MM-yyyy_hh-mm-ss',
          )}.xlsx`,
        );
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, apiExportLeasePayments],
  );

  return { exportLeases, exportAggregatedLeases, exportLeasePayments };
};
