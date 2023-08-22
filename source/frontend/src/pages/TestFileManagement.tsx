import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';

export const TestFileManagement: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
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
            parentId={'1'}
            relationshipType={DocumentRelationshipType.ACQUISITION_FILES}
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
