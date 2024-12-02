import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { ColumnWithProps } from '@/components/Table';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists } from '@/utils';
import { stringToFragment } from '@/utils/columnUtils';

export function createSubFilesTableColumns(currentAcquisitionId: number, routeURL: string) {
  const columns: ColumnWithProps<ApiGen_Concepts_AcquisitionFile>[] = [
    {
      Header: '#',
      align: 'left',
      sortable: false,
      minWidth: 15,
      maxWidth: 15,
      Cell: (cellProps: CellProps<ApiGen_Concepts_AcquisitionFile>) => {
        if (currentAcquisitionId === cellProps.row.original.id) {
          return stringToFragment(formatFileSuffix(cellProps.row.original.fileNumberSuffix));
        } else {
          return (
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={`${routeURL}/${cellProps.row.original.id}`}
              data-testid={`sub-file-link-${cellProps.row.original.id}`}
            >
              <span>{formatFileSuffix(cellProps.row.original.fileNumberSuffix)}</span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          );
        }
      },
    },
    {
      Header: 'Name',
      align: 'left',
      sortable: false,
      minWidth: 30,
      maxWidth: 30,
      Cell: (cellProps: CellProps<ApiGen_Concepts_AcquisitionFile>) => {
        return stringToFragment(cellProps.row.original.fileName);
      },
    },
    {
      Header: 'Status',
      align: 'left',
      sortable: false,
      minWidth: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<ApiGen_Concepts_AcquisitionFile>) => {
        return stringToFragment(cellProps.row.original.fileStatusTypeCode.description);
      },
    },
  ];

  return columns;
}

const StyledLink = styled(Link)`
  display: flex;
  align-items: center;
`;

function formatFileSuffix(suffix: number): string {
  return exists(suffix) ? suffix.toString().padStart(2, '0') : '';
}
