import { CellProps } from 'react-table';
import { prettyFormatDate, prettyFormatDateTime, stringToFragment } from 'utils';

export const DateCell = ({ cell: { value } }: CellProps<any, string | Date | undefined>) =>
  stringToFragment(prettyFormatDate(value));

export const DateTimeCell = ({ cell: { value } }: CellProps<any, string | Date | undefined>) =>
  stringToFragment(prettyFormatDateTime(value));
