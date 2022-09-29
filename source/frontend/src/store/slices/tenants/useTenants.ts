import { useApiTenants } from 'hooks/pims-api';
import React from 'react';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { useAppDispatch } from 'store/hooks';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { storeSettings, tenantsSlice } from '.';

/**
 * hook that wraps calls to the organizations api.
 */
export const useTenants = () => {
  const dispatch = useAppDispatch();
  const api = useApiTenants();

  /**
   * fetch all of the organizations from the server based on a filter.
   * @return the filtered, paged list of organizations.
   */
  const getSettings = React.useCallback(async () => {
    dispatch(logRequest(tenantsSlice.name));
    dispatch(showLoading());
    try {
      const response = await api.getSettings();

      dispatch(logSuccess({ name: tenantsSlice.name, status: response.status }));
      dispatch(storeSettings(response.data));
      return response;
    } catch (error) {
      dispatch(logError({ name: tenantsSlice.name, status: error?.response?.status, error }));
    } finally {
      dispatch(hideLoading());
    }
  }, [api, dispatch]);

  return {
    getSettings,
  };
};

export default useTenants;
