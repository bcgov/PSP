import { DocumentTypes } from 'constants/documentTypes';
import DocumentListView from 'features/documents/list/DocumentListView';
import { mockDocumentsResponse } from 'mocks/mockDocuments';
import { Container, Row } from 'react-bootstrap';

export const TestDocuments: React.FC = () => {
  return (
    <Container>
      <Row className="py-5">
        <DocumentListView
          isLoading={false}
          documentResults={mockDocumentsResponse()}
          entityId={1}
          documentType={DocumentTypes.ACTIVITY}
          hideFilters={false}
        />
      </Row>
    </Container>
  );
};
