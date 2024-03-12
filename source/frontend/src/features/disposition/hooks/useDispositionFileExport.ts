import axios from 'axios';
import fileDownload from 'js-file-download';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import {
  IPaginateDisposition,
  useApiDispositionFile,
} from '@/hooks/pims-api/useApiDispositionFile';
import { useModalContext } from '@/hooks/useModalContext';
import { logError, logRequest, logSuccess } from '@/store/slices/network/networkSlice';

export const useDispositionFileExport = () => {
  const { setModalContent, setDisplayModal } = useModalContext();
  const { exportDispositionFiles: apiExportDispositionFiles } = useApiDispositionFile();
  const dispatch = useDispatch();

  const exportDispositionFiles = useCallback(
    async (
      filter: IPaginateDisposition,
      outputFormat: 'excel',
      fileName = `Disposition_File_Export.xlsx`,
      requestId = 'properties-report',
    ) => {
      dispatch(logRequest(requestId));
      dispatch(showLoading());
      try {
        const { data, status } = await apiExportDispositionFiles(filter);
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
              name: 'GetDispositionListExport',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      }
    },
    [dispatch, apiExportDispositionFiles, setModalContent, setDisplayModal],
  );

  return { exportDispositionFiles };
};
