import { showLoading, hideLoading } from 'react-redux-loading-bar';
import { request, success, error } from 'actions/genericActions';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { AxiosResponse, AxiosError } from 'axios';
import { useAppDispatch } from 'store/hooks';
import React from 'react';
import { ILookupCode, storeLookupCodes } from '.';

export const useLookupCodes = () => {
  const dispatch = useAppDispatch();

  /**
   * Return an action that fetches all lookup codes within PIMS.
   * @returns an action that fetches all lookup codes.
   */
  const fetch = React.useCallback(async (): Promise<ILookupCode> => {
    dispatch(request(actionTypes.GET_LOOKUP_CODES));
    dispatch(showLoading());
    return CustomAxios()
      .get(ENVIRONMENT.apiUrl + API.LOOKUP_CODE_SET('all'))
      .then((response: AxiosResponse) => {
        dispatch(success(actionTypes.GET_LOOKUP_CODES));
        dispatch(storeLookupCodes(response.data));
        dispatch(hideLoading());
        return response.data;
      })
      .catch((axiosError: AxiosError) =>
        dispatch(error(actionTypes.GET_LOOKUP_CODES, axiosError?.response?.status, axiosError)),
      )
      .finally(() => dispatch(hideLoading()));
  }, [dispatch]);

  return {
    fetchLookupCodes: fetch,
  };
};
