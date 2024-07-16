import { CellProps } from 'react-table';

import TooltipIcon from '@/components/common/TooltipIcon';
import {
  ColumnWithProps,
  renderBooleanAsYesNo,
  renderMoney,
  renderTypeCode,
} from '@/components/Table';
import { ApiGen_CodeTypes_LeasePaymentCategoryTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentCategoryTypes';
import { formatMoney, stringToFragment } from '@/utils';

import { LeasePeriodByCategoryProjection } from '../../models';
import { renderExpectedTotal } from './periodColumns';

export const getLeaseVariablePeriodColumns =
  (): ColumnWithProps<LeasePeriodByCategoryProjection>[] => [
    {
      Header: 'Category',
      accessor: 'category',
      align: 'left',
      maxWidth: 70,
      Cell: renderCategory(),
    },
    {
      Header: 'Payment Fequency',
      accessor: 'leasePmtFreqTypeCode',
      align: 'left',
      maxWidth: 40,
      Cell: renderTypeCode,
    },
    {
      Header: 'Expected Payment ($)',
      accessor: 'paymentAmount',
      align: 'right',
      maxWidth: 90,
      Cell: renderMoney,
    },
    {
      Header: 'GST?',
      accessor: 'isGstEligible',
      align: 'left',
      maxWidth: 30,
      Cell: renderBooleanAsYesNo,
    },
    {
      Header: 'GST ($)',
      accessor: 'gstAmount',
      align: 'right',
      maxWidth: 40,
      Cell: renderMoney,
    },
    {
      Header: 'Expected total ($)',
      id: 'expectedTotal',
      align: 'right',
      maxWidth: 50,
      Cell: renderExpectedTotal(),
    },
    {
      Header: 'Actual total ($)',
      id: 'actualTotal',
      align: 'right',
      maxWidth: 50,
      Cell: renderActualTotal,
    },
    {
      id: 'id',
      align: 'left',
      maxWidth: 150,
    },
  ];

const renderCategory = () =>
  function ({ row: { original } }: CellProps<LeasePeriodByCategoryProjection, string>) {
    let rowProps: { categoryName: string; tooltipText: string };
    switch (original.category) {
      case ApiGen_CodeTypes_LeasePaymentCategoryTypes.ADDL:
        rowProps = {
          categoryName: 'Additional Rent',
          tooltipText: 'Operating Expenses and Taxes Payable by the Tenant.',
        };
        break;
      case ApiGen_CodeTypes_LeasePaymentCategoryTypes.VBL:
        rowProps = {
          categoryName: 'Variable Rent',
          tooltipText: 'Percentage Rent payable by Tenant.',
        };
        break;
      default:
        rowProps = {
          categoryName: 'Base Rent',
          tooltipText:
            'Fixed Amount of Rent per Payment Payment Period, excluding Operating Expenses.',
        };
    }
    return (
      <>
        {rowProps.categoryName}
        <TooltipIcon
          toolTipId={`variable-period-${original.category}`}
          toolTip={rowProps.tooltipText}
        />
      </>
    );
  };

function renderActualTotal({
  row: { original },
}: CellProps<LeasePeriodByCategoryProjection, string>) {
  const total = formatMoney(
    (original.payments ?? [])
      .filter(p => p.leasePaymentCategoryTypeCode?.id === original.category)
      .reduce((sum: number, p) => (sum += p.amountTotal as number), 0),
  );
  return stringToFragment(original.isTermExercised ? total : '-');
}
