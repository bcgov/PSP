import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdReceipt } from 'react-icons/md';

import { TableProps } from '@/components/Table';
import { Claims } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { defaultFormLeasePayment, FormLeasePayment } from '../../models';
import * as PaymentStyles from '../../styles';
import { getActualsColumns } from './paymentsColumns';

export interface IPaymentsViewProps {
  onEdit: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePayment) => void;
  onSave: (values: FormLeasePayment) => void;
  isExercised?: boolean;
  isGstEligible?: boolean;
  isReceivable?: boolean;
  periodId?: number;
  payments: FormLeasePayment[];
}

export const PaymentsView: React.FunctionComponent<React.PropsWithChildren<IPaymentsViewProps>> = ({
  onEdit,
  onDelete,
  onSave,
  isExercised,
  isGstEligible,
  isReceivable,
  periodId,
  payments,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const columns = useMemo(
    () =>
      getActualsColumns({
        onEdit,
        onDelete,
        isReceivable,
        isGstEligible,
        onSave,
        payments,
      }),
    [onEdit, onDelete, isReceivable, isGstEligible, onSave, payments],
  );

  return (
    <Row>
      <PaymentStyles.InlineCol md={2}>
        <MdReceipt className="receipt" size={24} />
        <PaymentStyles.FlexColDiv>
          <b>{isReceivable ? 'Payments Received' : 'Payments Sent'}</b>
          {isExercised && hasClaim(Claims.LEASE_ADD) && (
            <PaymentStyles.AddActualButton
              onClick={() => onEdit({ ...defaultFormLeasePayment, leasePeriodId: periodId ?? 0 })}
            >
              Record a Payment
            </PaymentStyles.AddActualButton>
          )}
        </PaymentStyles.FlexColDiv>
      </PaymentStyles.InlineCol>
      <Col md={10}>
        {!!payments?.length && isExercised ? (
          <>
            <PaymentStyles.StyledPaymentTable<React.FC<TableProps<FormLeasePayment>>>
              name="securityDepositsTable"
              columns={columns}
              data={payments ?? []}
              manualPagination
              hideToolbar
              noRowsMessage="There is no corresponding data"
              footer={true}
            />
          </>
        ) : (
          <PaymentStyles.WarningTextBox>
            {!payments?.length && <p>There are no recorded payments for this period.</p>}
            {!isExercised && <p>Period must be exercised to add payments.</p>}
          </PaymentStyles.WarningTextBox>
        )}
      </Col>
    </Row>
  );
};

export default PaymentsView;
