import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiDispositionFile } from '@/hooks/pims-api/useApiDispositionFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import {
  Api_DispositionFile,
  Api_DispositionFileOffer,
  Api_DispositionFileProperty,
  Api_DispositionFileSale,
  Api_DispositionFileTeam,
} from '@/models/api/DispositionFile';
import { Api_FileChecklistItem, Api_FileWithChecklist, Api_LastUpdatedBy } from '@/models/api/File';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import {
  useAxiosErrorHandler,
  useAxiosErrorHandlerWithAuthorization,
  useAxiosSuccessHandler,
} from '@/utils';

const ignoreErrorCodes = [409];

/**
 * hook that interacts with the Disposition File API.
 */
export const useDispositionProvider = () => {
  const {
    postDispositionFileApi,
    getDispositionFile,
    getDispositionFileProperties,
    getLastUpdatedByApi,
    getDispositionFileChecklist,
    putDispositionFileChecklist,
    getAllDispositionFileTeamMembers,
    getDispositionFileOffers,
    postDispositionFileOffer,
    getDispositionFileSale,
    getDispositionFileOffer,
    putDispositionFileOffer,
    deleteDispositionFileOffer,
  } = useApiDispositionFile();

  const addDispositionFileApi = useApiRequestWrapper<
    (
      dispositionFile: Api_DispositionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<Api_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFile: Api_DispositionFile, useOverride: UserOverrideCode[] = []) =>
        await postDispositionFileApi(dispositionFile, useOverride),
      [postDispositionFileApi],
    ),
    requestName: 'AddDispositionFile',
    onSuccess: useAxiosSuccessHandler('Disposition File saved'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const getDispositionFileApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFile(dispositionFileId),
      [getDispositionFile],
    ),
    requestName: 'RetrieveDispositionFile',
    onError: useAxiosErrorHandlerWithAuthorization('Failed to load Disposition File'),
  });

  const getLastUpdatedBy = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_LastUpdatedBy, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getLastUpdatedByApi(dispositionFileId),
      [getLastUpdatedByApi],
    ),
    requestName: 'getLastUpdatedBy',
    onError: useAxiosErrorHandler('Failed to load Disposition File last-updated-by'),
  });

  const getDispositionPropertiesApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_DispositionFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFileProperties(dispositionFileId),
      [getDispositionFileProperties],
    ),
    requestName: 'GetDispositionFileProperties',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Properties'),
  });

  const getDispositionChecklistApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<Api_FileChecklistItem[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getDispositionFileChecklist(acqFileId),
      [getDispositionFileChecklist],
    ),
    requestName: 'GetDispositionFileChecklist',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Checklist'),
  });

  const updateDispositionChecklistApi = useApiRequestWrapper<
    (acqFile: Api_FileWithChecklist) => Promise<AxiosResponse<Api_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: Api_FileWithChecklist) => await putDispositionFileChecklist(acqFile),
      [putDispositionFileChecklist],
    ),
    requestName: 'UpdateDispositionFileChecklist',
    onError: useAxiosErrorHandler('Failed to update Disposition File Checklist'),
    throwError: true,
  });

  const getAllDispositionTeamMembersApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_DispositionFileTeam[], any>>
  >({
    requestFunction: useCallback(
      async () => await getAllDispositionFileTeamMembers(),
      [getAllDispositionFileTeamMembers],
    ),
    requestName: 'GetAllDispositionTeamMembers',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Team Members'),
  });

  const getAllDispositionOffersApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_DispositionFileOffer[], any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFileOffers(dispositionFileId),
      [getDispositionFileOffers],
    ),
    requestName: 'GetAllDispositionOffers',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Offers'),
  });

  const postDispositionOfferApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      dispositionOffer: Api_DispositionFileOffer,
    ) => Promise<AxiosResponse<Api_DispositionFileOffer, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number, dispositionOffer: Api_DispositionFileOffer) =>
        await postDispositionFileOffer(dispositionFileId, dispositionOffer),
      [postDispositionFileOffer],
    ),
    requestName: 'PostDispositionOffer',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const getDispositionFileSaleApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<Api_DispositionFileSale, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFileSale(dispositionFileId),
      [getDispositionFileSale],
    ),
    requestName: 'GetAllDispositionSales',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Sale'),
  });

  const getDispositionOfferApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      offerId: number,
    ) => Promise<AxiosResponse<Api_DispositionFileOffer, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number, offerId: number) =>
        await getDispositionFileOffer(dispositionFileId, offerId),
      [getDispositionFileOffer],
    ),
    requestName: 'GetDispositionOffer',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Offer'),
  });

  const putDispositionOfferApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      offerId: number,
      dispositionOffer: Api_DispositionFileOffer,
    ) => Promise<AxiosResponse<Api_DispositionFileOffer, any>>
  >({
    requestFunction: useCallback(
      async (
        dispositionFileId: number,
        offerId: number,
        dispositionOffer: Api_DispositionFileOffer,
      ) => await putDispositionFileOffer(dispositionFileId, offerId, dispositionOffer),
      [putDispositionFileOffer],
    ),
    requestName: 'PutDispositionOffer',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const deleteDispositionOfferApi = useApiRequestWrapper<
    (dispositionFileId: number, offerId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number, offerId: number) =>
        await deleteDispositionFileOffer(dispositionFileId, offerId),
      [deleteDispositionFileOffer],
    ),
    requestName: 'DeleteDispositionOffer',
    onError: useAxiosErrorHandler('Failed to Delete Disposition File Offer'),
  });

  return useMemo(
    () => ({
      addDispositionFileApi: addDispositionFileApi,
      getDispositionFile: getDispositionFileApi,
      getLastUpdatedBy,
      getDispositionProperties: getDispositionPropertiesApi,
      getDispositionChecklist: getDispositionChecklistApi,
      putDispositionChecklist: updateDispositionChecklistApi,
      getAllDispositionTeamMembers: getAllDispositionTeamMembersApi,
      getDispositionFileOffers: getAllDispositionOffersApi,
      postDispositionFileOffer: postDispositionOfferApi,
      getDispositionFileSale: getDispositionFileSaleApi,
      getDispositionOffer: getDispositionOfferApi,
      putDispositionOffer: putDispositionOfferApi,
      deleteDispositionOffer: deleteDispositionOfferApi,
    }),
    [
      addDispositionFileApi,
      getDispositionFileApi,
      getLastUpdatedBy,
      getDispositionPropertiesApi,
      getDispositionChecklistApi,
      updateDispositionChecklistApi,
      getAllDispositionTeamMembersApi,
      getAllDispositionOffersApi,
      postDispositionOfferApi,
      getDispositionFileSaleApi,
      getDispositionOfferApi,
      putDispositionOfferApi,
      deleteDispositionOfferApi,
    ],
  );
};
