import { useCallback } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';

import { ManagementActivitySearchResultModel } from '../../models/ManagementActivitySearchResultModel';
import { columns } from './columns';

export interface IManagementActivitySearchResultsProps {
  results: ManagementActivitySearchResultModel[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ManagementActivitySearchResultModel>;
  setSort: (value: TableSort<ManagementActivitySearchResultModel>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

/**
 * Component that renders search results for management files.
 * @param {IManagementSearchResultsProps} props
 */
export const ManagementActivitySearchResults: React.FC<IManagementActivitySearchResultsProps> = (
  props: IManagementActivitySearchResultsProps,
) => {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, totalItems, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => {
      setPageIndex?.(pageIndex);
    },
    [setPageIndex],
  );

  return (
    <Table<ManagementActivitySearchResultModel>
      name="managementActivitiesTable"
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

export default ManagementActivitySearchResults;
