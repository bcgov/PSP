import React from 'react';
import { FaTrash } from 'react-icons/fa';
import { ImFileText2 } from 'react-icons/im';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { StyledIconButton, StyledRemoveLinkButton } from '@/components/common/buttons';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps, DateCell } from '@/components/Table';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { Api_Note } from '@/models/api/Note';

export function createTableColumns(
  onShowDetails: (note: Api_Note) => void,
  onDelete: (note: Api_Note) => void,
) {
  const columns: ColumnWithProps<Api_Note>[] = [
    {
      Header: 'Note',
      accessor: 'note',
      align: 'left',
      sortable: false,
      width: 80,
    },
    {
      Header: 'Created date',
      accessor: 'appCreateTimestamp',
      align: 'center',
      sortable: true,
      minWidth: 24,
      maxWidth: 24,
      Cell: DateCell,
    },
    {
      Header: 'Last updated by',
      accessor: 'appLastUpdateUserid',
      align: 'center',
      sortable: true,
      minWidth: 28,
      maxWidth: 28,
    },

    {
      Header: 'Actions',
      accessor: 'controls' as any, // this column is not part of the data model
      align: 'center',
      sortable: false,
      width: 10,
      maxWidth: 10,
      Cell: (cellProps: CellProps<Api_Note>) => {
        const { hasClaim } = useKeycloakWrapper();

        return (
          <StyledDiv>
            {hasClaim(Claims.NOTE_VIEW) && (
              <StyledIconButton
                title="View Note"
                variant="light"
                onClick={() => onShowDetails(cellProps.row.original)}
              >
                <ImFileText2 size="2rem" />
              </StyledIconButton>
            )}

            {hasClaim(Claims.NOTE_DELETE) && !cellProps.row.original.isSystemGenerated && (
              <StyledRemoveLinkButton
                title="Delete Note"
                variant="light"
                onClick={() => onDelete(cellProps.row.original)}
              >
                <FaTrash size="2rem" />
              </StyledRemoveLinkButton>
            )}
          </StyledDiv>
        );
      },
    },
  ];

  return columns;
}

const StyledDiv = styled(InlineFlexDiv)`
  justify-content: space-around;
  width: 100%;
`;
