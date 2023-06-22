import { CellProps } from 'react-table';

import { pidFormatter } from '@/utils';

export const PidCell = ({ value }: CellProps<any, any>) => {
  return <>{pidFormatter(value)}</>;
};
