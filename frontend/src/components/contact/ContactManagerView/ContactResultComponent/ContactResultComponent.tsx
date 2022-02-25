import { Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import { IContactSearchResult } from 'interfaces';
import { useCallback } from 'react';

import columns from './columns';
import summaryColumns from './summaryColumns';

export interface IContactResultComponentProps {
  results: IContactSearchResult[];
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  totalItems?: number;
  sort?: TableSort<IContactSearchResult>;
  setSort?: (value: TableSort<IContactSearchResult>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
  setSelectedRows?: (selectedContacts: IContactSearchResult[]) => void;
  selectedRows?: IContactSearchResult[];
  showSelectedRowCount?: boolean;
  isSummary?: boolean;
  isSingleSelect?: boolean;
}

/**
 * Display a react table with the results filtered by the {@link ContactFilter}
 * @param {IContactSearchResult} props
 */
export function ContactResultComponent(props: IContactResultComponentProps) {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, ...rest } = props;

  // results sort handler
  const handleSortChange = useCallback(
    (column: string, nextSortDirection: SortDirection) => {
      if (!setSort) return null;

      let nextSort: TableSort<IContactSearchResult>;

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

  const listColumns =
    props.isSummary === undefined || props.isSummary === false ? columns : summaryColumns;

  return (
    <Table<IContactSearchResult>
      name="contactsTable"
      columns={listColumns}
      data={results ?? []}
      sort={sort}
      manualSortBy={true}
      onSortChange={handleSortChange}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="No Contacts match the search criteria"
      showSelectedRowCount={props.showSelectedRowCount}
      totalItems={props.totalItems}
      isSingleSelect={props.isSingleSelect}
      {...rest}
    ></Table>
  );
}
