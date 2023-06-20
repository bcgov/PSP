import { getIn, useFormikContext } from 'formik';
import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdReceipt } from 'react-icons/md';

import { TableProps } from '@/components/Table';
import { Claims } from '@/constants';
import { LeaseFormModel } from '@/features/leases/models';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { withNameSpace } from '@/utils/formUtils';

import { defaultFormLeasePayment, FormLeasePayment } from '../../models';
import * as PaymentStyles from '../../styles';
import { getActualsColumns } from './paymentsColumns';

export interface IPaymentsFormProps {
  onEdit: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePayment) => void;
  onSave: (values: FormLeasePayment) => void;
  nameSpace?: string;
  isExercised?: boolean;
  isGstEligible?: boolean;
  isReceivable?: boolean;
  termId?: number;
}

export const PaymentsForm: React.FunctionComponent<React.PropsWithChildren<IPaymentsFormProps>> = ({
  onEdit,
  onDelete,
  onSave,
  nameSpace,
  isExercised,
  isGstEligible,
  isReceivable,
  termId,
}) => {
  const formikProps = useFormikContext<LeaseFormModel>();
  const { hasClaim } = useKeycloakWrapper();
  const field = useMemo(() => withNameSpace(nameSpace, 'payments'), [nameSpace]);
  const payments: FormLeasePayment[] = getIn(formikProps.values, field);
  const columns = useMemo(
    () =>
      getActualsColumns({
        onEdit,
        onDelete,
        isReceivable,
        isGstEligible,
        onSave,
        nameSpace: withNameSpace(nameSpace, 'payments'),
      }),
    [onEdit, onDelete, isReceivable, isGstEligible, onSave, nameSpace],
  );

  return (
    <Row>
      <PaymentStyles.InlineCol md={2}>
        <MdReceipt className="receipt" size={24} />
        <PaymentStyles.FlexColDiv>
          <b>{isReceivable ? 'Payments Received' : 'Payments Sent'}</b>
          {isExercised && hasClaim(Claims.LEASE_ADD) && (
            <PaymentStyles.AddActualButton
              onClick={() => onEdit({ ...defaultFormLeasePayment, leaseTermId: termId ?? 0 })}
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
            {!payments?.length && <p>There are no recorded payments for this term.</p>}
            {!isExercised && <p>Term must be exercised to add payments.</p>}
          </PaymentStyles.WarningTextBox>
        )}
      </Col>
    </Row>
  );
};

export default PaymentsForm;
