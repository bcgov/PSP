import { FiCheck } from 'react-icons/fi';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';

import { PropertySubdivisionResult } from './SubdivisionView';

const columns: ColumnWithProps<PropertySubdivisionResult>[] = [
  {
    Header: 'Parent',
    accessor: 'isSource',
    align: 'center',
    clickable: true,
    width: 5,
    maxWidth: 5,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      return props.row.original.isSource ? <FiCheck size="2rem" color="black" /> : <></>;
    },
  },
  {
    Header: 'Identifier',
    accessor: 'identifier',
    align: 'left',
    clickable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      return (
        <Link to={`/mapview/sidebar/property/${props.row.original.id}`}>
          {props.row.original.identifier}
        </Link>
      );
    },
  },
  {
    Header: 'Plan #',
    accessor: 'plan',
    align: 'right',
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      return <> {props.row.original.plan} </>;
    },
  },
  {
    Header: 'Status',
    accessor: 'status',
    align: 'left',
    clickable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      return <> {props.row.original.status} </>;
    },
  },
  {
    Header: 'Area (ha)',
    accessor: 'area',
    align: 'right',
    clickable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      return <> {props.row.original.area} </>;
    },
  },
];

export default columns;
