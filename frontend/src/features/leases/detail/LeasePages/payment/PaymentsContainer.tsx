import GenericModal from 'components/common/GenericModal';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { formLeaseTermToApiLeaseTerm, IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import * as React from 'react';
import { useContext, useState } from 'react';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';

import { useLeaseTerms } from './hooks/useTerms';
import PaymentModal from './modal/PaymentModal';
import { PaymentsForm } from './table/PaymentsForm';

export interface IPaymentsContainerProps {}

const deleteWithPayments =
  'A term with payments attached can not be deleted. If you intend to delete this term, you must delete each of the corresponding payments first.';

const deleteInitialWithRenewals = `You must delete all renewals before deleting the initial term.`;
const deleteWarning = 'You are about to delete a term. Do you wish to continue?';

/**
 * Orchestrates the display and modification of lease terms and payments.
 */
export const PaymentsContainer: React.FunctionComponent<IPaymentsContainerProps> = () => {
  const { setLease, lease } = useContext(LeaseStateContext);
  const [displayModal, setDisplayModal] = useState<boolean>(false);
  const [comfirmDeleteModalValues, setConfirmDeleteModalValues] = useState<
    IFormLeaseTerm | undefined
  >(undefined);
  const [deleteModalWarning, setDeleteModalWarning] = useState<string | undefined>(undefined);
  const [editModalValues, setEditModalValues] = useState<IFormLeaseTerm | undefined>(undefined);
  const { updateLeaseTerm, removeLeaseTerm } = useLeaseTerms();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  /**
   * If a lease is deleted, trigger the confirmation modal.
   * @param leaseTerm
   */
  const onDeleteTerm = (leaseTerm: IFormLeaseTerm) => {
    if (isValidForDelete(leaseTerm)) {
      setConfirmDeleteModalValues(leaseTerm);
    }
  };

  /**
   * If the deletion is confirmed, send the delete request. Use the response to update the parent lease.
   * @param leaseTerm
   */
  const onDeleteTermConfirmed = async (leaseTerm: IFormLeaseTerm) => {
    const updatedLease = await removeLeaseTerm({
      ...formLeaseTermToApiLeaseTerm(leaseTerm, gstDecimal),
      leaseId: lease?.id,
      leaseRowVersion: lease?.rowVersion,
    });
    if (!!updatedLease?.id) {
      setLease(updatedLease);
    }
  };

  /**
   * Validate business rules during a deletion attempt
   * @param leaseTerm
   */
  const isValidForDelete = (leaseTerm: IFormLeaseTerm) => {
    if (leaseTerm.payments.length) {
      setDeleteModalWarning(deleteWithPayments);
      return false;
    } else if (
      leaseTerm.id === lease?.terms[0].id &&
      lease?.terms?.length !== undefined &&
      lease.terms.length > 1
    ) {
      setDeleteModalWarning(deleteInitialWithRenewals);
      return false;
    }
    return true;
  };

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSaveTerm = async (values: IFormLeaseTerm) => {
    const updatedLease = await updateLeaseTerm(formLeaseTermToApiLeaseTerm(values, gstDecimal));
    if (!!updatedLease?.id) {
      setLease(updatedLease);
      setEditModalValues(undefined);
      setDisplayModal(false);
    }
  };

  return (
    <>
      <PaymentsForm
        onEdit={(values: IFormLeaseTerm) => {
          setEditModalValues(values);
          setDisplayModal(true);
        }}
        onDelete={onDeleteTerm}
        setDisplayModal={setDisplayModal}
      ></PaymentsForm>
      <PaymentModal
        displayModal={displayModal}
        initialValues={editModalValues}
        onCancel={() => {
          setDisplayModal(false);
          setEditModalValues(undefined);
        }}
        onSave={onSaveTerm}
      />
      <GenericModal
        display={!!comfirmDeleteModalValues}
        title="Delete Term"
        message={deleteWarning}
        handleOk={() => {
          comfirmDeleteModalValues && onDeleteTermConfirmed(comfirmDeleteModalValues);
          setConfirmDeleteModalValues(undefined);
        }}
        okButtonText="Continue"
        cancelButtonText="Cancel"
        handleCancel={() => setConfirmDeleteModalValues(undefined)}
      />
      <GenericModal
        display={!!deleteModalWarning}
        title="Delete Term"
        message={deleteModalWarning}
        handleOk={() => setDeleteModalWarning(undefined)}
        okButtonText="OK"
      />
    </>
  );
};

export default PaymentsContainer;
