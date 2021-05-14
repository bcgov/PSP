import { useDispatch } from 'react-redux';
import { savePropertyNames } from 'features/properties/common/slices/propertyNameSlice';
import { handleAxiosResponse } from 'utils';
import { STORE_PROPERTY_NAMES } from '../../../../constants/actionTypes';
import CustomAxios from 'customAxios';
import { ENVIRONMENT } from 'constants/environment';
import queryString from 'query-string';
import { IGeoSearchParams } from 'constants/API';
import { useCallback } from 'react';

const getPropertyNames = (filter: IGeoSearchParams) =>
  `/properties/search/names?${filter ? queryString.stringify(filter) : ''}`;

export const usePropertyNames = () => {
  const dispatch = useDispatch();
  const fetchPropertyNames = useCallback(
    async (agencyId: number): Promise<string[]> => {
      const axiosResponse = CustomAxios()
        .get(ENVIRONMENT.apiUrl + getPropertyNames({ agencies: agencyId?.toString() }))
        .then(response => dispatch(savePropertyNames(response.data)));
      return handleAxiosResponse(dispatch, STORE_PROPERTY_NAMES, axiosResponse);
    },
    [dispatch],
  );
  return { fetchPropertyNames };
};
