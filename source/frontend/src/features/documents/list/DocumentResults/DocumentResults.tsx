import { useMemo } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { DocumentRow } from '@/features/documents/ComposedDocument';
import { ApiGen_Concepts_Document } from '@/models/api/generated/ApiGen_Concepts_Document';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';

import { IUpdateDocumentsStrategy } from '../../models/IUpdateDocumentsStrategy';
import { getDocumentColumns } from './DocumentResultsColumns';

export interface IDocumentResultProps {
  results: DocumentRow[];
  loading?: boolean;
  sort: TableSort<ApiGen_Concepts_Document>;
  statusSolver?: IUpdateDocumentsStrategy;
  setSort: (value: TableSort<ApiGen_Concepts_Document>) => void;
  onViewDetails: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onPreview: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onDelete: (values: ApiGen_Concepts_DocumentRelationship) => void;
}

export const DocumentResults: React.FunctionComponent<
  React.PropsWithChildren<IDocumentResultProps>
> = ({ results, statusSolver, setSort, sort, onViewDetails, onDelete, onPreview, ...rest }) => {
  const columns = useMemo(
    () => getDocumentColumns({ statusSolver, onViewDetails, onDelete, onPreview }),
    [statusSolver, onViewDetails, onDelete, onPreview],
  );

  return (
    <Table<DocumentRow>
      name="documentsTable"
      manualSortBy={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching Documents found"
      manualPagination={false}
      {...rest}
    ></Table>
  );
};
