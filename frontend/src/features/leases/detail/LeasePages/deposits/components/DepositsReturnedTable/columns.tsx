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
    maxWidth: 50,
    Cell: renderDate,
  },
  {
    Header: 'Total (with interest)',
    accessor: 'depositTotal',
    maxWidth: 40,
    Cell: renderMoney,
  },
  {
    Header: 'Claims against Deposit',
    accessor: 'claimsAgainst',
    maxWidth: 50,
    Cell: renderMoney,
  },
  {
    Header: 'Returned Amount',
    accessor: 'returnAmount',
    maxWidth: 50,
    Cell: renderMoney,
  },
  {
    Header: 'Return Date',
    accessor: 'returnDate',
    maxWidth: 50,
    Cell: renderDate,
  },
  {
    Header: 'Cheque Number',
    accessor: 'chequeNumber',
    maxWidth: 40,
  },
  {
    Header: 'Payee Name',
    accessor: 'payeeName',
    maxWidth: 70,
  },
];
