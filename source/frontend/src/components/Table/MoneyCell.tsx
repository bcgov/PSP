import { CellProps } from 'react-table';

import { formatMoney } from '@/utils';

export const MoneyCell = ({ cell: { value } }: CellProps<any, number | '' | undefined>) => (
  <div>{value === undefined || value === '' ? '' : formatMoney(value)}</div>
);
