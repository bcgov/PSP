import { Formik, FormikProps } from 'formik';
import { find, noop, orderBy } from 'lodash';
import { useMemo } from 'react';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';

import { Button } from '@/components/common/buttons';
import { Section } from '@/components/common/Section/Section';
import { Table } from '@/components/Table';
import { Claims, LeaseTermStatusTypes } from '@/constants';
import { LeaseFormModel } from '@/features/leases/models';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useDeepCompareMemo from '@/hooks/util/useDeepCompareMemo';
import { prettyFormatDate } from '@/utils';

import { defaultFormLeaseTerm, FormLeasePayment, FormLeaseTerm } from '../../models';
import PaymentsForm from '../payments/PaymentsForm';
import { getLeaseTermColumns } from './columns';

export interface ITermsFormProps {
  onEdit: (values: FormLeaseTerm) => void;
  onEditPayment: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeaseTerm) => void;
  onDeletePayment: (values: FormLeasePayment) => void;
  onSavePayment: (values: FormLeasePayment) => void;
  isReceivable?: boolean;
  lease?: LeaseFormModel;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
}

export const TermsForm: React.FunctionComponent<React.PropsWithChildren<ITermsFormProps>> = ({
  onEdit,
  onEditPayment,
  onDelete,
  onDeletePayment,
  onSavePayment,
  isReceivable,
  lease,
  formikRef,
}) => {
  const columns = useMemo(
    () => getLeaseTermColumns({ onEdit, onDelete: onDelete }),
    [onEdit, onDelete],
  );
  const { hasClaim } = useKeycloakWrapper();
  const leaseForm = { ...new LeaseFormModel(), ...lease };

  //Get the most recent payment for display, if one exists.
  const allPayments = orderBy(
    (leaseForm.terms ?? []).flatMap(p => p.payments),
    'receivedDate',
    'desc',
  );
  const lastPaymentDate = allPayments.length ? allPayments[0]?.receivedDate : '';

  /** This is the payments subtable displayed for each term row. */
  const renderPayments = useDeepCompareMemo(
    () => (row: FormLeaseTerm) => {
      const matchingTerm = leaseForm.terms.find(t => t.id === row.id);
      return (
        <PaymentsForm
          onSave={onSavePayment}
          onEdit={onEditPayment}
          onDelete={onDeletePayment}
          nameSpace={`terms.${matchingTerm ? leaseForm.terms.indexOf(matchingTerm) : 0}`}
          isExercised={row?.statusTypeCode?.id === LeaseTermStatusTypes.EXERCISED}
          isGstEligible={row.isGstEligible}
          isReceivable={isReceivable}
          termId={row.id ?? undefined}
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
            data={formikProps.values.terms ?? []}
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
      )}
    </Formik>
  );
};

export default TermsForm;
