import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiDispositionFile } from '@/hooks/pims-api/useApiDispositionFile';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileAppraisal } from '@/models/api/generated/ApiGen_Concepts_DispositionFileAppraisal';
import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';
import { ApiGen_Concepts_DispositionFileProperty } from '@/models/api/generated/ApiGen_Concepts_DispositionFileProperty';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';
import { ApiGen_Concepts_DispositionFileTeam } from '@/models/api/generated/ApiGen_Concepts_DispositionFileTeam';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
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
    putDispositionFileApi,
    getDispositionFileProperties,
    getLastUpdatedByApi,
    getDispositionFileChecklist,
    putDispositionFileChecklist,
    getAllDispositionFileTeamMembers,
    getDispositionFileAppraisal,
    postDispositionFileAppraisal,
    putDispositionFileAppraisal,
    getDispositionFileOffers,
    postDispositionFileOffer,
    getDispositionFileSale,
    postDispositionFileSale,
    putDispositionFileSale,
    getDispositionFileOffer,
    putDispositionFileOffer,
    deleteDispositionFileOffer,
  } = useApiDispositionFile();

  const addDispositionFileApi = useApiRequestWrapper<
    (
      dispositionFile: ApiGen_Concepts_DispositionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (
        dispositionFile: ApiGen_Concepts_DispositionFile,
        useOverride: UserOverrideCode[] = [],
      ) => await postDispositionFileApi(dispositionFile, useOverride),
      [postDispositionFileApi],
    ),
    requestName: 'AddDispositionFile',
    onSuccess: useAxiosSuccessHandler('Disposition File saved'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const getDispositionFileApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFile(dispositionFileId),
      [getDispositionFile],
    ),
    requestName: 'RetrieveDispositionFile',
    onError: useAxiosErrorHandlerWithAuthorization('Failed to load Disposition File'),
  });

  const updateDispositionFileApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      dispositionFile: ApiGen_Concepts_DispositionFile,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (
        dispositionFileId: number,
        dispositionFile: ApiGen_Concepts_DispositionFile,
        useOverride: UserOverrideCode[] = [],
      ) => await putDispositionFileApi(dispositionFileId, dispositionFile, useOverride),
      [putDispositionFileApi],
    ),
    requestName: 'UpdateDispositionFile',
    onSuccess: useAxiosSuccessHandler('Disposition File saved'),
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
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
    (
      dispositionFileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFileProperties(dispositionFileId),
      [getDispositionFileProperties],
    ),
    requestName: 'GetDispositionFileProperties',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Properties'),
  });

  const getDispositionChecklistApi = useApiRequestWrapper<
    (acqFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_FileChecklistItem[], any>>
  >({
    requestFunction: useCallback(
      async (acqFileId: number) => await getDispositionFileChecklist(acqFileId),
      [getDispositionFileChecklist],
    ),
    requestName: 'GetDispositionFileChecklist',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Checklist'),
  });

  const updateDispositionChecklistApi = useApiRequestWrapper<
    (
      acqFile: ApiGen_Concepts_FileWithChecklist,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFile, any>>
  >({
    requestFunction: useCallback(
      async (acqFile: ApiGen_Concepts_FileWithChecklist) =>
        await putDispositionFileChecklist(acqFile),
      [putDispositionFileChecklist],
    ),
    requestName: 'UpdateDispositionFileChecklist',
    onError: useAxiosErrorHandler('Failed to update Disposition File Checklist'),
    throwError: true,
  });

  const getAllDispositionTeamMembersApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileTeam[], any>>
  >({
    requestFunction: useCallback(
      async () => await getAllDispositionFileTeamMembers(),
      [getAllDispositionFileTeamMembers],
    ),
    requestName: 'GetAllDispositionTeamMembers',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Team Members'),
  });

  const getDispositionAppraisalApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileAppraisal, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFileAppraisal(dispositionFileId),
      [getDispositionFileAppraisal],
    ),
    requestName: 'GetDispositionAppraisal',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Appraisal'),
  });

  const postDispositionAppraisalApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      dispositionAppraisal: ApiGen_Concepts_DispositionFileAppraisal,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileAppraisal, any>>
  >({
    requestFunction: useCallback(
      async (
        dispositionFileId: number,
        dispositionAppraisal: ApiGen_Concepts_DispositionFileAppraisal,
      ) => await postDispositionFileAppraisal(dispositionFileId, dispositionAppraisal),
      [postDispositionFileAppraisal],
    ),
    requestName: 'PostDispositionAppraisal',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const putDispositionAppraisalApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      appraisalId: number,
      dispositionAppraisal: ApiGen_Concepts_DispositionFileAppraisal,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileAppraisal, any>>
  >({
    requestFunction: useCallback(
      async (
        dispositionFileId: number,
        appraisalId: number,
        dispositionAppraisal: ApiGen_Concepts_DispositionFileAppraisal,
      ) => await putDispositionFileAppraisal(dispositionFileId, appraisalId, dispositionAppraisal),
      [putDispositionFileAppraisal],
    ),
    requestName: 'PutDispositionAppraisal',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const getAllDispositionOffersApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileOffer[], any>>
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
      dispositionOffer: ApiGen_Concepts_DispositionFileOffer,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileOffer, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number, dispositionOffer: ApiGen_Concepts_DispositionFileOffer) =>
        await postDispositionFileOffer(dispositionFileId, dispositionOffer),
      [postDispositionFileOffer],
    ),
    requestName: 'PostDispositionOffer',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const getDispositionFileSaleApi = useApiRequestWrapper<
    (dispositionFileId: number) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileSale, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number) => await getDispositionFileSale(dispositionFileId),
      [getDispositionFileSale],
    ),
    requestName: 'GetAllDispositionSales',
    onError: useAxiosErrorHandler('Failed to retrieve Disposition File Sale'),
  });

  const postDispositionSaleApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      dispositionSale: ApiGen_Concepts_DispositionFileSale,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileSale, any>>
  >({
    requestFunction: useCallback(
      async (dispositionFileId: number, dispositionSale: ApiGen_Concepts_DispositionFileSale) =>
        await postDispositionFileSale(dispositionFileId, dispositionSale),
      [postDispositionFileSale],
    ),
    requestName: 'PostDispositionSale',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const putDispositionSaleApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      saleId: number,
      dispositionSale: ApiGen_Concepts_DispositionFileSale,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileSale, any>>
  >({
    requestFunction: useCallback(
      async (
        dispositionFileId: number,
        saleId: number,
        dispositionSale: ApiGen_Concepts_DispositionFileSale,
      ) => await putDispositionFileSale(dispositionFileId, saleId, dispositionSale),
      [putDispositionFileSale],
    ),
    requestName: 'PutDispositionSale',
    onError: useAxiosErrorHandler('Failed to udpate Disposition File Sale'),
  });

  const getDispositionOfferApi = useApiRequestWrapper<
    (
      dispositionFileId: number,
      offerId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileOffer, any>>
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
      dispositionOffer: ApiGen_Concepts_DispositionFileOffer,
    ) => Promise<AxiosResponse<ApiGen_Concepts_DispositionFileOffer, any>>
  >({
    requestFunction: useCallback(
      async (
        dispositionFileId: number,
        offerId: number,
        dispositionOffer: ApiGen_Concepts_DispositionFileOffer,
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
      putDispositionFile: updateDispositionFileApi,
      getLastUpdatedBy,
      getDispositionProperties: getDispositionPropertiesApi,
      getDispositionChecklist: getDispositionChecklistApi,
      putDispositionChecklist: updateDispositionChecklistApi,
      getAllDispositionTeamMembers: getAllDispositionTeamMembersApi,
      getDispositionAppraisal: getDispositionAppraisalApi,
      postDispositionAppraisal: postDispositionAppraisalApi,
      putDispositionAppraisal: putDispositionAppraisalApi,
      getDispositionFileOffers: getAllDispositionOffersApi,
      postDispositionFileOffer: postDispositionOfferApi,
      getDispositionFileSale: getDispositionFileSaleApi,
      postDispositionFileSale: postDispositionSaleApi,
      putDispositionFileSale: putDispositionSaleApi,
      getDispositionOffer: getDispositionOfferApi,
      putDispositionOffer: putDispositionOfferApi,
      deleteDispositionOffer: deleteDispositionOfferApi,
    }),
    [
      addDispositionFileApi,
      getDispositionFileApi,
      updateDispositionFileApi,
      getLastUpdatedBy,
      getDispositionPropertiesApi,
      getDispositionChecklistApi,
      updateDispositionChecklistApi,
      getAllDispositionTeamMembersApi,
      getDispositionAppraisalApi,
      postDispositionAppraisalApi,
      putDispositionAppraisalApi,
      getAllDispositionOffersApi,
      postDispositionOfferApi,
      getDispositionFileSaleApi,
      postDispositionSaleApi,
      putDispositionSaleApi,
      getDispositionOfferApi,
      putDispositionOfferApi,
      deleteDispositionOfferApi,
    ],
  );
};
