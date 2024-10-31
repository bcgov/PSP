import { FormikProps } from 'formik/dist/types';
import { find, noop } from 'lodash';
import moment from 'moment';
import { useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { FaExclamationCircle, FaPlusCircle } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { ModalContext } from '@/contexts/modalContext';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useLeasePaymentRepository } from '@/hooks/repositories/useLeasePaymentRepository';
import { useLeasePeriodRepository } from '@/hooks/repositories/useLeasePeriodRepository';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { ApiGen_CodeTypes_LeasePaymentCategoryTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentCategoryTypes';
import { getEmptyLease } from '@/models/defaultInitializers';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { isValidId, isValidIsoDateTime } from '@/utils';

import { useDeletePeriodsPayments } from './hooks/useDeletePeriodsPayments';
import PaymentModal from './modal/payment/PaymentModal';
import PeriodForm from './modal/period/PeriodForm';
import { FormLeasePayment, FormLeasePeriod } from './models';
import { IPeriodPaymentsViewProps } from './table/periods/PaymentPeriodsView';

/**
 * Orchestrates the display and modification of lease periods and payments.
 */
export const PeriodPaymentsContainer: React.FunctionComponent<
  LeasePageProps<IPeriodPaymentsViewProps>
> = ({ formikRef, onSuccess, componentView }) => {
  const { lease } = useContext(LeaseStateContext);
  const [editModalValues, setEditModalValues] = useState<FormLeasePeriod | undefined>(undefined);
  const [editPaymentModalValues, setEditPaymentModalValues] = useState<
    FormLeasePayment | undefined
  >(undefined);

  const { updateLeasePeriod, addLeasePeriod, getLeasePeriods, deleteLeasePeriod } =
    useLeasePeriodRepository();

  const { setModalContent, setDisplayModal } = useContext(ModalContext);
  const { updateLeasePayment, addLeasePayment } = useLeasePaymentRepository();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);

  const leaseId = lease?.id;
  const getLeasePeriodsFunc = getLeasePeriods.execute;
  const refreshLeasePeriods = useCallback(
    async (leaseId: number) => {
      if (leaseId) {
        await getLeasePeriodsFunc(leaseId);
      }
    },
    [getLeasePeriodsFunc],
  );

  useDeepCompareEffect(() => {
    if (isValidId(leaseId)) {
      refreshLeasePeriods(leaseId);
    }
  }, [refreshLeasePeriods, leaseId]);

  const {
    onDeletePeriod,
    onDeletePayment,
    deleteModalWarning,
    setDeleteModalWarning,
    setConfirmDeleteModalValues,
    comfirmDeleteModalValues,
  } = useDeletePeriodsPayments(
    deleteLeasePeriod,
    refreshLeasePeriods,
    getLeasePeriods.response,
    onSuccess,
  );

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param values
   */
  const onSavePeriod = useCallback(
    async (values: FormLeasePeriod) => {
      const updatedPeriod = isValidId(values.id)
        ? await updateLeasePeriod.execute(FormLeasePeriod.toApi(values))
        : await addLeasePeriod.execute(FormLeasePeriod.toApi(values));

      if (isValidId(updatedPeriod?.id) && isValidId(leaseId)) {
        await getLeasePeriods.execute(leaseId);
        setEditModalValues(undefined);
        setDisplayModal(false);
        onSuccess();
      }
    },
    [addLeasePeriod, getLeasePeriods, leaseId, updateLeasePeriod, onSuccess, setDisplayModal],
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
          await getLeasePeriods.execute(leaseId);
          setEditPaymentModalValues(undefined);
          setDisplayModal(false);
          onSuccess();
        }
      }
    },
    [leaseId, updateLeasePayment, addLeasePayment, getLeasePeriods, onSuccess, setDisplayModal],
  );

  const onEdit = useCallback(
    (values: FormLeasePeriod) => {
      if (getLeasePeriods?.response?.length === 0) {
        values = {
          ...values,
          startDate: isValidIsoDateTime(lease?.startDate) ? lease.startDate : '',
        };
      }

      // For new periods, adjust the "Requires GST" field based on whether the lease is receivable or payable.
      if (!isValidId(values?.id)) {
        const isReceivableLease =
          lease?.paymentReceivableType?.id === ApiGen_CodeTypes_LeaseAccountTypes.RCVBL.toString();
        values = {
          ...values,
          isGstEligible: isReceivableLease ? true : false,
          isAdditionalRentGstEligible: isReceivableLease ? true : false,
          isVariableRentGstEligible: isReceivableLease ? true : false,
        };

        if (getLeasePeriods?.response?.length > 0) {
          const lastPeriod = getLeasePeriods?.response.slice(-1)[0];
          const startDate = isValidIsoDateTime(lastPeriod.expiryDate)
            ? moment(lastPeriod.expiryDate).add(1, 'days')
            : '';

          values.startDate = startDate ? startDate.toISOString() : '';
        }
      }

      setEditModalValues(values);
    },
    [getLeasePeriods?.response, lease?.paymentReceivableType?.id, lease.startDate],
  );

  const onEditPayment = useCallback((values: FormLeasePayment) => {
    setEditPaymentModalValues(values);
  }, []);

  const onCancelPeriod = useCallback(() => {
    setEditModalValues(undefined);
    setDisplayModal(false);
  }, [setDisplayModal]);

  const PeriodFormComp = useMemo(
    () => (
      <PeriodForm
        formikRef={formikRef as any}
        initialValues={editModalValues}
        onSave={onSavePeriod}
        lease={lease}
        gstConstant={gstConstant}
      />
    ),
    [formikRef, editModalValues, onSavePeriod, lease, gstConstant],
  );

  useEffect(() => {
    if (editModalValues) {
      setModalContent({
        variant: 'info',
        headerIcon: !isValidId(editModalValues?.id) ? (
          <FaPlusCircle />
        ) : (
          <FaExclamationCircle size={22} />
        ),
        title: !isValidId(editModalValues?.id) ? 'Add a Period' : 'Edit a Period',
        okButtonText: 'Yes',
        cancelButtonText: 'No',
        handleCancel: onCancelPeriod,
        handleOk: () => formikRef?.current?.submitForm(),
        message: PeriodFormComp,
        modalSize: ModalSize.MEDIUM,
      });
      setDisplayModal(!!editModalValues);
    }
  }, [
    setModalContent,
    setDisplayModal,
    onCancelPeriod,
    PeriodFormComp,
    editModalValues,
    formikRef,
    lease,
  ]);

  const View = componentView;
  return (
    <>
      <LoadingBackdrop show={getLeasePeriods.loading} parentScreen />
      <View
        onEdit={onEdit}
        onEditPayment={onEditPayment}
        onDelete={onDeletePeriod}
        onDeletePayment={onDeletePayment}
        onSavePayment={onSavePayment}
        isReceivable={
          lease?.paymentReceivableType?.id === ApiGen_CodeTypes_LeaseAccountTypes.RCVBL.toString()
        }
        lease={LeaseFormModel.fromApi({
          ...getEmptyLease(),
          periods: getLeasePeriods.response ?? [],
          type: lease?.type ?? null,
        })}
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
      ></View>
      <PaymentModal
        displayModal={!!editPaymentModalValues}
        initialValues={editPaymentModalValues}
        onCancel={() => {
          setEditPaymentModalValues(undefined);
        }}
        onSave={onSavePayment}
        periods={getLeasePeriods.response ?? []}
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

export const isActualGstEligible = (
  periodId: number,
  periods: FormLeasePeriod[],
  category: ApiGen_CodeTypes_LeasePaymentCategoryTypes,
) => {
  const period = find(periods, (period: FormLeasePeriod) => period.id === periodId);
  return (
    (category === ApiGen_CodeTypes_LeasePaymentCategoryTypes.VBL &&
      period?.isVariableRentGstEligible) ||
    (category === ApiGen_CodeTypes_LeasePaymentCategoryTypes.ADDL &&
      period?.isAdditionalRentGstEligible) ||
    (category === ApiGen_CodeTypes_LeasePaymentCategoryTypes.BASE && period?.isGstEligible)
  );
};

export default PeriodPaymentsContainer;
