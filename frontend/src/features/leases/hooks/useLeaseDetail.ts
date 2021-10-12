import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILease } from 'interfaces';
import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that fetches the lease given the lease id.
 * @param leaseId
 */
export const useLeaseDetail = (leaseId?: number) => {
  const [lease, setLease] = useState<ILease>();
  const { getLease } = useApiLeases();

  useEffect(() => {
    const getLeaseById = async (id: number) => {
      try {
        const { data } = await getLease(id);
        setLease(data);
      } catch (e) {
        toast.error('Failed to load lease, reload this page to try again.');
      }
    };
    if (leaseId) {
      getLeaseById(leaseId);
    } else {
      toast.error(
        'No valid lease id provided, go back to the lease and license list and select a valid lease.',
      );
    }
  }, [getLease, leaseId]);

  return { lease };
};
