import { ColumnWithProps, DateCell, renderTypeCode, Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import { Claims } from 'constants/claims';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { IResearchSearchResult } from 'interfaces/IResearchSearchResult';
import { Api_PropertyResearchFile } from 'models/api/PropertyResearchFile';
import React, { useCallback } from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

const columns: ColumnWithProps<IResearchSearchResult>[] = [
  {
    Header: 'R-File Number',
    accessor: 'rfileNumber',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<IResearchSearchResult>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.CONTACT_VIEW)) {
        return (
          <Link to={`/mapview/research/${props.row.original.id}/view`}>
            {props.row.original.rfileNumber}
          </Link>
        );
      }
      return props.row.original.rfileNumber;
    },
  },
  {
    Header: 'Research file name',
    accessor: 'name',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'MOTI Region',
    accessor: 'researchProperties',
    align: 'right',
    clickable: true,
    width: 10,
    maxWidth: 20,
    Cell: ({ value }: CellProps<any, Api_PropertyResearchFile[]>) => {
      const regions = [...new Set(value.map(pr => pr?.property?.region?.description))];
      return regions.join(', ');
    },
  },
  {
    Header: 'Created by',
    accessor: 'appCreateUserid',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Created date',
    accessor: 'appCreateTimestamp',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: DateCell,
  },
  {
    Header: 'Last updated by',
    accessor: 'appLastUpdateUserid',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Last updated date',
    accessor: 'appLastUpdateTimestamp',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: DateCell,
  },
  {
    Header: 'Status',
    accessor: 'researchFileStatusTypeCode',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: renderTypeCode,
  },
];

export interface IResearchSearchResultsProps {
  results: IResearchSearchResult[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<IResearchSearchResult>;
  setSort?: (value: TableSort<IResearchSearchResult>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

export function ResearchSearchResults(props: IResearchSearchResultsProps) {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, ...rest } = props;

  // results sort handler
  const handleSortChange = useCallback(
    (column: string, nextSortDirection: SortDirection) => {
      if (!setSort) return null;

      let nextSort: TableSort<IResearchSearchResult>;

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
    <Table<IResearchSearchResult>
      manualSortBy={true}
      name="researchFilesTable"
      columns={columns}
      data={results ?? []}
      sort={sort}
      onSortChange={handleSortChange}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="No matching Research Files found"
      totalItems={props.totalItems}
      {...rest}
    ></Table>
  );
}
