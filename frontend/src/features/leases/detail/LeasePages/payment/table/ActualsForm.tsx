import { TableProps } from 'components/Table/Table';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { ILeasePayment } from 'interfaces/ILeasePayment';
import * as React from 'react';
import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdReceipt } from 'react-icons/md';
import { withNameSpace } from 'utils/formUtils';

import * as PaymentStyles from '../styles';
import { getActualsColumns } from './actualsColumns';

export interface IActualsFormProps {
  onEdit: (values: ILeasePayment) => void;
  onDelete: (values: ILeasePayment) => void;
  setDisplayModal: (displayModal: boolean) => void;
  nameSpace?: string;
  isExercised?: boolean;
}

export const ActualsForm: React.FunctionComponent<IActualsFormProps> = ({
  onEdit,
  onDelete,
  setDisplayModal,
  nameSpace,
  isExercised,
}) => {
  const formikProps = useFormikContext<IFormLease>();
  const field = withNameSpace(nameSpace, 'payments');
  const payments = getIn(formikProps.values, field);
  const isReceivable = formikProps.values.paymentReceivableType?.id === 'RCVBL';
  const columns = useMemo(() => getActualsColumns({ onEdit, onDelete, isReceivable }), [
    onDelete,
    onEdit,
    isReceivable,
  ]);

  return (
    <Row>
      <PaymentStyles.InlineCol md={2}>
        <MdReceipt className="receipt" size={24} />
        <div>
          <b>{isReceivable ? 'Payments Received' : 'Payments Sent'}</b>
        </div>
      </PaymentStyles.InlineCol>
      <Col md={10}>
        {!!payments?.length && isExercised ? (
          <>
            <PaymentStyles.StyledPaymentTable<React.FC<TableProps<ILeasePayment>>>
              name="securityDepositsTable"
              columns={columns}
              data={payments ?? []}
              manualPagination={false}
              hideToolbar={true}
              noRowsMessage="There is no corresponding data"
              footer={true}
            />
          </>
        ) : (
          <PaymentStyles.WarningTextBox>
            <p>There are no recorded payments for this term.</p>
            {!isExercised && <p>Term must be exercised to add payments.</p>}
          </PaymentStyles.WarningTextBox>
        )}
      </Col>
    </Row>
  );
};

export default ActualsForm;
