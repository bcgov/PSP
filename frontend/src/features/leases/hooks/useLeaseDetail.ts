import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILease } from 'interfaces';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

export function useLeaseDetail(leaseId: number) {
  const [needsUpdate, setRefreshIndex] = useState<number>(0);
  const [lease, setLease] = useState<ILease>();
  const { getLease } = useApiLeases();
  const dispatch = useDispatch();

  useEffect(() => {
    const getLeaseById = async (id: number) => {
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
    };

    if (leaseId) {
      getLeaseById(leaseId);
    }
  }, [getLease, dispatch, leaseId, needsUpdate]);

  const refresh = () => {
    setRefreshIndex(r => r + 1);
  };

  return {
    lease,
    refresh,
    setLease,
  };
}
