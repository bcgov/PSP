import { FaExternalLinkAlt } from 'react-icons/fa';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { RemoveIconButton } from '@/components/common/buttons';
import ViewButton from '@/components/common/buttons/ViewButton';
import { StyledLink } from '@/components/common/styles';
import { ColumnWithProps } from '@/components/Table';
import { UtcDateCell } from '@/components/Table/DateCell';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { exists } from '@/utils/utils';

export function createNoteTableColumns() {
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
      Cell: UtcDateCell,
    },
    {
      Header: 'Last updated by',
      accessor: 'appLastUpdateUserid',
      align: 'center',
      sortable: true,
      minWidth: 34,
      maxWidth: 34,
    },
  ];

  return columns;
}

export const createNoteActionsColumn = (
  canEditNotes: boolean,
  onShowDetails: (note: ApiGen_Concepts_Note) => void,
  onDelete: (note: ApiGen_Concepts_Note) => void,
) => ({
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
        {hasClaim(Claims.NOTE_VIEW) && exists(onShowDetails) && (
          <ViewButton
            onClick={() => onShowDetails(cellProps.row.original)}
            title="View Note"
            data-testId={'view-note-' + cellProps.row.index}
          />
        )}
        {hasClaim(Claims.NOTE_DELETE) &&
          canEditNotes &&
          exists(onDelete) &&
          !cellProps.row.original.isSystemGenerated && (
            <RemoveIconButton
              title="Delete Note"
              onRemove={() => onDelete(cellProps.row.original)}
              data-testId={'remove-note-' + cellProps.row.index}
            />
          )}
      </StyledDiv>
    );
  },
});

export function createNoteLinkColumn(
  headerText: string,
  getNavigationUrlTitle: (row: ApiGen_Concepts_Note) => { url: string; title: string },
) {
  return {
    Header: headerText,
    align: 'left',
    sortable: false,
    width: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<ApiGen_Concepts_Note>) => {
      const activityRow = cellProps.row.original;
      if (!exists(getNavigationUrlTitle)) {
        return null;
      }
      const urlAndTitle = getNavigationUrlTitle(activityRow);
      return (
        <StyledLink target="_blank" rel="noopener noreferrer" to={urlAndTitle.url}>
          <span>
            {urlAndTitle.title} <FaExternalLinkAlt className="ml-2" size="1rem" />
          </span>
        </StyledLink>
      );
    },
  };
}

const StyledDiv = styled.div`
  justify-content: space-evenly;
  width: 100%;
  display: flex;
`;
