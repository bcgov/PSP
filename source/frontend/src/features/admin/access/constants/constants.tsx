import * as React from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { AccessRequestStatus, AccessStatusDisplayMapper } from '@/constants/accessStatus';
import { FormAccessRequest } from '@/features/admin/access-request/models';

import { RowActions } from '../components/RowActions';

export const getAccessRequestColumns = (
  refresh: () => void,
): ColumnWithProps<FormAccessRequest>[] => [
  {
    Header: 'IDIR/BCeID',
    accessor: 'businessIdentifierValue',
    align: 'left',
    Cell: (props: CellProps<FormAccessRequest>) => {
      return (
        <Link to={`/admin/access/request/${props.row.original.id}`}>
          {props.row.original.businessIdentifierValue}
        </Link>
      );
    },
  },
  {
    Header: 'First name',
    accessor: 'firstName',
    align: 'left',
  },
  {
    Header: 'Last name',
    accessor: 'surname',
    align: 'left',
  },
  {
    Header: 'Email',
    accessor: 'email',
    align: 'left',
    minWidth: 200,
    maxWidth: 300,
  },
  {
    Header: 'Position',
    accessor: 'position',
    align: 'left',
  },
  {
    Header: 'Status',
    accessor: 'accessRequestStatusTypeCodeId',
    align: 'left',
    width: 100,
    Cell: (props: CellProps<FormAccessRequest>) => (
      <>
        {
          AccessStatusDisplayMapper[
            props.row.original.accessRequestStatusTypeCodeId as AccessRequestStatus
          ]
        }
      </>
    ),
  },
  {
    Header: 'Role',
    accessor: 'roleName',
    align: 'left',
    minWidth: 200,
  },
  {
    Header: 'MoTI Region',
    accessor: 'regionName',
    align: 'left',
    minWidth: 200,
  },
  {
    Header: 'Actions',
    Cell: (props: CellProps<FormAccessRequest>) => {
      return <RowActions {...props} refresh={refresh} />;
    },
    width: 75,
  },
];
