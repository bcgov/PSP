import { ColumnWithProps, renderDate, renderMoney } from 'components/Table';
import { ILeaseSecurityDepositReturn } from 'interfaces';

export const columns: ColumnWithProps<ILeaseSecurityDepositReturn>[] = [
  {
    Header: 'Deposit Type',
    accessor: 'securityDepositType',
    maxWidth: 50,
  },
  {
    Header: 'Termination or Surrender Date',
    accessor: 'terminationDate',
    align: 'right',
    maxWidth: 50,
    Cell: renderDate,
  },
  {
    Header: 'Total (with interest)',
    accessor: 'depositTotal',
    align: 'right',
    maxWidth: 40,
    Cell: renderMoney,
  },
  {
    Header: 'Claims against Deposit',
    accessor: 'claimsAgainst',
    align: 'right',
    maxWidth: 50,
    Cell: renderMoney,
  },
  {
    Header: 'Returned Amount',
    accessor: 'returnAmount',
    align: 'right',
    maxWidth: 50,
    Cell: renderMoney,
  },
  {
    Header: 'Return Date',
    accessor: 'returnDate',
    align: 'right',
    maxWidth: 50,
    Cell: renderDate,
  },
  {
    Header: 'Cheque Number',
    accessor: 'chequeNumber',
    align: 'right',
    maxWidth: 40,
  },
  {
    Header: 'Payee Name',
    accessor: 'payeeName',
    maxWidth: 70,
  },
];
