import * as React from 'react';
import { CellProps } from 'react-table';

import { ColumnWithProps, Table } from '@/components/Table';
import { ApiGen_Concepts_ExpropriationPaymentItem } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPaymentItem';
import { stringToFragment } from '@/utils/columnUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

export interface IExpropriationPaymentItemsTableProps {
  paymentItems: ApiGen_Concepts_ExpropriationPaymentItem[];
}

const columns: ColumnWithProps<ApiGen_Concepts_ExpropriationPaymentItem>[] = [
  {
    Header: 'Item',
    align: 'left',
    sortable: false,
    width: 40,
    Cell: (cellProps: CellProps<ApiGen_Concepts_ExpropriationPaymentItem>) => {
      return stringToFragment(cellProps.row.original.paymentItemType?.description);
    },
  },
  {
    Header: 'Amount',
    align: 'right',
    sortable: false,
    minWidth: 20,
    maxWidth: 20,
    Cell: (cellProps: CellProps<ApiGen_Concepts_ExpropriationPaymentItem>) => {
      return stringToFragment(formatMoney(cellProps.row.original.pretaxAmount));
    },
  },
  {
    Header: 'GST',
    align: 'right',
    sortable: false,
    minWidth: 20,
    maxWidth: 20,
    Cell: (cellProps: CellProps<ApiGen_Concepts_ExpropriationPaymentItem>) => {
      return stringToFragment(formatMoney(cellProps.row.original.taxAmount));
    },
  },
  {
    Header: 'Total',
    align: 'right',
    sortable: false,
    minWidth: 20,
    maxWidth: 20,
    Cell: (cellProps: CellProps<ApiGen_Concepts_ExpropriationPaymentItem>) => {
      return stringToFragment(formatMoney(cellProps.row.original.totalAmount));
    },
  },
];

const ExpropriationPaymentItemsTable: React.FunctionComponent<
  React.PropsWithChildren<IExpropriationPaymentItemsTableProps>
> = ({ paymentItems }) => {
  return (
    <>
      <Table<ApiGen_Concepts_ExpropriationPaymentItem>
        name="paymentItems"
        columns={columns}
        hideToolbar
        data={paymentItems}
        noRowsMessage="No payment items found"
      />
    </>
  );
};

export default ExpropriationPaymentItemsTable;
