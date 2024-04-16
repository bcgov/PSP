import axios from 'axios';
import fileDownload from 'js-file-download';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import {
  IPaginateAcquisition,
  useApiAcquisitionFile,
} from '@/hooks/pims-api/useApiAcquisitionFile';
import { useModalContext } from '@/hooks/useModalContext';
import { logError, logRequest, logSuccess } from '@/store/slices/network/networkSlice';

export const useAcquisitionFileExport = () => {
  const { setModalContent, setDisplayModal } = useModalContext();
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
        const { data, status } = await apiExportAcquisitionFiles(filter);
        if (status === 204) {
          setModalContent({
            variant: 'warning',
            title: 'Warning',
            message: 'There is no data for the input parameters you entered.',
            okButtonText: 'Close',
          });
          setDisplayModal(true);
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
              name: 'GetAcquisitionListExport',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      }
    },
    [dispatch, apiExportAcquisitionFiles, setModalContent, setDisplayModal],
  );

  return { exportAcquisitionFiles };
};
