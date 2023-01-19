import { Button } from 'components/common/buttons/Button';
import Table from 'components/Table/Table';
import { Claims, LeaseTermStatusTypes } from 'constants/index';
import { Section } from 'features/mapSideBar/tabs/Section';
import { Formik, FormikProps } from 'formik';
import useDeepCompareMemo from 'hooks/useDeepCompareMemo';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { defaultFormLease, IFormLease, IFormLeasePayment } from 'interfaces';
import { defaultFormLeaseTerm, IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import { find, noop, orderBy } from 'lodash';
import * as React from 'react';
import { useMemo } from 'react';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';
import { prettyFormatDate } from 'utils';

import { PaymentsForm } from '../payments/PaymentsForm';
import { getLeaseTermColumns } from './columns';

export interface ITermsFormProps {
  onEdit: (values: IFormLeaseTerm) => void;
  onEditPayment: (values: IFormLeasePayment) => void;
  onDelete: (values: IFormLeaseTerm) => void;
  onDeletePayment: (values: IFormLeasePayment) => void;
  onSavePayment: (values: IFormLeasePayment) => void;
  isReceivable?: boolean;
  lease?: IFormLease;
  formikRef: React.RefObject<FormikProps<IFormLease>>;
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
  const leaseForm = { ...defaultFormLease, ...lease };

  //Get the most recent payment for display, if one exists.
  const allPayments = orderBy(
    (leaseForm.terms ?? []).flatMap(p => p.payments),
    'receivedDate',
    'desc',
  );
  const lastPaymentDate = allPayments.length ? prettyFormatDate(allPayments[0]?.receivedDate) : '';

  /** This is the payments subtable displayed for each term row. */
  const renderPayments = useDeepCompareMemo(
    () => (row: IFormLeaseTerm) => {
      return (
        <PaymentsForm
          onSave={onSavePayment}
          onEdit={onEditPayment}
          onDelete={onDeletePayment}
          nameSpace={`terms.${leaseForm.terms.indexOf(row)}`}
          isExercised={row?.statusTypeCode?.id === LeaseTermStatusTypes.EXERCISED}
          isGstEligible={row.isGstEligible}
          isReceivable={isReceivable}
          termId={row.id}
        />
      );
    },
    [leaseForm],
  );

  return (
    <Formik<IFormLease>
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
          <Table<IFormLeaseTerm>
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
              checkExpanded: (row: IFormLeaseTerm, state: IFormLeaseTerm[]) =>
                !!find(state, term => term.id === row.id),
              getRowId: (row: IFormLeaseTerm) => row.id,
              icons: { open: <MdArrowDropDown size={24} />, closed: <MdArrowRight size={24} /> },
            }}
          />
        </Section>
      )}
    </Formik>
  );
};

export default TermsForm;
