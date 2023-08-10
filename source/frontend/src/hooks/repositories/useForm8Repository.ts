import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_ExpropriationPayment } from '@/models/api/Form8';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { getForm8Api, putForm8Api } from '../pims-api/useApiForm8';

export const useForm8Repository = () => {
  const getForm8 = useApiRequestWrapper<(id: number) => Promise<AxiosResponse<Api_ExpropriationPayment, any>>>({
    requestFunction: useCallback(async (id: number) => await getForm8Api(id), []),
    requestName: 'getForm8',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load Form 8.'),
  });

  const updateForm8 = useApiRequestWrapper<
    (form8: Api_ExpropriationPayment) => Promise<AxiosResponse<Api_ExpropriationPayment, any>>
  >({
    requestFunction: useCallback(async (form8: Api_ExpropriationPayment) => await putForm8Api(form8), []),
    requestName: 'UpdateForm8',
    throwError: true,
  });

  return useMemo(
    () => ({
      getForm8: getForm8,
      updateForm8: updateForm8,
    }),
    [getForm8, updateForm8],
  );
};
