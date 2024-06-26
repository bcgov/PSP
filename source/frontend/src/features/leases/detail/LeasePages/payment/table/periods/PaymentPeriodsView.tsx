import { FormikProps } from 'formik';
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

import { defaultFormLeaseTerm, FormLeasePayment, FormLeaseTerm } from '../../models';
import PaymentsView from '../payments/PaymentsView';
import { getLeaseTermColumns } from './columns';

export interface IPaymentPeriodsViewProps {
  onEdit: (values: FormLeaseTerm) => void;
  onEditPayment: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePeriod) => void;
  onDeletePayment: (values: FormLeasePayment) => void;
  onSavePayment: (values: FormLeasePayment) => void;
  onGenerate: () => void;
  isReceivable?: boolean;
  lease?: LeaseFormModel;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
}

export const PaymentPeriodsView: React.FunctionComponent<
  React.PropsWithChildren<IPaymentPeriodsViewProps>
> = ({
  onEdit,
  onEditPayment,
  onDelete,
  onDeletePayment,
  onSavePayment,
  onGenerate,
  isReceivable,
  lease,
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
        <PaymentsView
          onSave={onSavePayment}
          onEdit={onEditPayment}
          onDelete={onDeletePayment}
          payments={matchingTerm?.payments ?? []}
          isExercised={row?.statusTypeCode?.id === LeaseTermStatusTypes.EXERCISED}
          isGstEligible={row.isGstEligible}
          isReceivable={isReceivable}
          periodId={row.id ?? undefined}
        />
      );
    },
    [leaseForm],
  );

  return (
    <Section header="Payments by Term">
      {hasClaim(Claims.LEASE_ADD) && (
        <Button variant="secondary" onClick={() => onEdit(defaultFormLeaseTerm)}>
          Add a Term
        </Button>
      )}
      {lastPaymentDate && <b>last payment received: {prettyFormatDate(lastPaymentDate)}</b>}
      <Table<FormLeaseTerm>
        name="leasePaymentsTable"
        columns={columns}
        data={leaseForm.terms ?? []}
        manualPagination
        hideToolbar
        noRowsMessage="There is no corresponding data"
        canRowExpand={() => true}
        detailsPanel={{
          render: renderPayments,
          onExpand: noop,
          checkExpanded: (row: FormLeaseTerm, state: FormLeaseTerm[]) =>
            !!find(state, term => term.id === row.id),
          getRowId: (row: FormLeaseTerm) => row.id,
          icons: { open: <MdArrowDropDown size={24} />, closed: <MdArrowRight size={24} /> },
        }}
      />
    </Section>
  );
};

export default PaymentPeriodsView;
