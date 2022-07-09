import { IconButton } from 'components/common/buttons';
import { ColumnWithProps, DateCell, Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { Claims } from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Note } from 'models/api/Note';
import { MdDelete } from 'react-icons/md';
import { CellProps } from 'react-table';

export interface INoteResultProps {
  results: Api_Note[];
  loading?: boolean;

  onDelete: (note: Api_Note) => void;
  sort: TableSort<Api_Note>;
  setSort: (value: TableSort<Api_Note>) => void;
}

export function NoteResults(props: INoteResultProps) {
  const { hasClaim } = useKeycloakWrapper();
  const { results, setSort, sort, ...rest } = props;

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
      accessor: 'id',
      align: 'right',
      sortable: false,
      width: 10,
      maxWidth: 20,
      Cell: (cellProps: CellProps<Api_Note>) => {
        return (
          hasClaim(Claims.NOTE_DELETE) && (
            <IconButton
              title="Delete Note"
              variant="light"
              onClick={() => {
                props.onDelete(cellProps.row.original);
              }}
            >
              <MdDelete size={22} />
            </IconButton>
          )
        );
      },
    },
  ];
  return (
    <Table<Api_Note>
      name="notesTable"
      manualSortBy={false}
      lockPageSize={true}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching Notes found"
      {...rest}
    ></Table>
  );
}
