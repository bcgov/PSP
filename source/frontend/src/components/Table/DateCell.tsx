import { CellProps } from 'react-table';
import { prettyFormatDate, prettyFormatDateTime } from 'utils';

export const DateCell = ({ cell: { value } }: CellProps<Date, string>) => prettyFormatDate(value);

export const DateTimeCell = ({ cell: { value } }: CellProps<Date, string>) =>
  prettyFormatDateTime(value);
