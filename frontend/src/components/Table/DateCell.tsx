import { ILeaseTerm } from 'interfaces';
import { CellProps } from 'react-table';
import { prettyFormatDate } from 'utils';

export const DateCell = ({ cell: { value } }: CellProps<ILeaseTerm, string>) =>
  prettyFormatDate(value);
