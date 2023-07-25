import React from 'react';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import * as actionTypes from '@/constants/actionTypes';
import { useApiSystemConstants } from '@/hooks/pims-api/useApiSystemConstants';
import { useAppDispatch, useAppSelector } from '@/store/hooks';

import { IGenericNetworkAction } from '../network/interfaces';
import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { ISystemConstant, storeSystemConstants } from '.';

export enum SystemConstants {
  FYEND = 'FYEND',
  FYSTART = 'FYSTART',
  GST = 'GST',
  CLIENT = 'CLIENT',
}

export const useSystemConstants = () => {
  const dispatch = useAppDispatch();
  const { getSystemConstants } = useApiSystemConstants();
  const systemConstants: ISystemConstant[] = useAppSelector(
    state => state.systemConstant?.systemConstants ?? [],
  );

  const getSystemConstant = (constant: SystemConstants) => {
    return systemConstants.find(c => c.name === constant);
  };

  /**
   * Return an action that fetches the PIMS system constants.
   * @returns an action that fetches the system constants.
   */
  const fetch = React.useCallback(async (): Promise<
    ISystemConstant[] | { payload: IGenericNetworkAction; type: string }
  > => {
    dispatch(logRequest(actionTypes.GET_SYSTEM_CONSTANTS));
    dispatch(showLoading());
    return getSystemConstants()
      .then(response => {
        dispatch(logSuccess({ name: actionTypes.GET_SYSTEM_CONSTANTS }));
        dispatch(storeSystemConstants(response.data));
        dispatch(hideLoading());
        return response.data;
      })
      .catch(axiosError =>
        dispatch(
          logError({
            name: actionTypes.GET_SYSTEM_CONSTANTS,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        ),
      )
      .finally(() => dispatch(hideLoading()));
  }, [dispatch, getSystemConstants]);

  return {
    fetchSystemConstants: fetch,
    systemConstants,
    getSystemConstant,
  };
};
