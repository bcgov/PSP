import { CellProps } from 'react-table';

import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps } from '@/components/Table';

import { ManagementActivitySearchResultModel } from '../../models/ManagementActivitySearchResultModel';

export const columns: ColumnWithProps<ManagementActivitySearchResultModel>[] = [
  {
    Header: 'File name',
    accessor: 'fileName',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'Historical File #',
    accessor: 'legacyFileNum',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Project',
    accessor: 'project',
    align: 'left',
    clickable: true,
    sortable: false,
    width: 20,
    maxWidth: 40,
    Cell: (props: CellProps<ManagementActivitySearchResultModel>) => {
      const project = props.row.original.project;

      return <>{[project?.code, project?.description].filter(Boolean).join(' ')}</>;
    },
  },
  {
    Header: 'Civic Address / PID / PIN',
    accessor: 'properties',
    align: 'left',
    Cell: (props: CellProps<ManagementActivitySearchResultModel>) => {
      return (
        <ExpandableFileProperties
          properties={props.row.original.properties}
          maxDisplayCount={2}
        ></ExpandableFileProperties>
      );
    },
  },
  {
    Header: 'Type',
    accessor: 'activityType',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Sub-type',
    accessor: 'activitySubType',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Status',
    accessor: 'activityStatus',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
];
