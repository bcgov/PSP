import { Button } from 'components/common/buttons/Button';
import { TableProps } from 'components/Table/Table';
import { Claims, LeaseTermStatusTypes } from 'constants/index';
import { useFormikContext } from 'formik';
import useDeepCompareMemo from 'hooks/useDeepCompareMemo';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { IFormLease, IFormLeasePayment } from 'interfaces';
import { defaultFormLeaseTerm, IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import { find, noop, orderBy } from 'lodash';
import * as React from 'react';
import { useMemo } from 'react';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';
import { prettyFormatDate } from 'utils';

import * as Styled from '../../../../styles';
import * as PaymentStyles from '../../styles';
import { PaymentsForm } from '../payments/PaymentsForm';
import { getColumns } from './columns';

export interface ITermsFormProps {
  onEdit: (values: IFormLeaseTerm) => void;
  onEditPayment: (values: IFormLeasePayment) => void;
  onDelete: (values: IFormLeaseTerm) => void;
  onDeletePayment: (values: IFormLeasePayment) => void;
  onSavePayment: (values: IFormLeasePayment) => void;
  isReceivable?: boolean;
}

export const TermsForm: React.FunctionComponent<ITermsFormProps> = ({
  onEdit,
  onEditPayment,
  onDelete,
  onDeletePayment,
  onSavePayment,
  isReceivable,
}) => {
  const formikProps = useFormikContext<IFormLease>();
  const columns = useMemo(() => getColumns({ onEdit, onDelete: onDelete }), [onEdit, onDelete]);
  const { hasClaim } = useKeycloakWrapper();

  //Get the most recent payment for display, if one exists.
  const allPayments = orderBy(
    (formikProps.values.terms ?? []).flatMap(p => p.payments),
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
          nameSpace={`terms.${formikProps.values.terms.indexOf(row)}`}
          isExercised={row?.statusTypeCode?.id === LeaseTermStatusTypes.EXERCISED}
          isGstEligible={row.isGstEligible}
          isReceivable={isReceivable}
          termId={row.id}
        />
      );
    },
    [formikProps.values.terms],
  );

  return (
    <>
      <PaymentStyles.StyledFormBody>
        <Styled.LeaseH6>Payments by Term</Styled.LeaseH6>

        <PaymentStyles.FullWidthInlineFlexDiv>
          {hasClaim(Claims.LEASE_ADD) && (
            <Button variant="secondary" onClick={() => onEdit(defaultFormLeaseTerm)}>
              Add a Term
            </Button>
          )}
          {lastPaymentDate && <b>last payment received: {prettyFormatDate(lastPaymentDate)}</b>}
        </PaymentStyles.FullWidthInlineFlexDiv>
        <PaymentStyles.StyledTable<React.FC<TableProps<IFormLeaseTerm>>>
          name="leasePaymentsTable"
          columns={columns}
          data={formikProps.values.terms ?? []}
          manualPagination={false}
          hideToolbar={true}
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
      </PaymentStyles.StyledFormBody>
    </>
  );
};

export default TermsForm;
