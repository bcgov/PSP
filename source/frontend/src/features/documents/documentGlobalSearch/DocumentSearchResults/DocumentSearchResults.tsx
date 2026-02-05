import { useCallback, useMemo } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentSearchResult } from '@/models/api/generated/ApiGen_Concepts_DocumentSearchResult';

import { getDocumentSearchColumns } from './columns/columns';

export interface IDocumentSearchResultsProps {
  results: ApiGen_Concepts_DocumentSearchResult[];
  totalItems?: number;
  pageSize?: number;
  pageIndex?: number;
  loading?: boolean;
  pageCount?: number;
  sort?: TableSort<ApiGen_Concepts_DocumentSearchResult>;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  onViewDetails?: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onPreview?: (mayanDocumentId: number) => void;
}

export const DocumentSearchResults: React.FC<IDocumentSearchResultsProps> = ({
  results,
  totalItems,
  pageIndex,
  pageSize,
  setPageIndex,
  setPageSize,
  onViewDetails,
  onPreview,
  ...rest
}: IDocumentSearchResultsProps) => {
  const columns = useMemo(
    () => getDocumentSearchColumns({ onViewDetails, onPreview }),
    [onPreview, onViewDetails],
  );

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex: nextPageIndex }: { pageIndex: number }) => {
      if (!setPageIndex) {
        return;
      }
      if (pageIndex === nextPageIndex) {
        return;
      }
      setPageIndex(nextPageIndex);
    },
    [setPageIndex, pageIndex],
  );

  const handlePageSizeChange = useCallback(
    (nextPageSize: number) => {
      if (!setPageSize) {
        return;
      }
      if (pageSize === nextPageSize) {
        return;
      }
      setPageSize(nextPageSize);
    },
    [setPageSize, pageSize],
  );

  return (
    <Table<ApiGen_Concepts_DocumentSearchResult>
      name="documentsTable"
      manualSortBy={false}
      totalItems={totalItems}
      columns={columns}
      data={results ?? []}
      noRowsMessage="No matching Documents found"
      onRequestData={updateCurrentPage}
      onPageSizeChange={handlePageSizeChange}
      pageIndex={pageIndex}
      pageSize={pageSize}
      {...rest}
    ></Table>
  );
};
