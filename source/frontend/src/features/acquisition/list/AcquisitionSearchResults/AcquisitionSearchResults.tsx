import { Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { useCallback } from 'react';

import { columns } from './columns';

export interface IAcquisitionSearchResultsProps {
  results: Api_AcquisitionFile[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<Api_AcquisitionFile>;
  setSort: (value: TableSort<Api_AcquisitionFile>) => void;
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
    <Table<Api_AcquisitionFile>
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
