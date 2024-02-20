import { FormikProps } from 'formik';
import { find, noop } from 'lodash';
import { useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { FaExclamationCircle, FaPlusCircle } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { ModalContext } from '@/contexts/modalContext';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { useGenerateH1005a } from '@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH1005a';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useLeasePaymentRepository } from '@/hooks/repositories/useLeasePaymentRepository';
import { useLeaseTermRepository } from '@/hooks/repositories/useLeaseTermRepository';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_Concepts_LeaseTerm } from '@/models/api/generated/ApiGen_Concepts_LeaseTerm';
import { getEmptyLease } from '@/models/defaultInitializers';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists, isValidId, isValidIsoDateTime } from '@/utils';

import { useDeleteTermsPayments } from './hooks/useDeleteTermsPayments';
import PaymentModal from './modal/payment/PaymentModal';
import TermForm from './modal/term/TermForm';
import { FormLeasePayment, FormLeaseTerm } from './models';
import TermsForm from './table/terms/TermsForm';

/**
 * Orchestrates the display and modification of lease terms and payments.
 */
export const TermPaymentsContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps>
> = ({ formikRef, onSuccess }) => {
  const { lease } = useContext(LeaseStateContext);
  const generateH1005a = useGenerateH1005a();
  const [editModalValues, setEditModalValues] = useState<FormLeaseTerm | undefined>(undefined);
  const [editPaymentModalValues, setEditPaymentModalValues] = useState<
    FormLeasePayment | undefined
  >(undefined);
  const [terms, setTerms] = useState<ApiGen_Concepts_LeaseTerm[]>([]);

  const { updateLeaseTerm, addLeaseTerm, getLeaseTerms, deleteLeaseTerm } =
    useLeaseTermRepository();

  const { setModalContent, setDisplayModal } = useContext(ModalContext);
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
    if (isValidId(leaseId)) {
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
  } = useDeleteTermsPayments(deleteLeaseTerm, refreshLeaseTerms, onSuccess);

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSaveTerm = useCallback(
    async (values: FormLeaseTerm) => {
      const updatedTerm = isValidId(values.id)
        ? await updateLeaseTerm.execute(FormLeaseTerm.toApi(values, gstDecimal))
        : await addLeaseTerm.execute(FormLeaseTerm.toApi(values, gstDecimal));

      if (isValidId(updatedTerm?.id) && isValidId(leaseId)) {
        const response = await getLeaseTerms.execute(leaseId);
        setTerms(response ?? []);
        setEditModalValues(undefined);
        onSuccess();
      }
    },
    [addLeaseTerm, getLeaseTerms, gstDecimal, leaseId, updateLeaseTerm, onSuccess],
  );

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSavePayment = useCallback(
    async (values: FormLeasePayment) => {
      if (isValidId(leaseId)) {
        const updatedLeasePayment = values.id
          ? await updateLeasePayment.execute(leaseId, FormLeasePayment.toApi(values))
          : await addLeasePayment.execute(leaseId, FormLeasePayment.toApi(values));
        if (isValidId(updatedLeasePayment?.id)) {
          const response = await getLeaseTerms.execute(leaseId);
          setTerms(response ?? []);
          setEditPaymentModalValues(undefined);
          onSuccess();
        }
      }
    },
    [leaseId, updateLeasePayment, addLeasePayment, getLeaseTerms, onSuccess],
  );

  const onEdit = useCallback(
    (values: FormLeaseTerm) => {
      if (lease?.terms?.length === 0) {
        values = {
          ...values,
          startDate: isValidIsoDateTime(lease?.startDate) ? lease.startDate : '',
        };
      }
      setEditModalValues(values);
    },
    [lease],
  );

  const onEditPayment = useCallback((values: FormLeasePayment) => {
    setEditPaymentModalValues(values);
  }, []);

  const onGenerate = () => {
    if (exists(lease)) {
      generateH1005a(lease);
    }
  };

  const onCancelTerm = useCallback(() => {
    setEditModalValues(undefined);
  }, []);

  const TermFormComp = useMemo(
    () => (
      <TermForm
        formikRef={formikRef as any}
        initialValues={editModalValues}
        onSave={onSaveTerm}
        lease={lease}
      />
    ),
    [editModalValues, formikRef, onSaveTerm, lease],
  );
  useEffect(() => {
    setModalContent({
      variant: 'info',
      headerIcon: !isValidId(editModalValues?.id) ? (
        <FaPlusCircle />
      ) : (
        <FaExclamationCircle size={22} />
      ),
      title: !isValidId(editModalValues?.id) ? 'Add a Term' : 'Edit a Term',
      okButtonText: 'Yes',
      cancelButtonText: 'No',
      handleCancel: onCancelTerm,
      handleOk: () => formikRef?.current?.submitForm(),
      message: TermFormComp,
      modalSize: ModalSize.MEDIUM,
    });
    setDisplayModal(!!editModalValues);
  }, [
    setModalContent,
    setDisplayModal,
    onCancelTerm,
    TermFormComp,
    editModalValues,
    formikRef,
    lease,
  ]);

  return (
    <>
      <LoadingBackdrop show={getLeaseTerms.loading} parentScreen />
      <TermsForm
        onEdit={onEdit}
        onEditPayment={onEditPayment}
        onDelete={onDeleteTerm}
        onDeletePayment={onDeletePayment}
        onSavePayment={onSavePayment}
        onGenerate={onGenerate}
        isReceivable={lease?.paymentReceivableType?.id === 'RCVBL'}
        lease={LeaseFormModel.fromApi({
          ...getEmptyLease(),
          terms: terms,
          type: lease?.type ?? null,
        })}
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
      <GenericModal
        variant="warning"
        display={!!comfirmDeleteModalValues}
        title={comfirmDeleteModalValues?.title}
        message={comfirmDeleteModalValues?.message}
        handleOk={comfirmDeleteModalValues?.onContinue ?? noop}
        okButtonText="Continue"
        cancelButtonText="Cancel"
        handleCancel={() => setConfirmDeleteModalValues(undefined)}
      />
      <GenericModal
        variant="info"
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
