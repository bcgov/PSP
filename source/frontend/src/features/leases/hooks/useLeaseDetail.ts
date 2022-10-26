import axios, { AxiosError } from 'axios';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { useContext, useEffect, useMemo } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';
import { useAxiosErrorHandler } from 'utils';

import { LeaseStateContext } from './../context/LeaseContext';

export function useLeaseDetail(leaseId?: number) {
  const { lease, setLease } = useContext(LeaseStateContext);
  leaseId = leaseId ?? lease?.id;
  const { getLease, getApiLease } = useApiLeases();
  const dispatch = useDispatch();

  const getLeaseById = useMemo(
    () => async (id?: number) => {
      if (!id) {
        throw Error(`cannot retrieve lease with id of ${id}`);
      }
      try {
        dispatch(showLoading());
        const { data } = await getLease(id);
        setLease(data);
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          toast.error('Failed to load lease, reload this page to try again.');
          dispatch(
            logError({
              name: 'getLeaseById',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        }
      } finally {
        dispatch(hideLoading());
      }
    },
    [dispatch, getLease, setLease],
  );

  const getApiLeaseById = useApiRequestWrapper({
    requestFunction: getApiLease,
    requestName: 'getApiLease',
    onError: useAxiosErrorHandler('Failed to load lease, reload this page to try again.'),
  });

  useEffect(() => {
    if (leaseId) {
      getLeaseById(leaseId);
    }
  }, [leaseId, getLeaseById]);

  return {
    lease,
    setLease,
    refresh: () => leaseId && getLeaseById(leaseId),
    getApiLeaseById,
  };
}
