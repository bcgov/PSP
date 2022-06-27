import { IconButton } from 'components/common/buttons';
import { ColumnWithProps, DateCell, Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { INoteResult } from 'interfaces/INoteResult';
import { MdDelete } from 'react-icons/md';
import { CellProps } from 'react-table';

export interface INoteResultProps {
  results: INoteResult[];
  loading?: boolean;

  onDelete: (note: INoteResult) => void;
  sort: TableSort<INoteResult>;
  setSort: (value: TableSort<INoteResult>) => void;
}

export function NoteResults(props: INoteResultProps) {
  const { results, setSort, sort, ...rest } = props;

  const columns: ColumnWithProps<INoteResult>[] = [
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
      Cell: (cellProps: CellProps<INoteResult>) => {
        return (
          <IconButton
            title="Delete Note"
            variant="light"
            onClick={() => {
              props.onDelete(cellProps.row.original);
            }}
          >
            <MdDelete size={22} />
          </IconButton>
        );
      },
    },
  ];
  return (
    <Table<INoteResult>
      name="notesTable"
      manualSortBy={false}
      pageSize={-1}
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
