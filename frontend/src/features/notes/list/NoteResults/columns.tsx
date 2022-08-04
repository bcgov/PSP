import { IconButton } from 'components/common/buttons';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps, DateCell } from 'components/Table';
import { Claims } from 'constants/index';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { Api_Note } from 'models/api/Note';
import { ImFileText2 } from 'react-icons/im';
import { MdDelete } from 'react-icons/md';
import { CellProps } from 'react-table';
import styled from 'styled-components';

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
      minWidth: 100,
      width: 100,
    },
    {
      Header: 'Created date',
      accessor: 'appCreateTimestamp',
      align: 'center',
      sortable: true,
      width: 10,
      maxWidth: 20,
      Cell: DateCell,
    },
    {
      Header: 'Last updated by',
      accessor: 'appLastUpdateUserid',
      align: 'center',
      sortable: true,
      width: 10,
      maxWidth: 20,
    },

    {
      Header: '',
      accessor: 'controls' as any, // this column is not part of the data model
      align: 'right',
      sortable: false,
      width: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<Api_Note>) => {
        const { hasClaim } = useKeycloakWrapper();

        return (
          <StyledDiv>
            {hasClaim(Claims.NOTE_VIEW) && (
              <IconButton
                title="View Note"
                variant="light"
                onClick={() => onShowDetails(cellProps.row.original)}
              >
                <ImFileText2 size="2rem" />
              </IconButton>
            )}

            {hasClaim(Claims.NOTE_DELETE) && (
              <IconButton
                title="Delete Note"
                variant="light"
                onClick={() => onDelete(cellProps.row.original)}
              >
                <MdDelete size="2rem" />
              </IconButton>
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
