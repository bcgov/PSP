import { AxiosResponse } from 'axios';
import { useCallback, useContext, useState } from 'react';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { useLeasePaymentRepository } from '@/hooks/repositories/useLeasePaymentRepository';
import { IResponseWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LeaseTerm } from '@/models/api/LeaseTerm';

import { FormLeasePayment, FormLeaseTerm } from '../models';

export const useDeleteTermsPayments = (
  deleteLeaseTerm: IResponseWrapper<(term: Api_LeaseTerm) => Promise<AxiosResponse<boolean, any>>>,
  getLeaseTerms: (leaseId: number) => Promise<void>,
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
   * @param leaseTerm
   */
  const onDeleteTermConfirmed = useCallback(
    async (leaseTerm: FormLeaseTerm) => {
      const deleted = await deleteLeaseTerm.execute({
        ...FormLeaseTerm.toApi(leaseTerm),
        leaseId: lease?.id ?? 0,
      });
      if (deleted && lease?.id) {
        getLeaseTerms(lease.id);
      }
    },
    [deleteLeaseTerm, lease, getLeaseTerms],
  );

  /**
   * Validate term business rules during a deletion attempt
   * @param leaseTerm
   */
  const isValidForDelete = useCallback(
    (leaseTerm: FormLeaseTerm) => {
      if (leaseTerm.payments.length) {
        setDeleteModalWarning({ title: 'Delete Term', message: deleteWithPayments });
        return false;
      } else if (
        lease?.terms?.length !== undefined &&
        lease.terms.length > 1 &&
        leaseTerm.id === lease?.terms[0].id
      ) {
        setDeleteModalWarning({ title: 'Delete Term', message: deleteInitialWithRenewals });
        return false;
      }
      return true;
    },
    [lease?.terms],
  );

  /**
   * If a lease is deleted, trigger the confirmation modal.
   * @param leaseTerm
   */
  const onDeleteTerm = useCallback(
    (leaseTerm: FormLeaseTerm) => {
      if (isValidForDelete(leaseTerm)) {
        setConfirmDeleteModalValues({
          title: 'Delete Term',
          message: deleteWarning,
          onContinue: () => {
            onDeleteTermConfirmed(leaseTerm);
            setConfirmDeleteModalValues(undefined);
          },
        });
      }
    },
    [isValidForDelete, onDeleteTermConfirmed],
  );

  /**
   * If the deletion is confirmed, send the delete request. Use the response to update the parent lease.
   * @param leaseTerm
   */
  const onDeletePaymentConfirmed = useCallback(
    async (leasePayment: FormLeasePayment) => {
      if (leaseId) {
        const deleted = await deleteLeasePayment.execute(
          leaseId,
          FormLeasePayment.toApi(leasePayment),
        );
        if (deleted && leaseId) {
          getLeaseTerms(leaseId);
        }
      }
    },
    [deleteLeasePayment, leaseId, getLeaseTerms],
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
    onDeleteTerm,
    onDeleteTermConfirmed,
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
  'A term with payments attached can not be deleted. If you intend to delete this term, you must delete each of the corresponding payments first.';
const deleteInitialWithRenewals = `You must delete all renewals before deleting the initial term.`;
const deleteWarning = 'You are about to delete a term. Do you wish to continue?';
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
