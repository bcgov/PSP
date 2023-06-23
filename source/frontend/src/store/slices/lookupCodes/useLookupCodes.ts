import React from 'react';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import * as actionTypes from '@/constants/actionTypes';
import { useApiLookupCodes } from '@/hooks/pims-api/useApiLookupCodes';
import { useAppDispatch } from '@/store/hooks';

import { IGenericNetworkAction } from '../network/interfaces';
import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { ILookupCode, storeLookupCodes } from '.';

export const useLookupCodes = () => {
  const dispatch = useAppDispatch();
  const { getLookupCodes } = useApiLookupCodes();

  /**
   * Return an action that fetches all lookup codes within PIMS.
   * @returns an action that fetches all lookup codes.
   */
  const fetch = React.useCallback(async (): Promise<
    ILookupCode[] | { payload: IGenericNetworkAction; type: string }
  > => {
    dispatch(logRequest(actionTypes.GET_LOOKUP_CODES));
    dispatch(showLoading());
    return getLookupCodes()
      .then(response => {
        dispatch(logSuccess({ name: actionTypes.GET_LOOKUP_CODES }));
        dispatch(storeLookupCodes(response.data));
        dispatch(hideLoading());
        return response.data;
      })
      .catch(axiosError =>
        dispatch(
          logError({
            name: actionTypes.GET_LOOKUP_CODES,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        ),
      )
      .finally(() => dispatch(hideLoading()));
  }, [dispatch, getLookupCodes]);

  return {
    fetchLookupCodes: fetch,
  };
};
