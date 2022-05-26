import { ColumnWithProps, Table } from 'components/Table';
import { CellProps } from 'react-table';
import styled from 'styled-components';
import { formatNumber } from 'utils';

import { TableCaption } from '../../../SectionStyles';

export const LandMeasurementTable: React.FC<{ data?: any[] }> = ({ data = [] }) => {
  return (
    <>
      <TableCaption>Land measurement</TableCaption>
      <StyledTable
        name="land-measurements"
        hideHeaders
        hideToolbar
        noRowsMessage="No data available"
        columns={columns}
        data={data}
      ></StyledTable>
    </>
  );
};

const columns: ColumnWithProps<any>[] = [
  {
    accessor: 'value',
    align: 'right',
    Cell: ({ value }: CellProps<any, number>) => formatNumber(value, 0, 2),
  },
  { accessor: 'unit', align: 'left' },
];

const StyledTable = styled(Table)`
  &.table {
    .no-rows-message {
      align-items: flex-start;
    }
    .tbody {
      .tr {
        .td {
          min-height: 3.6rem;
        }
      }
    }
  }
` as typeof Table;
