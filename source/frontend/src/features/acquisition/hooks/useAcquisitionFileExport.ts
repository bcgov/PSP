import axios from 'axios';
import fileDownload from 'js-file-download';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import {
  IPaginateAcquisition,
  useApiAcquisitionFile,
} from '@/hooks/pims-api/useApiAcquisitionFile';
import { logError, logRequest, logSuccess } from '@/store/slices/network/networkSlice';

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
        if (status === 204) {
          toast.warn(
            "We were unable to retrieve any data for your request. If you've applied any filters or search criteria, ensure they are set correctly. Broadening your criteria may yield results.",
          );
        } else {
          dispatch(logSuccess({ name: requestId, status }));
          fileDownload(data, fileName);
          dispatch(hideLoading());
        }
        // trigger file download in client browser
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          dispatch(
            logError({
              name: 'GetAquisitionListExport',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      }
    },
    [dispatch, apiExportAcquisitionFiles],
  );

  return { exportAcquisitionFiles };
};
