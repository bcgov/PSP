import { useCallback } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';

import { columns } from './columns';
import { ProjectSearchResultModel } from './models';

export interface IProjectSearchResultsProps {
  results: ProjectSearchResultModel[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ProjectSearchResultModel>;
  setSort: (value: TableSort<ProjectSearchResultModel>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

/**
 * Component that renders search results for Projects.
 * @param {IProjectSearchResultsProps} props
 */
export const ProjectSearchResults = (props: IProjectSearchResultsProps) => {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, totalItems, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<ProjectSearchResultModel>
      name="projectsTable"
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
