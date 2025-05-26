import { noop } from 'lodash';
import { useCallback, useEffect, useMemo, useState } from 'react';

import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { exists, relationshipTypeToPathName } from '@/utils';

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

  const [documentResults, setDocumentResults] = useState<DocumentRow[]>([]);

  const [managementActivities, setManagementActivities] = useState<
    ApiGen_Concepts_PropertyActivity[]
  >([]);

  const {
    getManagementActivities: { execute: getActivities, loading: activitiesLoading },
  } = useManagementActivityRepository();

  const { retrieveDocumentRelationship, retrieveDocumentRelationshipLoading } =
    useDocumentRelationshipProvider();

  const retrieveActivities = useCallback(async () => {
    const result = await getActivities(Number(props.parentId));
    if (exists(result) && isMounted()) {
      setManagementActivities(result);
    }
  }, [getActivities, props.parentId, isMounted]);

  const retrieveActivitiesDocuments = useCallback(
    async (activities: ApiGen_Concepts_PropertyActivity[]) => {
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
    retrieveActivities();
  }, [retrieveActivities]);

  useEffect(() => {
    retrieveActivitiesDocuments(managementActivities);
  }, [retrieveActivitiesDocuments, managementActivities]);

  const handleDocumentsRefresh = async () => {
    retrieveActivitiesDocuments(managementActivities);
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
      isLoading={retrieveDocumentRelationshipLoading || activitiesLoading}
      documentResults={documentResults}
      onDelete={undefined}
      onSuccess={noop}
      onRefresh={handleDocumentsRefresh}
      onViewParent={handleViewParent}
      disableAdd={props.disableAdd}
      title={props.title}
      showParentInformation={true}
    />
  );
};

export default DocumentManagementListContainer;
