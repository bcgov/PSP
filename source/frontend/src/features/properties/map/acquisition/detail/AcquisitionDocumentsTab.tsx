import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import * as React from 'react';

interface IAcquisitionDocumentsTabProps {
  acquisitionFileId: number;
}

const AcquisitionDocumentsTab: React.FunctionComponent<IAcquisitionDocumentsTabProps> = ({
  acquisitionFileId,
}) => {
  return (
    <>
      <DocumentListContainer
        title="File Documents"
        parentId={acquisitionFileId}
        relationshipType={DocumentRelationshipType.ACQUISITION_FILES}
      />
      <DocumentListContainer
        title="Activity Documents"
        parentId={acquisitionFileId}
        relationshipType={DocumentRelationshipType.ACQUISITION_FILE_ACTIVITIES}
        disableAdd
      />
    </>
  );
};

export default AcquisitionDocumentsTab;
