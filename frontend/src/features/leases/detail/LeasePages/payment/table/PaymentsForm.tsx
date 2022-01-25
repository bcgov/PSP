import { Button } from 'components/common/form';
import { TableProps } from 'components/Table/Table';
import Claims from 'constants/claims';
import { useFormikContext } from 'formik';
import useDeepCompareMemo from 'hooks/useDeepCompareMemo';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { IFormLease } from 'interfaces';
import { IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import _, { noop, orderBy } from 'lodash';
import * as React from 'react';
import { useMemo } from 'react';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';
import { prettyFormatDate } from 'utils';

import * as Styled from '../../../styles';
import * as PaymentStyles from '../styles';
import { ActualsForm } from './ActualsForm';
import { getColumns } from './columns';

export interface IPaymentsFormProps {
  onEdit: (values: IFormLeaseTerm) => void;
  onDelete: (values: IFormLeaseTerm) => void;
  setDisplayModal: (displayModal: boolean) => void;
}

export const PaymentsForm: React.FunctionComponent<IPaymentsFormProps> = ({
  onEdit,
  onDelete,
  setDisplayModal,
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

  const renderPayments = useDeepCompareMemo(
    () => (row: IFormLeaseTerm) => {
      return (
        <ActualsForm
          onEdit={noop}
          onDelete={noop}
          setDisplayModal={noop}
          nameSpace={`terms.${formikProps.values.terms.indexOf(row)}`}
          isExercised={row.isTermExercised}
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
          {hasClaim(Claims.LEASE_EDIT) && (
            <Button variant="secondary" onClick={() => setDisplayModal(true)}>
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
              !!_.find(state, term => term.id === row.id),
            getRowId: (row: IFormLeaseTerm) => row.id,
            icons: { open: <MdArrowDropDown size={24} />, closed: <MdArrowRight size={24} /> },
          }}
        />
      </PaymentStyles.StyledFormBody>
    </>
  );
};

export default PaymentsForm;
