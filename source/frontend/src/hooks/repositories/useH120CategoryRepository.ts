import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { useApiRequestWrapper } from '@/hooks/pims-api/useApiRequestWrapper';
import { Api_H120Category } from '@/models/api/H120Category';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { getH120Categories as getH120CategoriesApi } from './../pims-api/useApiH120Category';

/**
 * hook that interacts with the Compensation API.
 */
export const useH120CategoryRepository = () => {
  const getH120Categories = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_H120Category[], any>>
  >({
    requestFunction: useCallback(async () => await getH120CategoriesApi(), []),
    requestName: 'getH120Categories',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load H120 Categories.'),
  });
  return getH120Categories;
};
