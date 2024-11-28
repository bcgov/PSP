import { FormikProps } from 'formik';
import { find, noop, orderBy } from 'lodash';
import { useMemo } from 'react';
import { FaPlus } from 'react-icons/fa';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { Table } from '@/components/Table';
import { Claims, LeasePeriodStatusTypes } from '@/constants';
import { LeaseFormModel } from '@/features/leases/models';
import useDeepCompareMemo from '@/hooks/util/useDeepCompareMemo';
import { prettyFormatDate } from '@/utils';

import { defaultFormLeasePeriod, FormLeasePayment, FormLeasePeriod } from '../../models';
import PaymentsView from '../payments/PaymentsView';
import { getLeasePeriodColumns } from './periodColumns';

export interface IPeriodPaymentsViewProps {
  onEdit: (values: FormLeasePeriod) => void;
  onEditPayment: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePeriod) => void;
  onDeletePayment: (values: FormLeasePayment) => void;
  onSavePayment: (values: FormLeasePayment) => void;

  isReceivable?: boolean;
  lease?: LeaseFormModel;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
}

export const PeriodPaymentsView: React.FunctionComponent<
  React.PropsWithChildren<IPeriodPaymentsViewProps>
> = ({ onEdit, onEditPayment, onDelete, onDeletePayment, onSavePayment, isReceivable, lease }) => {
  const columns = useMemo(
    () =>
      getLeasePeriodColumns({
        onEdit,
        onDelete: onDelete,
      }),
    [onEdit, onDelete],
  );
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
        <PaymentsView
          onSave={onSavePayment}
          onEdit={onEditPayment}
          onDelete={onDeletePayment}
          period={matchingPeriod}
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
    <Section
      header={
        <SectionListHeader
          title="Payment Periods"
          addButtonText="Add a Period"
          addButtonIcon={<FaPlus size="2rem" />}
          claims={[Claims.LEASE_EDIT]}
          onAdd={() => onEdit(defaultFormLeasePeriod)}
        />
      }
    >
      {lastPaymentDate && <b>last payment received: {prettyFormatDate(lastPaymentDate)}</b>}
      <Table<FormLeasePeriod>
        name="leasePaymentsTable"
        columns={columns}
        className="no-zebra-rows"
        data={leaseForm.periods ?? []}
        manualPagination
        hideToolbar
        noRowsMessage="There is no corresponding data"
        canRowExpand={() => true}
        detailsPanel={{
          render: renderPayments,
          onExpand: noop,
          checkExpanded: (row: FormLeasePeriod, state: FormLeasePeriod[]) =>
            !!find(state, term => term.id === row.id),
          getRowId: (row: FormLeasePeriod) => row.id,
          icons: { open: <MdArrowDropDown size={24} />, closed: <MdArrowRight size={24} /> },
        }}
      />
    </Section>
  );
};

export default PeriodPaymentsView;
