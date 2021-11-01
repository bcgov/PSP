import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILease } from 'interfaces';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';
import { logError } from 'store/slices/network/networkSlice';

/**
 * hook that fetches the lease given the lease id.
 * @param leaseId
 */
export const useLeaseDetail = (leaseId?: number) => {
  const [lease, setLease] = useState<ILease>();
  const { getLease } = useApiLeases();
  const dispatch = useDispatch();

  useEffect(() => {
    const getLeaseById = async (id: number) => {
      dispatch(showLoading());
      return getLease(id)
        .then(result => {
          const { data } = result;
          setLease(data);
          return data;
        })
        .catch(axiosError => {
          toast.error('Failed to load lease, reload this page to try again.');
          dispatch(
            logError({
              name: 'LeaseLoad',
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        })
        .finally(() => {
          dispatch(hideLoading());
        });
    };
    if (leaseId) {
      getLeaseById(leaseId);
    } else {
      toast.error(
        'No valid lease id provided, go back to the lease and license list and select a valid lease.',
      );
    }
  }, [getLease, leaseId, dispatch]);

  return { lease };
};
