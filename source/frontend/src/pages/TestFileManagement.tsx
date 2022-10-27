import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

export const TestFileManagement: React.FunctionComponent = () => {
  return (
    <StyledBody>
      <Row className="no-gutters">
        <Col>
          <h1>File Management test page</h1>
        </Col>
      </Row>
      <Row className="py-5 no-gutters w-100">
        <Col>
          <DocumentListContainer
            parentId={1}
            relationshipType={DocumentRelationshipType.ACTIVITIES}
          />
        </Col>
      </Row>
    </StyledBody>
  );
};

const StyledBody = styled.div`
  padding: 10rem;
  width: 100%;
  position: relative;
  overflow: auto;
`;
