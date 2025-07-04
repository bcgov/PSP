import { useMemo } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { DocumentRow } from '@/features/documents/ComposedDocument';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_Document } from '@/models/api/generated/ApiGen_Concepts_Document';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';

import { ParentInformationDisplay } from '../DocumentListView';
import { getDocumentColumns } from './DocumentResultsColumns';

export interface IDocumentResultProps {
  results: DocumentRow[];
  loading?: boolean;
  sort: TableSort<ApiGen_Concepts_Document>;
  canEditDocuments: boolean;
  setSort: (value: TableSort<ApiGen_Concepts_Document>) => void;
  onViewDetails: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onViewParent: (relationshipType: ApiGen_CodeTypes_DocumentRelationType, parentId: number) => void;
  onPreview: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onDelete: (values: ApiGen_Concepts_DocumentRelationship) => void;
  showParentInformation: boolean;
  relationshipDisplay?: ParentInformationDisplay;
}

export const DocumentResults: React.FunctionComponent<
  React.PropsWithChildren<IDocumentResultProps>
> = ({
  results,
  setSort,
  sort,
  onViewDetails,
  onViewParent,
  onDelete,
  onPreview,
  showParentInformation,
  relationshipDisplay,
  canEditDocuments,
  ...rest
}) => {
  const columns = useMemo(
    () =>
      getDocumentColumns({
        onViewDetails,
        onViewParent,
        onDelete,
        onPreview,
        showParentInformation,
        relationshipDisplay,
        canEditDocuments,
      }),
    [
      onViewDetails,
      onViewParent,
      onDelete,
      onPreview,
      showParentInformation,
      relationshipDisplay,
      canEditDocuments,
    ],
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
