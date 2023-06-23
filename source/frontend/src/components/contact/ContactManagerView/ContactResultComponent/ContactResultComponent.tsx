import { useCallback } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { IContactSearchResult } from '@/interfaces';

import columns from './columns';
import summaryColumns from './summaryColumns';

export interface IContactResultComponentProps {
  results: IContactSearchResult[];
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  totalItems?: number;
  sort?: TableSort<IContactSearchResult>;
  setSort: (value: TableSort<IContactSearchResult>) => void;
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
      externalSort={{ sort: sort, setSort: setSort }}
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
