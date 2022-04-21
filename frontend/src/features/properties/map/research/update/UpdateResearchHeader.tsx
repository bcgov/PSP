import { InlineInput } from 'components/common/form/styles';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

const UpdateResearchHeader: React.FunctionComponent = () => {
  const lefttColumnWidth = '6';
  const leftColumnLabel = '4';
  return (
    <Row>
      <Col>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="Research file #:" labelWidth={leftColumnLabel}>
              R-12345
            </HeaderField>
          </Col>
          <Col className="text-right">
            Created: <strong>Mar 16, 2022</strong> by USERID
          </Col>
        </Row>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="R-file name:" labelWidth={leftColumnLabel}>
              Some file name here
            </HeaderField>
          </Col>
          <Col className="text-right">
            Created: <strong>Mar 16, 2022</strong> by USERID
          </Col>
        </Row>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="MoTI region:" labelWidth={leftColumnLabel}>
              South Coast
            </HeaderField>
          </Col>
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              Active
            </HeaderField>
          </Col>
        </Row>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="Ministry district:" labelWidth={leftColumnLabel}>
              Vancouver Island
            </HeaderField>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default UpdateResearchHeader;

const StyledSummarySection = styled.div`
  background-color: red;
`;

const LargeInlineInput = styled(InlineInput)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;
