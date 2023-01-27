import { ColumnWithProps } from 'components/Table';
import { IOrganization, IOrganizationRecord } from 'interfaces';
import * as React from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

export const columnDefinitions: ColumnWithProps<IOrganizationRecord>[] = [
  {
    Header: 'Organization name',
    accessor: 'name',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Short name',
    accessor: 'code',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Description',
    accessor: 'description',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Parent Organization',
    accessor: 'parent',
    align: 'left',
    clickable: true,
    Cell: (props: CellProps<IOrganization>) => {
      return (
        <Link to={`/admin/organization/${props.row.original.parentId}`}>
          {props.row.original.parent}
        </Link>
      );
    },
  },
];
