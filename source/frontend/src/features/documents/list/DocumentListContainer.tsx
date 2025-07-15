import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { exists } from '@/utils';

import { DocumentRow } from '../ComposedDocument';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { IUpdateDocumentsStrategy } from '../models/IUpdateDocumentsStrategy';
import DocumentListView from './DocumentListView';

export interface IDocumentListContainerProps {
  parentId: string;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  disableAdd?: boolean;
  addButtonText?: string;
  title?: string;
  statusSolver?: IUpdateDocumentsStrategy | null;
  onSuccess?: () => void;
}

const DocumentListContainer: React.FunctionComponent<IDocumentListContainerProps> = props => {
  const pathGenerator = usePathGenerator();

  const isMounted = useIsMounted();

  const [documentResults, setDocumentResults] = useState<DocumentRow[]>([]);

  const {
    retrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading,
    deleteDocumentRelationship,
  } = useDocumentRelationshipProvider();

  const retrieveDocuments = useCallback(async () => {
    const documents = await retrieveDocumentRelationship(props.relationshipType, props.parentId);
    if (exists(documents) && isMounted()) {
      setDocumentResults([
        ...documents
          .filter((x): x is ApiGen_Concepts_DocumentRelationship => !!x?.document)
          .map(x => DocumentRow.fromApi(x, 'self')),
      ]);
    }
  }, [isMounted, retrieveDocumentRelationship, props.relationshipType, props.parentId]);

  useEffect(() => {
    retrieveDocuments();
  }, [retrieveDocuments]);

  const onDelete = async (
    documentRelationship: ApiGen_Concepts_DocumentRelationship,
  ): Promise<boolean | undefined> => {
    if (documentRelationship.relationshipType !== null) {
      const result = await deleteDocumentRelationship(
        documentRelationship.relationshipType,
        documentRelationship,
      );
      if (result && isMounted()) {
        props.onSuccess?.();
        updateCallback();
      }

      return result;
    } else {
      toast.error(
        'Invalid document relationship error during delete. Check responses and try again.',
      );
    }
  };

  const onSuccess = async () => {
    props.onSuccess?.();
    updateCallback();
  };

  const updateCallback = useCallback(() => {
    retrieveDocuments();
  }, [retrieveDocuments]);

  const handleDocumentsRefresh = async () => {
    retrieveDocuments();
  };

  const handleViewParent = async (
    relationshipType: ApiGen_CodeTypes_DocumentRelationType,
    parentId: number,
  ) => {
    pathGenerator.showFile(relationshipType, parentId);
  };

  const editDocumentsEnabled = !props.statusSolver || props.statusSolver?.canEditDocuments();

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading}
      documentResults={documentResults}
      disableAdd={props.disableAdd}
      canEditDocuments={editDocumentsEnabled}
      onDelete={onDelete}
      onSuccess={onSuccess}
      onRefresh={handleDocumentsRefresh}
      onViewParent={handleViewParent}
      title={props.title}
      showParentInformation={false}
      relationshipTypes={[]}
      data-testId="main-document-list"
    />
  );
};

export default DocumentListContainer;
