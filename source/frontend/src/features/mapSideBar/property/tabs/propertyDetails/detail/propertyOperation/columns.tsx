import { FiCheck } from 'react-icons/fi';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { AreaUnitTypes } from '@/constants';
import { convertArea, formatNumber } from '@/utils';

import { PropertySubdivisionResult } from './SubdivisionView';

const columns: ColumnWithProps<PropertySubdivisionResult>[] = [
  {
    Header: 'Parent',
    accessor: 'isSource',
    align: 'center',
    width: 5,
    maxWidth: 5,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      return props.row.original.isSource ? (
        <span data-testid="isSource">
          <FiCheck size="2rem" color="black" />
        </span>
      ) : (
        <></>
      );
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
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      return <> {props.row.original.status} </>;
    },
  },
  {
    Header: 'Area (sq m)',
    accessor: 'area',
    align: 'right',
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<PropertySubdivisionResult>) => {
      const landArea = props.row.original.area;
      const landUnitCode = props.row.original.areaUnitCode;
      const meters = convertArea(landArea, landUnitCode, AreaUnitTypes.SquareMeters);
      return <> {formatNumber(meters, 0, 2)} </>;
    },
  },
];

export default columns;
