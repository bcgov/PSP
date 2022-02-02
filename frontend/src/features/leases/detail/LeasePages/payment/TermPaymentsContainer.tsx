import GenericModal from 'components/common/GenericModal';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { formLeasePaymentToApiPayment, IFormLeasePayment } from 'interfaces';
import { formLeaseTermToApiLeaseTerm, IFormLeaseTerm, ILeaseTerm } from 'interfaces/ILeaseTerm';
import { find, noop } from 'lodash';
import * as React from 'react';
import { useCallback } from 'react';
import { useContext, useState } from 'react';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';

import { useDeleteTermsPayments } from './hooks/useDeleteTermsPayments';
import { useLeasePayments } from './hooks/usePayments';
import { useLeaseTerms } from './hooks/useTerms';
import PaymentModal from './modal/payment/PaymentModal';
import TermModal from './modal/term/TermModal';
import TermsForm from './table/terms/TermsForm';

export interface ITermPaymentsContainerProps {}

/**
 * Orchestrates the display and modification of lease terms and payments.
 */
export const TermPaymentsContainer: React.FunctionComponent<ITermPaymentsContainerProps> = () => {
  const { setLease, lease } = useContext(LeaseStateContext);
  const [editModalValues, setEditModalValues] = useState<IFormLeaseTerm | undefined>(undefined);
  const [editPaymentModalValues, setEditPaymentModalValues] = useState<
    IFormLeasePayment | undefined
  >(undefined);

  const {
    onDeleteTerm,
    onDeletePayment,
    deleteModalWarning,
    setDeleteModalWarning,
    setConfirmDeleteModalValues,
    comfirmDeleteModalValues,
  } = useDeleteTermsPayments();
  const { updateLeaseTerm } = useLeaseTerms();
  const { updateLeasePayment } = useLeasePayments();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSaveTerm = useCallback(
    async (values: IFormLeaseTerm) => {
      const updatedLease = await updateLeaseTerm(formLeaseTermToApiLeaseTerm(values, gstDecimal));
      if (!!updatedLease?.id) {
        setLease(updatedLease);
        setEditModalValues(undefined);
      }
    },
    [gstDecimal, setLease, updateLeaseTerm],
  );

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSavePayment = useCallback(
    async (values: IFormLeasePayment) => {
      const updatedLease = await updateLeasePayment(
        formLeasePaymentToApiPayment({
          ...values,
          leaseId: lease?.id,
          leaseRowVersion: lease?.rowVersion,
        }),
      );
      if (!!updatedLease?.id) {
        setLease(updatedLease);
        setEditPaymentModalValues(undefined);
      }
    },
    [lease?.id, lease?.rowVersion, setLease, updateLeasePayment],
  );

  const onEdit = useCallback((values: IFormLeaseTerm) => {
    setEditModalValues(values);
  }, []);

  const onEditPayment = useCallback((values: IFormLeasePayment) => {
    setEditPaymentModalValues(values);
  }, []);

  return (
    <>
      <TermsForm
        onEdit={onEdit}
        onEditPayment={onEditPayment}
        onDelete={onDeleteTerm}
        onDeletePayment={onDeletePayment}
        onSavePayment={onSavePayment}
        isReceivable={lease?.paymentReceivableType?.id === 'RCVBL'}
      ></TermsForm>
      <PaymentModal
        displayModal={!!editPaymentModalValues}
        initialValues={editPaymentModalValues}
        onCancel={() => {
          setEditPaymentModalValues(undefined);
        }}
        onSave={onSavePayment}
      />
      <TermModal
        displayModal={!!editModalValues}
        initialValues={editModalValues}
        onCancel={() => {
          setEditModalValues(undefined);
        }}
        onSave={onSaveTerm}
      />
      <GenericModal
        display={!!comfirmDeleteModalValues}
        title={comfirmDeleteModalValues?.title}
        message={comfirmDeleteModalValues?.message}
        handleOk={comfirmDeleteModalValues?.onContinue ?? noop}
        okButtonText="Continue"
        cancelButtonText="Cancel"
        handleCancel={() => setConfirmDeleteModalValues(undefined)}
      />
      <GenericModal
        display={!!deleteModalWarning}
        title={deleteModalWarning?.title}
        message={deleteModalWarning?.message}
        handleOk={() => setDeleteModalWarning(undefined)}
        okButtonText="OK"
      />
    </>
  );
};

export const isActualGstEligible = (termId: number, terms: ILeaseTerm[]) => {
  return !!find(terms, (term: ILeaseTerm) => term.id === termId)?.isGstEligible;
};

export default TermPaymentsContainer;
