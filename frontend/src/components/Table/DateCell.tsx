import { CellProps } from 'react-table';
import { prettyFormatDate } from 'utils';

export const DateCell = ({ cell: { value } }: CellProps<Date, string>) => prettyFormatDate(value);
