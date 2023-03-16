import { Claims } from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import * as React from 'react';

interface IAcquisitionDocumentsTabProps {
  acquisitionFileId: number;
}

const AcquisitionDocumentsTab: React.FunctionComponent<IAcquisitionDocumentsTabProps> = ({
  acquisitionFileId,
}) => {
  const { hasClaim } = useKeycloakWrapper();

  return (
    <>
      <DocumentListContainer
        title="File Documents"
        parentId={acquisitionFileId}
        relationshipType={DocumentRelationshipType.ACQUISITION_FILES}
      />
      {hasClaim(Claims.ACTIVITY_VIEW) && (
        <DocumentListContainer
          title="Activity Documents"
          parentId={acquisitionFileId}
          relationshipType={DocumentRelationshipType.ACQUISITION_FILE_ACTIVITIES}
          disableAdd
        />
      )}
    </>
  );
};

export default AcquisitionDocumentsTab;
