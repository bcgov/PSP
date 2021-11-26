import { ColumnWithProps, renderDate, renderMoney, renderPercentage } from 'components/Table';
import { ILeaseSecurityDeposit } from 'interfaces';
import moment from 'moment';
import { CellProps } from 'react-table';
import { formatMoney } from 'utils';

function renderInterestToDate({ row: { original } }: CellProps<ILeaseSecurityDeposit, string>) {
  const { annualInterestRate, amountPaid, depositDate } = original;
  const value = calculateInterest(annualInterestRate, amountPaid, depositDate);
  return formatMoney(value);
}

function renderTotalAmount({ row: { original } }: CellProps<ILeaseSecurityDeposit, string>) {
  const { annualInterestRate, amountPaid, depositDate } = original;
  const interest = calculateInterest(annualInterestRate, amountPaid, depositDate);
  return formatMoney(amountPaid + interest);
}

// As specified in Confluence, this calculates interest to date using the "simple interest" method.
// The formula looks like this: I (interest) = P (principal) x r (rate) x t (time periods)
function calculateInterest(
  annualInterestRate: number,
  amountPaid: number,
  depositDate: Date | moment.Moment | string,
): number {
  const rateValue = annualInterestRate / 100;

  // calculate number of months between today and deposit date
  const today = moment();
  const totalMonths = today.diff(depositDate, 'months');

  return amountPaid * (rateValue / 12) * totalMonths;
}

export const columns: ColumnWithProps<ILeaseSecurityDeposit>[] = [
  {
    Header: 'Deposit Type',
    accessor: 'securityDepositType',
    maxWidth: 50,
  },
  {
    Header: 'Description',
    accessor: 'description',
    minWidth: 150,
  },
  {
    Header: 'Amount Paid',
    accessor: 'amountPaid',
    align: 'right',
    maxWidth: 40,
    Cell: renderMoney,
  },
  {
    Header: 'Paid Date',
    accessor: 'depositDate',
    align: 'right',
    maxWidth: 50,
    Cell: renderDate,
  },
  {
    Header: 'Deposit holder',
    accessor: 'securityDepositHolderType', // TODO: Support "other holder type" when DB schema is updated
    maxWidth: 60,
  },
  {
    Header: 'Annual Interest %',
    accessor: 'annualInterestRate',
    align: 'right',
    maxWidth: 40,
    Cell: renderPercentage,
  },
  {
    // This is a derived field. See `renderInterestToDate` for details
    Header: 'Interest (approx)',
    align: 'right',
    maxWidth: 40,
    Cell: renderInterestToDate,
  },
  {
    // This is a derived field.
    Header: 'Total (with interest)',
    align: 'right',
    maxWidth: 40,
    Cell: renderTotalAmount,
  },
];
