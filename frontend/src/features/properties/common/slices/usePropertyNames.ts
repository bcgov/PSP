import { IGeoSearchParams } from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { savePropertyNames } from 'features/properties/common/slices/propertyNameSlice';
import queryString from 'query-string';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { handleAxiosResponse } from 'utils';

import { STORE_PROPERTY_NAMES } from '../../../../constants/actionTypes';

const getPropertyNames = (filter: IGeoSearchParams) =>
  `/properties/search/names?${filter ? queryString.stringify(filter) : ''}`;

export const usePropertyNames = () => {
  const dispatch = useDispatch();
  const fetchPropertyNames = useCallback(async (): Promise<string[]> => {
    const axiosResponse = CustomAxios()
      .get<string[]>(ENVIRONMENT.apiUrl + getPropertyNames({}))
      .then(response => dispatch(savePropertyNames(response.data)));
    return handleAxiosResponse(dispatch, STORE_PROPERTY_NAMES, axiosResponse);
  }, [dispatch]);
  return { fetchPropertyNames };
};
