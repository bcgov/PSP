import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import {
  formLeasePaymentToApiPayment,
  formLeaseTermToApiLeaseTerm,
  IFormLeasePayment,
  IFormLeaseTerm,
} from 'interfaces';
import { useCallback, useContext, useState } from 'react';

import { useLeasePayments } from './usePayments';
import { useLeaseTerms } from './useTerms';

export const useDeleteTermsPayments = () => {
  const [comfirmDeleteModalValues, setConfirmDeleteModalValues] = useState<
    IDeleteConfirmationModal | undefined
  >(undefined);
  const [deleteModalWarning, setDeleteModalWarning] = useState<IDeleteWarningModal | undefined>(
    undefined,
  );

  const { removeLeaseTerm } = useLeaseTerms();
  const { removeLeasePayment } = useLeasePayments();
  const { setLease, lease } = useContext(LeaseStateContext);

  /**
   * If the deletion is confirmed, send the delete request. Use the response to update the parent lease.
   * @param leaseTerm
   */
  const onDeleteTermConfirmed = useCallback(
    async (leaseTerm: IFormLeaseTerm) => {
      const updatedLease = await removeLeaseTerm({
        ...formLeaseTermToApiLeaseTerm(leaseTerm),
        leaseId: lease?.id,
        leaseRowVersion: lease?.rowVersion,
      });
      if (!!updatedLease?.id) {
        setLease(updatedLease);
      }
    },
    [lease?.id, lease?.rowVersion, removeLeaseTerm, setLease],
  );

  /**
   * Validate term business rules during a deletion attempt
   * @param leaseTerm
   */
  const isValidForDelete = useCallback(
    (leaseTerm: IFormLeaseTerm) => {
      if (leaseTerm.payments.length) {
        setDeleteModalWarning({ title: 'Delete Term', message: deleteWithPayments });
        return false;
      } else if (
        leaseTerm.id === lease?.terms[0].id &&
        lease?.terms?.length !== undefined &&
        lease.terms.length > 1
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
    (leaseTerm: IFormLeaseTerm) => {
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
    async (leasePayment: IFormLeasePayment) => {
      const updatedLease = await removeLeasePayment({
        ...formLeasePaymentToApiPayment(leasePayment),
        leaseId: lease?.id,
        leaseRowVersion: lease?.rowVersion,
      });
      if (!!updatedLease?.id) {
        setLease(updatedLease);
      }
    },
    [lease?.id, lease?.rowVersion, removeLeasePayment, setLease],
  );

  /**
   * If a payment is deleted, trigger the confirmation modal.
   * @param leasePayment
   */
  const onDeletePayment = useCallback(
    (leasePayment: IFormLeasePayment) => {
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
