import { ColumnWithProps, renderDate, renderMoney, renderPercentage } from 'components/Table';
import { ILeaseSecurityDeposit } from 'interfaces';

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
    maxWidth: 40,
    Cell: renderMoney,
  },
  {
    Header: 'Paid Date',
    accessor: 'depositDate',
    maxWidth: 50,
    Cell: renderDate,
  },
  {
    Header: 'Deposit holder',
    accessor: 'securityDepositHolderType',
    maxWidth: 50,
  },
  {
    Header: 'Annual Interest %',
    accessor: 'annualInterestRate',
    maxWidth: 40,
    Cell: renderPercentage,
  },
  {
    Header: 'Interest',
    maxWidth: 40,
    Cell: () => '[ TODO ]', // TODO: Implement derived field here
  },
  {
    Header: 'Total',
    accessor: 'totalAmount',
    maxWidth: 40,
    Cell: renderMoney,
  },
];
