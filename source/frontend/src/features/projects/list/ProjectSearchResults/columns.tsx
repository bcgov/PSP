import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { prettyFormatUTCDate } from '@/utils';

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
      <Link to={`/mapview/sidebar/project/${props.row.original.id}`}>
        {props.row.original.code}
      </Link>
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
      <Link to={`/mapview/sidebar/project/${props.row.original.id}`}>
        {props.row.original.description}
      </Link>
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
    Cell: (props: CellProps<ProjectSearchResultModel>) => {
      const updateDate = props.row.original.lastUpdatedDate;
      return (
        <>
          <span>{prettyFormatUTCDate(updateDate)}</span>
        </>
      );
    },
  },
];
