import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { Api_LeaseTerm } from '@/models/api/LeaseTerm';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
  deleteLeaseTerm,
  getLeaseTerms,
  postLeaseTerm,
  putLeaseTerm,
} from '../pims-api/useApiLeaseTerms';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';
import useDeepCompareMemo from '../util/useDeepCompareMemo';

/**
 * hook that interacts with the lease term API.
 */
export const useLeaseTermRepository = () => {
  const getLeaseTermsApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<Api_LeaseTerm[], any>>
  >({
    requestFunction: useCallback(async (leaseId: number) => await getLeaseTerms(leaseId), []),
    requestName: 'getLeaseTerms',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateLeaseTermApi = useApiRequestWrapper<
    (term: Api_LeaseTerm) => Promise<AxiosResponse<Api_LeaseTerm, any>>
  >({
    requestFunction: useCallback(async (term: Api_LeaseTerm) => await putLeaseTerm(term), []),
    requestName: 'putLeaseTerm',
    onSuccess: useAxiosSuccessHandler('Term saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const addLeaseTermApi = useApiRequestWrapper<
    (term: Api_LeaseTerm) => Promise<AxiosResponse<Api_LeaseTerm, any>>
  >({
    requestFunction: useCallback(async (term: Api_LeaseTerm) => await postLeaseTerm(term), []),
    requestName: 'postLeaseTerm',
    onSuccess: useAxiosSuccessHandler('Term saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const deleteLeaseTermApi = useApiRequestWrapper<
    (term: Api_LeaseTerm) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(async (term: Api_LeaseTerm) => await deleteLeaseTerm(term), []),
    requestName: 'deleteLeaseTerm',
    onSuccess: useAxiosSuccessHandler('Term deleted successfully.'),
    onError: useAxiosErrorHandler(),
  });

  return useDeepCompareMemo(
    () => ({
      getLeaseTerms: getLeaseTermsApi,
      updateLeaseTerm: updateLeaseTermApi,
      addLeaseTerm: addLeaseTermApi,
      deleteLeaseTerm: deleteLeaseTermApi,
    }),
    [getLeaseTermsApi, updateLeaseTermApi, addLeaseTermApi, deleteLeaseTermApi],
  );
};
