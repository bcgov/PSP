import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import * as React from 'react';

interface IResearchDocumentsTabProps {
  researchFileId: number;
}

const ResearchDocumentsTab: React.FunctionComponent<IResearchDocumentsTabProps> = ({
  researchFileId,
}) => {
  return (
    <>
      <DocumentListContainer
        title="File Documents"
        parentId={researchFileId}
        relationshipType={DocumentRelationshipType.RESEARCH_FILES}
      />
      {false && (
        <DocumentListContainer
          title="Activity Documents"
          parentId={researchFileId}
          relationshipType={DocumentRelationshipType.RESEARCH_FILE_ACTIVITIES}
          disableAdd
        />
      )}
    </>
  );
};

export default ResearchDocumentsTab;
