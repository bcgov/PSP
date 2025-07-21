import { useCallback, useEffect, useMemo, useState } from 'react';

import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { useManagementActivityPropertyRepository } from '@/hooks/repositories/useManagementActivityPropertyRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
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

  const [activityDocuments, setActivityDocuments] = useState<DocumentRow[]>([]);
  const [fileDocuments, setFileDocuments] = useState<DocumentRow[]>([]);

  const [managementActivities, setManagementActivities] = useState<
    ApiGen_Concepts_ManagementActivity[]
  >([]);

  const [fileAssociations, setFileAssociations] =
    useState<ApiGen_Concepts_PropertyAssociations>(null);

  const { execute: getPropertyAssociations, loading: associationsLoading } =
    usePropertyAssociations();

  const {
    getActivities: { execute: getActivities, loading: loadingActivities },
  } = useManagementActivityPropertyRepository();

  const { retrieveDocumentRelationship, retrieveDocumentRelationshipLoading } =
    useDocumentRelationshipProvider();

  const retrievePropertyAssociations = useCallback(async () => {
    const result = await getPropertyAssociations(Number(props.parentId));
    if (exists(result) && isMounted()) {
      setFileAssociations(result);
    }
  }, [getPropertyAssociations, props.parentId, isMounted]);

  const retrieveActivities = useCallback(async () => {
    const result = await getActivities(Number(props.parentId));
    if (exists(result) && isMounted()) {
      setManagementActivities(result);
    }
  }, [getActivities, props.parentId, isMounted]);

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
        if (exists(documents)) {
          documentRows = documentRows.concat(
            documents
              .filter((x): x is ApiGen_Concepts_DocumentRelationship => exists(x?.document))
              .map(x => DocumentRow.fromApi(x, docRelation.fileNumber)),
          );
        }
      }
      if (isMounted()) {
        setFileDocuments([...documentRows]);
      }
    },
    [retrieveDocumentRelationship, isMounted],
  );

  const retrieveActivitiesDocuments = useCallback(
    async (activities: ApiGen_Concepts_ManagementActivity[]) => {
      if (!exists(activities)) {
        return;
      }

      const docRelations: DocRelation[] = activities?.map(x => ({
        id: x.id,
        relationType: ApiGen_CodeTypes_DocumentRelationType.ManagementActivities,
        fileNumber: x.description,
      }));

      let documentRows: DocumentRow[] = [];
      for (const docRelation of docRelations.filter(exists)) {
        const documents = await retrieveDocumentRelationship(
          docRelation.relationType,
          docRelation.id.toString(),
        );
        if (exists(documents)) {
          documentRows = documentRows.concat(
            documents
              .filter((x): x is ApiGen_Concepts_DocumentRelationship => !!x?.document)
              .map(x => DocumentRow.fromApi(x, docRelation.fileNumber)),
          );
        }
      }
      if (isMounted()) {
        setActivityDocuments([...documentRows]);
      }
    },
    [retrieveDocumentRelationship, isMounted],
  );

  useEffect(() => {
    retrievePropertyAssociations();
    retrieveActivities();
  }, [retrieveActivities, retrievePropertyAssociations]);

  useEffect(() => {
    retrieveAssociationDocuments(fileAssociations);
  }, [retrieveAssociationDocuments, fileAssociations]);

  useEffect(() => {
    retrieveActivitiesDocuments(managementActivities);
  }, [managementActivities, retrieveActivitiesDocuments]);

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

  const documentResults = useMemo(() => {
    return [...activityDocuments, ...fileDocuments];
  }, [activityDocuments, fileDocuments]);

  const relationshipTypes = useMemo(() => {
    return [...new Set(documentResults?.map(x => x.relationshipType) ?? [])];
  }, [documentResults]);

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      relationshipTypes={relationshipTypes}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading || loadingActivities || associationsLoading}
      documentResults={documentResults}
      onDelete={undefined}
      onSuccess={undefined}
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
      data-testId="pims-files-document-list"
      canEditDocuments={true}
    />
  );
};

export default DocumentFileListContainer;
