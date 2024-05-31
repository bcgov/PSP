import { FaExternalLinkAlt } from 'react-icons/fa';
import { FiCheck } from 'react-icons/fi';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { AreaUnitTypes } from '@/constants';
import { convertArea, formatNumber } from '@/utils';

import { PropertyOperationResult } from './OperationView';

const getPropertyOperationColumns: (
  isSubdivision: boolean,
) => ColumnWithProps<PropertyOperationResult>[] = (isSubdivision: boolean) => {
  return [
    {
      Header: isSubdivision ? 'Parent' : 'Child',
      accessor: 'isSource',
      align: 'center',
      width: 5,
      maxWidth: 5,
      Cell: (props: CellProps<PropertyOperationResult>) => {
        const isSource = props.row.original.isSource;
        if ((isSubdivision && isSource) || (!isSubdivision && !isSource)) {
          return (
            <span data-testid="isSource">
              <FiCheck size="2rem" color="black" />
            </span>
          );
        } else {
          return <></>;
        }
      },
    },
    {
      Header: 'Identifier',
      accessor: 'identifier',
      align: 'left',
      clickable: true,
      width: 10,
      maxWidth: 20,
      Cell: (props: CellProps<PropertyOperationResult>) => {
        return (
          <Link
            target="_blank"
            rel="noopener noreferrer"
            to={`/mapview/sidebar/property/${props.row.original.id}`}
          >
            {props.row.original.identifier}
            <FaExternalLinkAlt className="ml-2" size="1rem" />
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
      Cell: (props: CellProps<PropertyOperationResult>) => {
        return <> {props.row.original.plan} </>;
      },
    },
    {
      Header: 'Status',
      accessor: 'status',
      align: 'left',
      width: 10,
      maxWidth: 20,
      Cell: (props: CellProps<PropertyOperationResult>) => {
        return <> {props.row.original.status} </>;
      },
    },
    {
      Header: 'Area',
      accessor: 'area',
      align: 'right',
      width: 20,
      maxWidth: 20,
      Cell: (props: CellProps<PropertyOperationResult>) => {
        const landArea = props.row.original.area;
        const landUnitCode = props.row.original.areaUnitCode;
        const meters = convertArea(landArea, landUnitCode, AreaUnitTypes.SquareMeters);
        return (
          <>
            {formatNumber(meters, 0, 4)} m<sup>2</sup>
          </>
        );
      },
    },
  ];
};

export default getPropertyOperationColumns;
