import React from 'react';
import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';

/**
 * Common hook to make requests to the PIMS APi.
 * @returns CustomAxios object setup for the PIMS API.
 */
export const useApi = () => {
  return React.useMemo(() => CustomAxios({ baseURL: ENVIRONMENT.apiUrl }), []);
};

export default useApi;
