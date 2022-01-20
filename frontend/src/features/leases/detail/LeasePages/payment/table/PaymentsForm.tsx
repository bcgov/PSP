import { Button } from 'components/common/form';
import { TableProps } from 'components/Table/Table';
import { useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import { orderBy } from 'lodash';
import * as React from 'react';
import { Prompt } from 'react-router-dom';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';
import { prettyFormatDate } from 'utils';

import * as Styled from '../../../styles';
import * as PaymentStyles from '../styles';
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
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const columns = getColumns({ onEdit, onDelete: onDelete, gstConstant });

  //Get the most recent payment for display, if one exists.
  const allPayments = orderBy(
    (formikProps.values.terms ?? []).flatMap(p => p.payments),
    'receivedDate',
    'desc',
  );
  const lastPaymentDate = allPayments.length ? prettyFormatDate(allPayments[0]?.receivedDate) : '';

  return (
    <>
      <>
        <Prompt
          when={formikProps.dirty}
          message="You have made changes on this form. Do you wish to leave without saving?"
        />
        <PaymentStyles.StyledFormBody>
          <Styled.LeaseH6>Payments by Term</Styled.LeaseH6>

          <PaymentStyles.FullWidthInlineFlexDiv>
            <Button variant="secondary" onClick={() => setDisplayModal(true)}>
              Add a Term
            </Button>
            {lastPaymentDate && <b>last payment received: {prettyFormatDate(lastPaymentDate)}</b>}
          </PaymentStyles.FullWidthInlineFlexDiv>
          <PaymentStyles.StyledTable<React.FC<TableProps<IFormLeaseTerm>>>
            name="securityDepositsTable"
            columns={columns}
            data={formikProps.values.terms ?? []}
            manualPagination={false}
            hideToolbar={true}
            noRowsMessage="There is no corresponding data"
          />
        </PaymentStyles.StyledFormBody>
      </>
    </>
  );
};

export default PaymentsForm;
