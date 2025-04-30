import { useCallback } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';

import { ManagementSearchResultModel } from '../models';
import { columns } from './columns';

export interface IManagementSearchResultsProps {
  results: ManagementSearchResultModel[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ManagementSearchResultModel>;
  setSort: (value: TableSort<ManagementSearchResultModel>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

/**
 * Component that renders search results for management files.
 * @param {IManagementSearchResultsProps} props
 */
export const ManagementSearchResults: React.FC<IManagementSearchResultsProps> = props => {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, totalItems, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => {
      setPageIndex?.(pageIndex);
    },
    [setPageIndex],
  );

  return (
    <Table<ManagementSearchResultModel>
      name="managementFilesTable"
      columns={columns}
      data={results ?? []}
      externalSort={{ sort: sort, setSort: setSort }}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="No matching results can be found. Try widening your search criteria."
      totalItems={totalItems}
      manualPagination
      {...rest}
    ></Table>
  );
};
