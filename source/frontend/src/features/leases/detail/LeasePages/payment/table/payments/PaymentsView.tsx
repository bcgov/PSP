import { useMemo } from 'react';
import { FaPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableProps } from '@/components/Table';
import Claims from '@/constants/claims';
import { ApiGen_CodeTypes_LeasePaymentCategoryTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentCategoryTypes';

import {
  defaultFormLeasePayment,
  FormLeasePayment,
  FormLeasePeriod,
  LeasePeriodByCategoryProjection,
} from '../../models';
import * as PaymentStyles from '../../styles';
import { getLeaseVariablePeriodColumns } from '../periods/variablePeriodColumns';
import { getActualsColumns } from './paymentsColumns';

export interface IPaymentsViewProps {
  onEdit: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePayment) => void;
  onSave: (values: FormLeasePayment) => void;
  isExercised?: boolean;
  isGstEligible?: boolean;
  isReceivable?: boolean;
  periodId?: number;
  period: FormLeasePeriod | undefined;
}

export const PaymentsView: React.FunctionComponent<React.PropsWithChildren<IPaymentsViewProps>> = ({
  onEdit,
  onDelete,
  onSave,
  isExercised,
  isGstEligible,
  isReceivable,
  period,
}) => {
  const variablePaymentColumns = getLeaseVariablePeriodColumns();
  const variablePaymentData: LeasePeriodByCategoryProjection[] = [
    new LeasePeriodByCategoryProjection(period, ApiGen_CodeTypes_LeasePaymentCategoryTypes.BASE),
    new LeasePeriodByCategoryProjection(period, ApiGen_CodeTypes_LeasePaymentCategoryTypes.ADDL),
    new LeasePeriodByCategoryProjection(period, ApiGen_CodeTypes_LeasePaymentCategoryTypes.VBL),
  ];
  const columns = useMemo(
    () =>
      getActualsColumns({
        onEdit,
        onDelete,
        isReceivable,
        isGstEligible,
        onSave,
        payments: period?.payments ?? [],
      }),
    [onEdit, onDelete, isReceivable, isGstEligible, onSave, period],
  );

  return (
    <>
      {period?.isVariable === 'true' && (
        <Section header={<StyledSectionListHeader claims={[]} title="Rent Breakdown" />}>
          <PaymentStyles.StyledPaymentTable<React.FC<TableProps<LeasePeriodByCategoryProjection>>>
            name="variablePeriodTable"
            columns={variablePaymentColumns}
            data={variablePaymentData ?? []}
            manualPagination
            hideToolbar
          />
        </Section>
      )}
      <Section
        header={
          <StyledSectionListHeader
            title="Payments"
            addButtonText="Add a Payment"
            onAdd={
              isExercised
                ? () => onEdit({ ...defaultFormLeasePayment, leasePeriodId: period?.id ?? 0 })
                : undefined
            }
            claims={[Claims.LEASE_EDIT]}
            addButtonIcon={<FaPlus size={'2rem'} />}
          />
        }
      >
        {!!period?.payments?.length && isExercised ? (
          <>
            <PaymentStyles.StyledPaymentTable<React.FC<TableProps<FormLeasePayment>>>
              name="paymentsTable"
              columns={columns}
              data={period?.payments ?? []}
              manualPagination
              hideToolbar
              noRowsMessage="There is no corresponding data"
              footer={true}
            />
          </>
        ) : (
          <PaymentStyles.WarningTextBox>
            {!period?.payments?.length && <p>There are no recorded payments for this period.</p>}
            {!isExercised && <p>Period must be exercised to add payments.</p>}
          </PaymentStyles.WarningTextBox>
        )}
      </Section>
    </>
  );
};

export default PaymentsView;

const StyledSectionListHeader = styled(SectionListHeader)`
  font-size: 1.8rem;
`;
