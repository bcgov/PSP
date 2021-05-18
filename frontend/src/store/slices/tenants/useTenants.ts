import React from 'react';
import { showLoading, hideLoading } from 'react-redux-loading-bar';
import { useAppDispatch } from 'store/hooks';
import { useApiTenants } from 'hooks/pims-api';
import { tenantsSlice, storeSettings } from '.';
import { logRequest, logSuccess } from '../network/networkSlice';

/**
 * hook that wraps calls to the agencies api.
 */
export const useTenants = () => {
  const dispatch = useAppDispatch();
  const api = useApiTenants();

  /**
   * fetch all of the agencies from the server based on a filter.
   * @return the filtered, paged list of agencies.
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
      dispatch(error(tenantsSlice.name, error?.response?.status, error));
    } finally {
      dispatch(hideLoading());
    }
  }, [api, dispatch]);

  return {
    getSettings,
  };
};

export default useTenants;
