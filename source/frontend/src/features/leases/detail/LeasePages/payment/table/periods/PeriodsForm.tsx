import { Formik, FormikProps } from 'formik';
import { find, noop, orderBy } from 'lodash';
import { useMemo } from 'react';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';

import { Button } from '@/components/common/buttons';
import { Section } from '@/components/common/Section/Section';
import { Table } from '@/components/Table';
import { Claims, LeasePeriodStatusTypes } from '@/constants';
import { LeaseFormModel } from '@/features/leases/models';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useDeepCompareMemo from '@/hooks/util/useDeepCompareMemo';
import { prettyFormatDate } from '@/utils';

import { defaultFormLeasePeriod, FormLeasePayment, FormLeasePeriod } from '../../models';
import PaymentsForm from '../payments/PaymentsForm';
import { getLeasePeriodColumns } from './columns';

export interface IPeriodsFormProps {
  onEdit: (values: FormLeasePeriod) => void;
  onEditPayment: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePeriod) => void;
  onDeletePayment: (values: FormLeasePayment) => void;
  onSavePayment: (values: FormLeasePayment) => void;
  onGenerate: () => void;
  isReceivable?: boolean;
  lease?: LeaseFormModel;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
}

export const PeriodsForm: React.FunctionComponent<React.PropsWithChildren<IPeriodsFormProps>> = ({
  onEdit,
  onEditPayment,
  onDelete,
  onDeletePayment,
  onSavePayment,
  onGenerate,
  isReceivable,
  lease,
  formikRef,
}) => {
  const columns = useMemo(
    () =>
      getLeasePeriodColumns({
        onEdit,
        onDelete: onDelete,
        onGenerate,
        leaseTypeCode: lease?.leaseTypeCode,
      }),
    [onEdit, onDelete, onGenerate, lease?.leaseTypeCode],
  );
  const { hasClaim } = useKeycloakWrapper();
  const leaseForm = { ...new LeaseFormModel(), ...lease };

  //Get the most recent payment for display, if one exists.
  const allPayments = orderBy(
    (leaseForm.periods ?? []).flatMap(p => p.payments),
    'receivedDate',
    'desc',
  );
  const lastPaymentDate = allPayments.length > 0 ? allPayments[0]?.receivedDate : '';

  /** This is the payments subtable displayed for each period row. */
  const renderPayments = useDeepCompareMemo(
    () => (row: FormLeasePeriod) => {
      const matchingPeriod = leaseForm.periods.find(t => t.id === row.id);
      return (
        <PaymentsForm
          onSave={onSavePayment}
          onEdit={onEditPayment}
          onDelete={onDeletePayment}
          nameSpace={`periods.${matchingPeriod ? leaseForm.periods.indexOf(matchingPeriod) : 0}`}
          isExercised={row?.statusTypeCode?.id === LeasePeriodStatusTypes.EXERCISED}
          isGstEligible={row.isGstEligible}
          isReceivable={isReceivable}
          periodId={row.id ?? undefined}
        />
      );
    },
    [leaseForm],
  );

  return (
    <Formik<LeaseFormModel>
      initialValues={leaseForm}
      enableReinitialize={true}
      innerRef={formikRef}
      onSubmit={noop}
    >
      {formikProps => (
        <Section header="Payments by Period">
          {hasClaim(Claims.LEASE_ADD) && (
            <Button variant="secondary" onClick={() => onEdit(defaultFormLeasePeriod)}>
              Add a Period
            </Button>
          )}
          {lastPaymentDate && <b>last payment received: {prettyFormatDate(lastPaymentDate)}</b>}
          <Table<FormLeasePeriod>
            name="leasePaymentsTable"
            columns={columns}
            data={formikProps.values.periods ?? []}
            manualPagination
            hideToolbar
            noRowsMessage="There is no corresponding data"
            canRowExpand={() => true}
            detailsPanel={{
              render: renderPayments,
              onExpand: noop,
              checkExpanded: (row: FormLeasePeriod, state: FormLeasePeriod[]) =>
                !!find(state, period => period.id === row.id),
              getRowId: (row: FormLeasePeriod) => row.id,
              icons: { open: <MdArrowDropDown size={24} />, closed: <MdArrowRight size={24} /> },
            }}
          />
        </Section>
      )}
    </Formik>
  );
};

export default PeriodsForm;
