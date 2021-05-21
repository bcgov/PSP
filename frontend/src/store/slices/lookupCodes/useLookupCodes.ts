import { useApiLookupCodes } from './../../../hooks/pims-api/useApiLookupCodes';
import { showLoading, hideLoading } from 'react-redux-loading-bar';
import * as actionTypes from 'constants/actionTypes';
import { AxiosResponse, AxiosError } from 'axios';
import { useAppDispatch } from 'store/hooks';
import React from 'react';
import { ILookupCode, storeLookupCodes } from '.';
import { logRequest, logSuccess, logError } from '../network/networkSlice';

export const useLookupCodes = () => {
  const dispatch = useAppDispatch();
  const { getLookupCodes } = useApiLookupCodes();

  /**
   * Return an action that fetches all lookup codes within PIMS.
   * @returns an action that fetches all lookup codes.
   */
  const fetch = React.useCallback(async (): Promise<ILookupCode> => {
    dispatch(logRequest(actionTypes.GET_LOOKUP_CODES));
    dispatch(showLoading());
    return getLookupCodes()
      .then((response: AxiosResponse) => {
        dispatch(logSuccess({ name: actionTypes.GET_LOOKUP_CODES }));
        dispatch(storeLookupCodes(response.data));
        dispatch(hideLoading());
        return response.data;
      })
      .catch((axiosError: AxiosError) =>
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
