import axios, { AxiosError } from 'axios';
import fileDownload from 'js-file-download';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import * as actionTypes from '@/constants/actionTypes';
import { catchAxiosError } from '@/customAxios';
import {
  IPaginateAcquisition,
  useApiAcquisitionFile,
} from '@/hooks/pims-api/useApiAcquisitionFile';
import { IApiError } from '@/interfaces/IApiError';
import { logRequest, logSuccess } from '@/store/slices/network/networkSlice';

export const useAcquisitionFileExport = () => {
  const { exportAcquisitionFiles: apiExportAcquisitionFiles } = useApiAcquisitionFile();
  const dispatch = useDispatch();

  const exportAcquisitionFiles = useCallback(
    async (
      filter: IPaginateAcquisition,
      outputFormat: 'excel',
      fileName = `Acquisition_File_Export.xlsx`,
      requestId = 'properties-report',
    ) => {
      dispatch(logRequest(requestId));
      dispatch(showLoading());
      try {
        const { data, status } = await apiExportAcquisitionFiles(filter, outputFormat);
        dispatch(logSuccess({ name: requestId, status }));
        dispatch(hideLoading());
        // trigger file download in client browser
        fileDownload(data, fileName);
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError?.response?.status === 409) {
            toast.error('Export contains no data');
          } else {
            catchAxiosError(e, dispatch, actionTypes.DELETE_PARCEL);
          }
          dispatch(hideLoading());
        }
      }
    },
    [dispatch, apiExportAcquisitionFiles],
  );

  return { exportAcquisitionFiles };
};
