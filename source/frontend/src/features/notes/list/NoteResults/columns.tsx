import { Col, Row } from 'react-bootstrap';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { RemoveIconButton } from '@/components/common/buttons';
import ViewButton from '@/components/common/buttons/ViewButton';
import { ColumnWithProps, DateCell } from '@/components/Table';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';

export function createTableColumns(
  onShowDetails: (note: ApiGen_Concepts_Note) => void,
  onDelete: (note: ApiGen_Concepts_Note) => void,
) {
  const columns: ColumnWithProps<ApiGen_Concepts_Note>[] = [
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
      minWidth: 34,
      maxWidth: 34,
    },

    {
      Header: 'Actions',
      accessor: 'controls' as any, // this column is not part of the data model
      align: 'center',
      sortable: false,
      width: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<ApiGen_Concepts_Note>) => {
        const { hasClaim } = useKeycloakWrapper();

        return (
          <StyledDiv>
            <Col xs={1} className="p-0">
              {' '}
              {hasClaim(Claims.NOTE_VIEW) && (
                <ViewButton
                  onClick={() => onShowDetails(cellProps.row.original)}
                  title="View Note"
                />
              )}
            </Col>
            <Col xs={1} className="p-0">
              {hasClaim(Claims.NOTE_DELETE) && !cellProps.row.original.isSystemGenerated && (
                <RemoveIconButton
                  title="Delete Note"
                  onRemove={() => onDelete(cellProps.row.original)}
                />
              )}
            </Col>
          </StyledDiv>
        );
      },
    },
  ];

  return columns;
}

const StyledDiv = styled(Row)`
  justify-content: space-evenly;
  width: 100%;
`;
