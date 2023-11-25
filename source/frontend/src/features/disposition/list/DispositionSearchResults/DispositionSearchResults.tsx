import { useCallback } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';

import { DispositionSearchResultModel } from '../models';
import { columns } from './columns';

export interface IDispositionSearchResultsProps {
  results: DispositionSearchResultModel[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<DispositionSearchResultModel>;
  setSort: (value: TableSort<DispositionSearchResultModel>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

/**
 * Component that renders search results for disposition files.
 * @param {IDispositionSearchResultsProps} props
 */
export const DispositionSearchResults: React.FC<IDispositionSearchResultsProps> = props => {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, totalItems, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<DispositionSearchResultModel>
      name="dispositionFilesTable"
      columns={columns}
      data={results ?? []}
      externalSort={{ sort: sort, setSort: setSort }}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="No matching results can be found. Try widening your search criteria."
      totalItems={totalItems}
      {...rest}
    ></Table>
  );
};
