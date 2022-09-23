import { AxiosResponse } from 'axios';
import { useApiAcquisitionFile } from 'hooks/pims-api/useApiAcquisitionFile';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the Acquisition File API.
 */
export const useAcquisitionProvider = () => {
  const {
    getAcquisitionFile,
    postAcquisitionFile,
    putAcquisitionFileProperties,
  } = useApiAcquisitionFile();

  const addAcquisitionFileApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile) => await postAcquisitionFile(acqFile),
      [postAcquisitionFile],
    ),
    requestName: 'AddAcquisitionFile',
    onSuccess: useAxiosSuccessHandler('Acquisition File saved'),
    onError: useAxiosErrorHandler(),
  });

  const getAcquisitionFileApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(async (acqFileId: number) => await getAcquisitionFile(acqFileId), [
      getAcquisitionFile,
    ]),
    requestName: 'RetrieveAcquisitionFile',
    onSuccess: useAxiosSuccessHandler('Acquisition File retrieved'),
    onError: useAxiosErrorHandler(),
  });

  const updateAcquisitionPropertiesApi = useApiRequestWrapper<
    (acqFile: Api_AcquisitionFile) => Promise<AxiosResponse<Api_AcquisitionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_AcquisitionFile) => await putAcquisitionFileProperties(acqFile),
      [putAcquisitionFileProperties],
    ),
    requestName: 'UpdateAcquisitionFileProperties',
    onSuccess: useAxiosSuccessHandler('Acquisition File Properties updated'),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      addAcquisitionFile: addAcquisitionFileApi,
      getAcquisitionFile: getAcquisitionFileApi,
      updateAcquisitionFile: updateAcquisitionPropertiesApi,
    }),
    [addAcquisitionFileApi, getAcquisitionFileApi, updateAcquisitionPropertiesApi],
  );
};
