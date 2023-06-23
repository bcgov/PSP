import { useCallback } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';

import { columns } from './columns';
import { AcquisitionSearchResultModel } from './models';

export interface IAcquisitionSearchResultsProps {
  results: AcquisitionSearchResultModel[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<AcquisitionSearchResultModel>;
  setSort: (value: TableSort<AcquisitionSearchResultModel>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

/**
 * Component that renders search results for acquisition files.
 * @param {IAcquisitionSearchResultsProps} props
 */
export function AcquisitionSearchResults(props: IAcquisitionSearchResultsProps) {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, totalItems, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<AcquisitionSearchResultModel>
      name="acquisitionFilesTable"
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
}
