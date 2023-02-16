import { Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { DocumentRow } from 'features/documents/ComposedDocument';
import { Api_Document, Api_DocumentRelationship } from 'models/api/Document';
import { useMemo } from 'react';
import { getPage } from 'utils';

import { getDocumentColumns } from './DocumentResultsColumns';

export interface IDocumentResultProps {
  results: DocumentRow[];
  loading?: boolean;
  sort: TableSort<Api_Document>;
  setSort: (value: TableSort<Api_Document>) => void;
  onViewDetails: (values: Api_DocumentRelationship) => void;
  onDelete: (values: Api_DocumentRelationship) => void;
  onPageChange: (props: { pageIndex?: number; pageSize: number }) => void;
  pageProps: { pageIndex?: number; pageSize: number };
}

export const DocumentResults: React.FunctionComponent<
  React.PropsWithChildren<IDocumentResultProps>
> = ({ results, setSort, sort, onViewDetails, onDelete, pageProps, ...rest }) => {
  const columns = useMemo(
    () => getDocumentColumns({ onViewDetails, onDelete }),
    [onViewDetails, onDelete],
  );

  return (
    <Table<DocumentRow>
      name="documentsTable"
      manualSortBy={false}
      manualPagination
      onPageSizeChange={size => {
        rest.onPageChange({ pageSize: size });
      }}
      onRequestData={rest.onPageChange}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={getPage(pageProps.pageIndex ?? 0, pageProps.pageSize, results) ?? []}
      noRowsMessage="No matching Documents found"
      pageCount={Math.ceil(results.length / pageProps.pageSize)}
      pageSize={pageProps.pageSize}
      pageIndex={pageProps.pageIndex}
      {...rest}
    ></Table>
  );
};
