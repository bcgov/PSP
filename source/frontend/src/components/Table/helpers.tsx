import { CellProps } from 'react-table';

import Api_TypeCode from '@/models/api/TypeCode';
import { formatMoney, formatNumber, prettyFormatDate, stringToFragment } from '@/utils';

/**
 * These helper methods below provide ways to render common values on a Table cell;
 *  - dates
 *  - money
 *  - percentages
 */

export const renderDate = <T extends object>({
  cell: { value },
}: CellProps<T, string | Date | undefined>) => stringToFragment(prettyFormatDate(value));

export const renderMoney = <T extends object>({
  cell: { value },
}: CellProps<T, number | '' | undefined>) => stringToFragment(formatMoney(value));

export const renderPercentage = <T extends object>({ cell: { value } }: CellProps<T, number>) =>
  stringToFragment(`${formatNumber(value, 0, 2)}%`);

export const renderBooleanAsYesNo = ({ value }: CellProps<any, boolean | undefined>) =>
  stringToFragment(value ? 'Y' : 'N');

export const renderTypeCode = ({ value }: CellProps<any, Api_TypeCode<any> | undefined | null>) =>
  stringToFragment(value?.description ?? '');
