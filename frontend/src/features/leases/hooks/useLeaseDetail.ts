import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { useContext, useEffect, useMemo } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

import { LeaseStateContext } from './../context/LeaseContext';

export function useLeaseDetail(leaseId: number) {
  const { lease, setLease } = useContext(LeaseStateContext);
  const { getLease } = useApiLeases();
  const dispatch = useDispatch();

  const getLeaseById = useMemo(
    () => async (id: number) => {
      try {
        dispatch(showLoading());
        const { data } = await getLease(id);
        setLease(data);
      } catch (e) {
        toast.error('Failed to load lease, reload this page to try again.');
        dispatch(
          logError({
            name: 'LeaseLoad',
            status: e?.response?.status,
            error: e,
          }),
        );
      } finally {
        dispatch(hideLoading());
      }
    },
    [dispatch, getLease, setLease],
  );

  useEffect(() => {
    if (leaseId) {
      getLeaseById(leaseId);
    }
  }, [leaseId, getLeaseById]);

  return {
    lease,
    setLease,
    refresh: () => getLeaseById(leaseId),
  };
}
