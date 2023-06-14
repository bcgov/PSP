import * as React from 'react';

import { Claims } from '@/constants/claims';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

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
        parentId={acquisitionFileId.toString()}
        relationshipType={DocumentRelationshipType.ACQUISITION_FILES}
      />
      {hasClaim(Claims.ACTIVITY_VIEW) && (
        <DocumentListContainer
          title="Activity Documents"
          parentId={acquisitionFileId.toString()}
          relationshipType={DocumentRelationshipType.ACQUISITION_FILE_ACTIVITIES}
          disableAdd
        />
      )}
    </>
  );
};

export default AcquisitionDocumentsTab;
