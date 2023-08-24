import axios, { AxiosError } from 'axios';
import fileDownload from 'js-file-download';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

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
      outputFormat: 'csv' | 'excel' = 'excel',
      fileName = `pims-acquisition-files.${outputFormat === 'csv' ? 'csv' : 'xlsx'}`,
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
          console.log(axiosError);
          if (axiosError?.response?.status === 409) {
            toast.error('Export contains no data');
          }
          dispatch(hideLoading());
          //catchAxiosError(e, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, apiExportAcquisitionFiles],
  );

  return { exportAcquisitionFiles };
};
