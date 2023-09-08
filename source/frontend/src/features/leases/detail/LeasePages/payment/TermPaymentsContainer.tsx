import { FormikProps } from 'formik';
import { find, noop } from 'lodash';
import { useCallback, useContext, useState } from 'react';

import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useLeasePaymentRepository } from '@/hooks/repositories/useLeasePaymentRepository';
import { useLeaseTermRepository } from '@/hooks/repositories/useLeaseTermRepository';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { defaultApiLease } from '@/models/api/Lease';
import { Api_LeaseTerm } from '@/models/api/LeaseTerm';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

import { useDeleteTermsPayments } from './hooks/useDeleteTermsPayments';
import PaymentModal from './modal/payment/PaymentModal';
import TermModal from './modal/term/TermModal';
import { FormLeasePayment, FormLeaseTerm } from './models';
import TermsForm from './table/terms/TermsForm';

/**
 * Orchestrates the display and modification of lease terms and payments.
 */
export const TermPaymentsContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps>
> = ({ formikRef }) => {
  const { lease } = useContext(LeaseStateContext);
  const [editModalValues, setEditModalValues] = useState<FormLeaseTerm | undefined>(undefined);
  const [editPaymentModalValues, setEditPaymentModalValues] = useState<
    FormLeasePayment | undefined
  >(undefined);
  const [terms, setTerms] = useState<Api_LeaseTerm[]>([]);

  const { updateLeaseTerm, addLeaseTerm, getLeaseTerms, deleteLeaseTerm } =
    useLeaseTermRepository();

  const { updateLeasePayment, addLeasePayment } = useLeasePaymentRepository();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  const leaseId = lease?.id;
  const getLeaseTermsFunc = getLeaseTerms.execute;
  const refreshLeaseTerms = useCallback(
    async (leaseId: number) => {
      if (leaseId) {
        const response = await getLeaseTermsFunc(leaseId);
        setTerms(response ?? []);
      }
    },
    [getLeaseTermsFunc],
  );
  useDeepCompareEffect(() => {
    if (!!leaseId) {
      refreshLeaseTerms(leaseId);
    }
  }, [refreshLeaseTerms, leaseId]);

  const {
    onDeleteTerm,
    onDeletePayment,
    deleteModalWarning,
    setDeleteModalWarning,
    setConfirmDeleteModalValues,
    comfirmDeleteModalValues,
  } = useDeleteTermsPayments(deleteLeaseTerm, refreshLeaseTerms);

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSaveTerm = useCallback(
    async (values: FormLeaseTerm) => {
      const updatedTerm = values.id
        ? await updateLeaseTerm.execute(FormLeaseTerm.toApi(values, gstDecimal))
        : await addLeaseTerm.execute(FormLeaseTerm.toApi(values, gstDecimal));
      if (!!updatedTerm?.id && leaseId) {
        const response = await getLeaseTerms.execute(leaseId);
        setTerms(response ?? []);
        setEditModalValues(undefined);
      }
    },
    [addLeaseTerm, getLeaseTerms, gstDecimal, leaseId, updateLeaseTerm],
  );

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSavePayment = useCallback(
    async (values: FormLeasePayment) => {
      if (leaseId) {
        const updatedLeasePayment = values.id
          ? await updateLeasePayment.execute(leaseId, FormLeasePayment.toApi(values))
          : await addLeasePayment.execute(leaseId, FormLeasePayment.toApi(values));
        if (!!updatedLeasePayment?.id) {
          const response = await getLeaseTerms.execute(leaseId);
          setTerms(response ?? []);
          setEditPaymentModalValues(undefined);
        }
      }
    },
    [leaseId, updateLeasePayment, addLeasePayment, getLeaseTerms],
  );

  const onEdit = useCallback(
    (values: FormLeaseTerm) => {
      if (lease?.terms?.length === 0) {
        values = { ...values, startDate: lease?.startDate ?? '' };
      }
      setEditModalValues(values);
    },
    [lease],
  );

  const onEditPayment = useCallback((values: FormLeasePayment) => {
    setEditPaymentModalValues(values);
  }, []);

  return (
    <>
      <LoadingBackdrop show={getLeaseTerms.loading} parentScreen />
      <TermsForm
        onEdit={onEdit}
        onEditPayment={onEditPayment}
        onDelete={onDeleteTerm}
        onDeletePayment={onDeletePayment}
        onSavePayment={onSavePayment}
        isReceivable={lease?.paymentReceivableType?.id === 'RCVBL'}
        lease={LeaseFormModel.fromApi({ ...defaultApiLease, terms: terms })}
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
      ></TermsForm>
      <PaymentModal
        displayModal={!!editPaymentModalValues}
        initialValues={editPaymentModalValues}
        onCancel={() => {
          setEditPaymentModalValues(undefined);
        }}
        onSave={onSavePayment}
        terms={terms ?? []}
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

export const isActualGstEligible = (termId: number, terms: FormLeaseTerm[]) => {
  return !!find(terms, (term: FormLeaseTerm) => term.id === termId)?.isGstEligible;
};

export default TermPaymentsContainer;
