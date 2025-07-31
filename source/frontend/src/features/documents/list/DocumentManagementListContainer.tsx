import { useCallback, useEffect, useMemo, useState } from 'react';

import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { exists, getFilePropertyName, relationshipTypeToPathName } from '@/utils';

import { DocumentRow } from '../ComposedDocument';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { IDocumentListContainerProps } from './DocumentListContainer';
import DocumentListView from './DocumentListView';
import { DocRelation } from './models';

const DocumentManagementListContainer: React.FunctionComponent<
  IDocumentListContainerProps
> = props => {
  const pathGenerator = usePathGenerator();

  const isMounted = useIsMounted();

  const [activityDocuments, setActivityDocuments] = useState<DocumentRow[]>([]);
  const [propertyDocuments, setPropertyDocuments] = useState<DocumentRow[]>([]);

  const [managementActivities, setManagementActivities] = useState<
    ApiGen_Concepts_ManagementActivity[]
  >([]);

  const [managementProperties, setManagementProperties] = useState<
    ApiGen_Concepts_ManagementFileProperty[]
  >([]);

  const {
    getManagementActivities: { execute: getActivities, loading: activitiesLoading },
  } = useManagementActivityRepository();

  const {
    getManagementProperties: { execute: getProperties, loading: propertiesLoading },
  } = useManagementFileRepository();

  const { retrieveDocumentRelationship, retrieveDocumentRelationshipLoading } =
    useDocumentRelationshipProvider();

  const retrieveActivities = useCallback(async () => {
    const result = await getActivities(Number(props.parentId));
    if (exists(result) && isMounted()) {
      setManagementActivities(result);
    }
  }, [getActivities, props.parentId, isMounted]);

  const retrieveProperties = useCallback(async () => {
    const result = await getProperties(Number(props.parentId));
    if (exists(result) && isMounted()) {
      setManagementProperties(result);
    }
  }, [getProperties, props.parentId, isMounted]);

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

  const retrievePropertyDocuments = useCallback(
    async (properties: ApiGen_Concepts_ManagementFileProperty[]) => {
      if (!exists(properties)) {
        return;
      }

      const docRelations: DocRelation[] = properties?.map(x => {
        const name = getFilePropertyName(x);
        return {
          id: x.propertyId,
          relationType: ApiGen_CodeTypes_DocumentRelationType.Properties,
          fileNumber: `${name.value}`,
        };
      });

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
        setPropertyDocuments([...documentRows]);
      }
    },
    [retrieveDocumentRelationship, isMounted],
  );

  useEffect(() => {
    retrieveActivities();
    retrieveProperties();
  }, [retrieveActivities, retrieveProperties]);

  useEffect(() => {
    retrieveActivitiesDocuments(managementActivities);
  }, [retrieveActivitiesDocuments, managementActivities]);

  useEffect(() => {
    retrievePropertyDocuments(managementProperties);
  }, [retrievePropertyDocuments, managementProperties]);

  const handleDocumentsRefresh = async () => {
    retrieveActivitiesDocuments(managementActivities);
  };

  const handleViewParent = async (
    relationshipType: ApiGen_CodeTypes_DocumentRelationType,
    parentId: number,
  ) => {
    if (relationshipType === ApiGen_CodeTypes_DocumentRelationType.ManagementActivities) {
      const file = relationshipTypeToPathName(props.relationshipType);
      const detail = relationshipTypeToPathName(relationshipType);
      pathGenerator.showDetail(file, Number(props.parentId), detail, parentId, false);
    } else {
      const file = relationshipTypeToPathName(relationshipType);
      pathGenerator.showFile(file, parentId);
    }
  };

  const documentResults = useMemo(() => {
    return [...activityDocuments, ...propertyDocuments];
  }, [activityDocuments, propertyDocuments]);

  const relationshipTypes = useMemo(() => {
    return [...new Set(documentResults?.map(x => x.relationshipType) ?? [])];
  }, [documentResults]);

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      relationshipTypes={relationshipTypes}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading || activitiesLoading || propertiesLoading}
      documentResults={documentResults}
      onDelete={undefined}
      onSuccess={undefined}
      onRefresh={handleDocumentsRefresh}
      onViewParent={handleViewParent}
      disableAdd={props.disableAdd}
      title={props.title}
      showParentInformation={true}
      relationshipDisplay={{
        relationshipIdLabel: 'Association',
        relationshipTypeLabel: 'Association Type',
        searchParentIdLabel: 'Association Name',
        searchParentTypeLabel: 'Association Type',
      }}
      data-testId="adhoc-properties-documents-list"
      canEditDocuments={false}
    />
  );
};

export default DocumentManagementListContainer;
