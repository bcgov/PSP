import { noop } from 'lodash';
import { useCallback, useEffect, useMemo, useState } from 'react';

import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { exists, relationshipTypeToPathName } from '@/utils';

import { DocumentRow } from '../ComposedDocument';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { IDocumentListContainerProps } from './DocumentListContainer';
import DocumentListView from './DocumentListView';
import { DocRelation } from './models';

const DocumentFileListContainer: React.FunctionComponent<IDocumentListContainerProps> = props => {
  const pathGenerator = usePathGenerator();

  const isMounted = useIsMounted();

  const [documentResults, setDocumentResults] = useState<DocumentRow[]>([]);
  const [fileAssociations, setFileAssociations] =
    useState<ApiGen_Concepts_PropertyAssociations>(null);

  const { execute: getPropertyAssociations } = usePropertyAssociations();

  const { retrieveDocumentRelationship, retrieveDocumentRelationshipLoading } =
    useDocumentRelationshipProvider();

  const retrievePropertyAssociations = useCallback(async () => {
    const result = await getPropertyAssociations(Number(props.parentId));
    if (exists(result) && isMounted()) {
      setFileAssociations(result);
    }
  }, [getPropertyAssociations, props.parentId, isMounted]);

  const retrieveAssociationDocuments = useCallback(
    async (associations: ApiGen_Concepts_PropertyAssociations) => {
      if (!exists(associations)) {
        return;
      }

      let docRelations: DocRelation[] = associations.acquisitionAssociations?.map(x => ({
        id: x.id,
        relationType: ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles,
        fileNumber: x.fileNumber,
      }));
      docRelations = docRelations.concat(
        associations.managementAssociations?.map(x => ({
          id: x.id,
          relationType: ApiGen_CodeTypes_DocumentRelationType.ManagementFiles,
          fileNumber: x.fileNumber,
        })),
      );
      docRelations = docRelations.concat(
        associations.researchAssociations?.map(x => ({
          id: x.id,
          relationType: ApiGen_CodeTypes_DocumentRelationType.ResearchFiles,
          fileNumber: x.fileNumber,
        })),
      );
      docRelations = docRelations.concat(
        associations.leaseAssociations?.map(x => ({
          id: x.id,
          relationType: ApiGen_CodeTypes_DocumentRelationType.Leases,
          fileNumber: x.fileNumber,
        })),
      );
      docRelations = docRelations.concat(
        associations.dispositionAssociations?.map(x => ({
          id: x.id,
          relationType: ApiGen_CodeTypes_DocumentRelationType.DispositionFiles,
          fileNumber: x.fileNumber,
        })),
      );
      let documentRows: DocumentRow[] = [];
      for (const docRelation of docRelations.filter(exists)) {
        const documents = await retrieveDocumentRelationship(
          docRelation.relationType,
          docRelation.id.toString(),
        );
        if (documents !== undefined && isMounted()) {
          documentRows = documentRows.concat(
            documents
              .filter((x): x is ApiGen_Concepts_DocumentRelationship => !!x?.document)
              .map(x => DocumentRow.fromApi(x, docRelation.fileNumber)),
          );
        }
      }
      setDocumentResults([...documentRows]);
    },
    [retrieveDocumentRelationship, isMounted],
  );

  useEffect(() => {
    retrievePropertyAssociations();
  }, [retrievePropertyAssociations]);

  useEffect(() => {
    retrieveAssociationDocuments(fileAssociations);
  }, [retrieveAssociationDocuments, fileAssociations]);

  const handleDocumentsRefresh = async () => {
    retrieveAssociationDocuments(fileAssociations);
  };

  const handleViewParent = async (
    relationshipType: ApiGen_CodeTypes_DocumentRelationType,
    parentId: number,
  ) => {
    const file = relationshipTypeToPathName(relationshipType);
    pathGenerator.showFile(file, parentId);
  };

  const relationshipTypes = useMemo(() => {
    return [...new Set(documentResults?.map(x => x.relationshipType) ?? [])];
  }, [documentResults]);

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      relationshipTypes={relationshipTypes}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading}
      documentResults={documentResults}
      onDelete={undefined}
      onSuccess={noop}
      onRefresh={handleDocumentsRefresh}
      onViewParent={handleViewParent}
      disableAdd={props.disableAdd}
      title={props.title}
      showParentInformation={true}
      relationshipDisplay={{
        relationshipIdLabel: 'File #',
        relationshipTypeLabel: 'File Type',
        searchParentIdLabel: 'File Number',
        searchParentTypeLabel: 'File Type',
      }}
    />
  );
};

export default DocumentFileListContainer;
