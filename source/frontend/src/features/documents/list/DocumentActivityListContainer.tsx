import { useCallback, useEffect, useMemo, useState } from 'react';

import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { exists, getApiPropertyName, relationshipTypeToPathName } from '@/utils';

import { DocumentRow } from '../ComposedDocument';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { IDocumentListContainerProps } from './DocumentListContainer';
import DocumentListView from './DocumentListView';
import { DocRelation } from './models';

const DocumentActivityListContainer: React.FunctionComponent<
  IDocumentListContainerProps
> = props => {
  const pathGenerator = usePathGenerator();

  const isMounted = useIsMounted();

  const [documentResults, setDocumentResults] = useState<DocumentRow[]>([]);

  const [activity, setActivity] = useState<ApiGen_Concepts_ManagementActivity>(null);

  const {
    getPropertyManagementActivity: { execute: getManagementActivity, loading: getActivityLoading },
  } = useManagementActivityRepository();

  const {
    getAllPropertiesById: { execute: getProperties, loading: getPropertiesLoading },
  } = usePimsPropertyRepository();

  const { retrieveDocumentRelationship, retrieveDocumentRelationshipLoading } =
    useDocumentRelationshipProvider();

  const retrieveActivity = useCallback(async () => {
    const result = await getManagementActivity(Number(props.parentId));
    if (exists(result) && isMounted()) {
      const propertyResult = await getProperties(
        result.activityProperties.map(ap => ap.propertyId),
      );
      result.activityProperties.forEach(ap => {
        const foundProperty = propertyResult.find(p => p.id === ap.propertyId);
        ap.property = foundProperty;
      });
      setActivity(result);
    }
  }, [getManagementActivity, props.parentId, isMounted, getProperties]);

  const retrieveActivitiesDocuments = useCallback(
    async (activity: ApiGen_Concepts_ManagementActivity) => {
      if (!exists(activity)) {
        return;
      }

      const docRelations: DocRelation[] = activity?.activityProperties?.map(x => ({
        id: x.propertyId,
        relationType: ApiGen_CodeTypes_DocumentRelationType.Properties,
        fileNumber: getApiPropertyName(x.property).value,
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
        setDocumentResults([...documentRows]);
      }
    },
    [retrieveDocumentRelationship, isMounted],
  );

  useEffect(() => {
    retrieveActivity();
  }, [retrieveActivity]);

  useEffect(() => {
    retrieveActivitiesDocuments(activity);
  }, [retrieveActivitiesDocuments, activity]);

  const handleDocumentsRefresh = async () => {
    retrieveActivitiesDocuments(activity);
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

  const editDocumentsEnabled = !props.statusSolver || props.statusSolver?.canEditDocuments();

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      relationshipTypes={relationshipTypes}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading || getActivityLoading || getPropertiesLoading}
      documentResults={documentResults}
      onDelete={undefined}
      onSuccess={undefined}
      onRefresh={handleDocumentsRefresh}
      onViewParent={handleViewParent}
      disableAdd={props.disableAdd}
      title={props.title}
      showParentInformation={true}
      data-testId="document-activity-list"
      canEditDocuments={editDocumentsEnabled}
    />
  );
};

export default DocumentActivityListContainer;
