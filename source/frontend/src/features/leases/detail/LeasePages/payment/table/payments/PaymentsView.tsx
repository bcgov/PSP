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
  FormLeasePeriodWithCategory,
} from '../../models';
import * as PaymentStyles from '../../styles';
import { getLeaseVariablePeriodColumns } from '../periods/columns';
import { getActualsColumns } from './paymentsColumns';

export interface IPaymentsViewProps {
  onEdit: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePayment) => void;
  onSave: (values: FormLeasePayment) => void;
  isExercised?: boolean;
  isGstEligible?: boolean;
  isReceivable?: boolean;
  periodId?: number;
  period: FormLeasePeriod;
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
  const variablePaymentData: FormLeasePeriodWithCategory[] = [
    { category: ApiGen_CodeTypes_LeasePaymentCategoryTypes.BASE, ...period },
    {
      category: ApiGen_CodeTypes_LeasePaymentCategoryTypes.ADDL,
      ...period,
      isGstEligible: period?.isAdditionalRentGstEligible,
      paymentAmount: period?.additionalRentPaymentAmount ?? 0,
      leasePmtFreqTypeCode: period?.additionalRentFreqTypeCode,
      gstAmount: period.additionalRentGstAmount,
    },
    {
      category: ApiGen_CodeTypes_LeasePaymentCategoryTypes.VBL,
      ...period,
      isGstEligible: period?.isVariableRentGstEligible,
      paymentAmount: period?.variableRentPaymentAmount ?? 0,
      leasePmtFreqTypeCode: period?.variableRentFreqTypeCode,
      gstAmount: period.variableRentGstAmount,
    },
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
        <Section className="ml-10 p-0">
          <PaymentStyles.StyledPaymentTable<React.FC<TableProps<FormLeasePeriodWithCategory>>>
            name="variablePeriodTable"
            columns={variablePaymentColumns}
            data={variablePaymentData ?? []}
            manualPagination
            hideToolbar
          />
        </Section>
      )}
      <Section
        className="ml-20 p-0"
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
