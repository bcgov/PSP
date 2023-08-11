import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { deleteForm8Api, getForm8Api, putForm8Api } from '../pims-api/useApiForm8';

export const useForm8Repository = () => {
  const getForm8 = useApiRequestWrapper<
    (id: number) => Promise<AxiosResponse<Api_ExpropriationPayment, any>>
  >({
    requestFunction: useCallback(async (id: number) => await getForm8Api(id), []),
    requestName: 'getForm8',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load Expropriation Payment.'),
  });

  const updateForm8 = useApiRequestWrapper<
    (form8: Api_ExpropriationPayment) => Promise<AxiosResponse<Api_ExpropriationPayment, any>>
  >({
    requestFunction: useCallback(
      async (form8: Api_ExpropriationPayment) => await putForm8Api(form8),
      [],
    ),
    requestName: 'UpdateForm8',
    throwError: true,
  });

  const deleteForm8 = useApiRequestWrapper<
    (form8Id: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(async (form8Id: number) => await deleteForm8Api(form8Id), []),
    throwError: true,
    requestName: 'DeleteForm8',
    onSuccess: useAxiosSuccessHandler('Expropriation Payment deleted'),
    onError: useAxiosErrorHandler(
      'Failed to delete Expropriation Payment. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      getForm8: getForm8,
      updateForm8: updateForm8,
      deleteForm8: deleteForm8,
    }),
    [deleteForm8, getForm8, updateForm8],
  );
};
