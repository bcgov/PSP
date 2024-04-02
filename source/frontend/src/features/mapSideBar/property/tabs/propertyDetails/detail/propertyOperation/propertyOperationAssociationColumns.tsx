import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ColumnWithProps } from '@/components/Table';
import { FileTypes } from '@/constants';
import { ApiGen_Concepts_Association } from '@/models/api/generated/ApiGen_Concepts_Association';

export interface PropertyOperationTypedAssociation extends ApiGen_Concepts_Association {
  type: FileTypes;
}

const getFileNameByType = (type: FileTypes) => {
  switch (type) {
    case FileTypes.Acquisition:
      return 'Acquisition';
    case FileTypes.Disposition:
      return 'Disposition';
    case FileTypes.Lease:
      return 'Lease/License';
    case FileTypes.Research:
      return 'Research';
    default:
      return 'Unknown File Type';
  }
};

const getUrlByType = (type: FileTypes, fileId: number) => {
  switch (type) {
    case FileTypes.Acquisition:
      return `/mapview/sidebar/acquisition/${fileId}`;
    case FileTypes.Disposition:
      return `/mapview/sidebar/disposition/${fileId}`;
    case FileTypes.Lease:
      return `/mapview/sidebar/lease/${fileId}`;
    case FileTypes.Research:
      return `/mapview/sidebar/research/${fileId}`;
    default:
      return '';
  }
};

export const getPropertyOperationAssociationColumns: () => ColumnWithProps<PropertyOperationTypedAssociation>[] =
  () => {
    return [
      {
        Header: 'Type',
        accessor: 'type',
        align: 'left',
        width: 10,
        maxWidth: 15,
        Cell: (props: CellProps<PropertyOperationTypedAssociation>) => {
          return <>{getFileNameByType(props.row.original.type)}</>;
        },
      },
      {
        Header: 'Number',
        accessor: 'id',
        align: 'left',
        clickable: true,
        width: 10,
        maxWidth: 30,
        Cell: (props: CellProps<PropertyOperationTypedAssociation>) => {
          return (
            <Link
              target="_blank"
              rel="noopener noreferrer"
              to={getUrlByType(props.row.original.type, props.row.original.id)}
            >
              {props.row.original.fileNumber}
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </Link>
          );
        },
      },
      {
        Header: 'File Status',
        accessor: 'status',
        align: 'left',
        width: 5,
        maxWidth: 10,
      },
    ];
  };

export default getPropertyOperationAssociationColumns;
