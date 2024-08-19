import { AxiosResponse } from 'axios';
import { useCallback, useContext, useState } from 'react';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { useLeasePaymentRepository } from '@/hooks/repositories/useLeasePaymentRepository';
import { IResponseWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';

import { FormLeasePayment, FormLeasePeriod } from '../models';

export const useDeletePeriodsPayments = (
  deleteLeasePeriod: IResponseWrapper<
    (period: ApiGen_Concepts_LeasePeriod) => Promise<AxiosResponse<boolean, any>>
  >,
  getLeasePeriods: (leaseId: number) => Promise<void>,
  leasePeriods: ApiGen_Concepts_LeasePeriod[],
  onSuccess: () => void,
) => {
  const [comfirmDeleteModalValues, setConfirmDeleteModalValues] = useState<
    IDeleteConfirmationModal | undefined
  >(undefined);
  const [deleteModalWarning, setDeleteModalWarning] = useState<IDeleteWarningModal | undefined>(
    undefined,
  );

  const { deleteLeasePayment } = useLeasePaymentRepository();

  const { lease } = useContext(LeaseStateContext);
  const leaseId = lease?.id;

  /**
   * If the deletion is confirmed, send the delete request. Use the response to update the parent lease.
   * @param leasePeriod
   */
  const onDeletePeriodConfirmed = useCallback(
    async (leasePeriod: FormLeasePeriod) => {
      const deleted = await deleteLeasePeriod.execute({
        ...FormLeasePeriod.toApi(leasePeriod),
        leaseId: lease?.id ?? 0,
      });
      if (deleted && lease?.id) {
        getLeasePeriods(lease.id);
        onSuccess();
      }
    },
    [deleteLeasePeriod, lease, getLeasePeriods, onSuccess],
  );

  /**
   * Validate period business rules during a deletion attempt
   * @param leasePeriod
   */
  const isValidForDelete = useCallback(
    (leasePeriod: FormLeasePeriod) => {
      if (leasePeriod.payments.length > 0) {
        setDeleteModalWarning({ title: 'Delete Period', message: deleteWithPayments });
        return false;
      } else if (
        leasePeriods?.length !== undefined &&
        leasePeriods.length > 1 &&
        leasePeriod.id === leasePeriods[0].id
      ) {
        setDeleteModalWarning({ title: 'Delete Period', message: deleteInitialWithRenewals });
        return false;
      }
      return true;
    },
    [leasePeriods],
  );

  /**
   * If a lease is deleted, trigger the confirmation modal.
   * @param leasePeriod
   */
  const onDeletePeriod = useCallback(
    (leasePeriod: FormLeasePeriod) => {
      if (isValidForDelete(leasePeriod)) {
        setConfirmDeleteModalValues({
          title: 'Delete Period',
          message: deleteWarning,
          onContinue: () => {
            onDeletePeriodConfirmed(leasePeriod);
            setConfirmDeleteModalValues(undefined);
          },
        });
      }
    },
    [isValidForDelete, onDeletePeriodConfirmed],
  );

  /**
   * If the deletion is confirmed, send the delete request. Use the response to update the parent lease.
   * @param leasePeriod
   */
  const onDeletePaymentConfirmed = useCallback(
    async (leasePayment: FormLeasePayment) => {
      if (leaseId) {
        const deleted = await deleteLeasePayment.execute(
          leaseId,
          FormLeasePayment.toApi(leasePayment),
        );
        if (deleted && leaseId) {
          getLeasePeriods(leaseId);
          onSuccess();
        }
      }
    },
    [deleteLeasePayment, leaseId, getLeasePeriods, onSuccess],
  );

  /**
   * If a payment is deleted, trigger the confirmation modal.
   * @param leasePayment
   */
  const onDeletePayment = useCallback(
    (leasePayment: FormLeasePayment) => {
      setConfirmDeleteModalValues({
        title: 'Delete Payment',
        message: deletePaymentWarning,
        onContinue: () => {
          leasePayment && onDeletePaymentConfirmed(leasePayment);
          setConfirmDeleteModalValues(undefined);
        },
      });
    },
    [onDeletePaymentConfirmed],
  );
  return {
    onDeletePeriod,
    onDeletePeriodConfirmed,
    onDeletePayment,
    onDeletePaymentConfirmed,
    deleteModalWarning,
    setConfirmDeleteModalValues,
    setDeleteModalWarning,
    deletePaymentWarning,
    comfirmDeleteModalValues,
  };
};

const deleteWithPayments =
  'A period with payments attached can not be deleted. If you intend to delete this period, you must delete each of the corresponding payments first.';
const deleteInitialWithRenewals = `You must delete all renewals before deleting the initial period.`;
const deleteWarning = 'You are about to delete a period. Do you wish to continue?';
const deletePaymentWarning = 'You are about to delete a payment. Do you wish to continue?';

interface IDeleteWarningModal {
  message: string;
  title: string;
}

interface IDeleteConfirmationModal {
  message: string;
  title: string;
  onContinue: () => void;
}
