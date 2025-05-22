import { CellProps } from 'react-table';

import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps } from '@/components/Table';

import { ManagementActivitySearchResultModel } from '../../models/ManagementActivitySearchResultModel';

export const columns: ColumnWithProps<ManagementActivitySearchResultModel>[] = [
  // {
  //   Header: 'Management file #',
  //   accessor: 'managementFileId',
  //   align: 'center',
  //   clickable: true,
  //   sortable: true,
  //   width: 10,
  //   maxWidth: 20,
  //   Cell: (props: CellProps<ManagementActivitySearchResultModel>) => {
  //     const { hasClaim } = useKeycloakWrapper();
  //     if (hasClaim(Claims.MANAGEMENT_VIEW)) {
  //       return (
  //         <Link to={`/mapview/sidebar/management/${props.row.original.managementFileId}`}>
  //           M-{props.row.original.managementFileId.toString()}
  //         </Link>
  //       );
  //     }
  //     return stringToFragment(props.row.original.managementFileId.toString());
  //   },
  // },
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
