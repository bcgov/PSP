import { CellProps } from 'react-table';

import { ExternalLink } from '@/components/common/ExternalLink';
import { ColumnWithProps } from '@/components/Table';
import { UtcDateCell } from '@/components/Table/DateCell';

import { ProjectSearchResultModel } from './models';

export const columns: ColumnWithProps<ProjectSearchResultModel>[] = [
  {
    Header: 'Project #',
    accessor: 'code',
    align: 'center',
    clickable: false,
    sortable: true,
    width: 5,
    maxWidth: 20,
    Cell: (props: CellProps<ProjectSearchResultModel>) => (
      <ExternalLink to={`/mapview/sidebar/project/${props.row.original.id}`}>
        {props.row.original.code}
      </ExternalLink>
    ),
  },
  {
    Header: 'Project name',
    accessor: 'description',
    align: 'left',
    clickable: false,
    sortable: true,
    width: 45,
    maxWidth: 45,
    Cell: (props: CellProps<ProjectSearchResultModel>) => (
      <ExternalLink to={`/mapview/sidebar/project/${props.row.original.id}`}>
        {props.row.original.description}
      </ExternalLink>
    ),
  },
  {
    Header: 'Region',
    accessor: 'region',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 20,
    maxWidth: 20,
  },
  {
    Header: 'Status',
    accessor: 'status',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 20,
    maxWidth: 20,
  },
  {
    Header: 'Last updated by',
    accessor: 'lastUpdatedBy',
    align: 'left',
    clickable: false,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Updated date',
    accessor: 'lastUpdatedDate',
    align: 'left',
    sortable: true,
    width: 10,
    Cell: UtcDateCell,
  },
];
