import { ColumnWithProps, Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import { ILeaseSearchResult } from 'interfaces';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

const columns: ColumnWithProps<ILeaseSearchResult>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'right',
    clickable: true,
    width: 10,
    Cell: (props: CellProps<ILeaseSearchResult>) => (
      <Link to={`/lease/${props.row.original.id}`}>{props.row.original.lFileNo}</Link>
    ),
  },
  {
    Header: 'Tenant Names',
    accessor: 'tenantNames',
    align: 'left',
    width: 40,
    maxWidth: 100,
    Cell: (props: CellProps<ILeaseSearchResult>) => {
      return props.row.original.tenantNames.map((x, y) => {
        return (
          <Row key={y} className="w-100">
            <Col md="auto">
              <div> {x}</div>
            </Col>
          </Row>
        );
      });
    },
  },
  {
    Header: 'Program Name',
    accessor: 'programName',
    align: 'left',
    width: 40,
    maxWidth: 80,
  },
  {
    Header: 'Properties',
    accessor: 'properties',
    align: 'left',

    Cell: (props: CellProps<ILeaseSearchResult>) => {
      return props.row.original.properties.map((x, y) => {
        return (
          <Row key={y} className="w-100 mx-1 mb-3 border-bottom">
            <Col md="auto" className="mr-0 pr-0">
              <div>
                <strong>PID:</strong> {x.pid || 'n.a'}
              </div>
              <div>
                <strong>PIN:</strong> {x.pin || 'n.a'}
              </div>
            </Col>
            <Col md="auto" className="mr-1 pr-1">
              <strong>Address:</strong>
            </Col>
            <Col className="ml-0 pl-0">{x.address}</Col>
          </Row>
        );
      });
    },
  },
];

export interface ILeaseSearchResultsProps {
  results: ILeaseSearchResult[];
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ILeaseSearchResult>;
  setSort?: (value: TableSort<ILeaseSearchResult>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
}

export function LeaseSearchResults(props: ILeaseSearchResultsProps) {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, ...rest } = props;

  // results sort handler
  const handleSortChange = useCallback(
    (column: string, nextSortDirection: SortDirection) => {
      if (!setSort) return null;

      let nextSort: TableSort<ILeaseSearchResult>;

      // add new column to sort criteria
      if (nextSortDirection) {
        nextSort = { ...sort, [column]: nextSortDirection };
      } else {
        // remove column from sort criteria
        nextSort = { ...sort };
        delete (nextSort as any)[column];
      }
      setSort(nextSort);
    },
    [setSort, sort],
  );

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<ILeaseSearchResult>
      name="leasesTable"
      columns={columns}
      data={results ?? []}
      sort={sort}
      onSortChange={handleSortChange}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="Lease / License details do not exist in PIMS inventory"
      {...rest}
    ></Table>
  );
}
