import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

export interface IResearchHeaderProps {
  researchFile?: Api_ResearchFile;
}

const ResearchHeader: React.FunctionComponent<IResearchHeaderProps> = props => {
  const leftColumnWidth = '6';
  const leftColumnLabel = '4';
  const researchFile = props.researchFile;

  const region = researchFile?.researchProperties?.map(x => x.property?.region).join(', ');
  const district = researchFile?.researchProperties?.map(x => x.property?.district).join(', ');
  return (
    <Row>
      <Col>
        <Row>
          <Col xs={leftColumnWidth}>
            <HeaderField label="Research file #:" labelWidth={leftColumnLabel}>
              {researchFile?.rfileNumber}
            </HeaderField>
          </Col>
          <Col className="text-right">
            Created: <strong>{prettyFormatDate(researchFile?.appCreateTimestamp)}</strong> by
            <span> {researchFile?.appCreateUserid} </span>
          </Col>
        </Row>
        <Row>
          <Col xs={leftColumnWidth}>
            <HeaderField label="R-file name:" labelWidth={leftColumnLabel}>
              {researchFile?.name}
            </HeaderField>
          </Col>
          <Col className="text-right">
            Last updated: <strong>{prettyFormatDate(researchFile?.appLastUpdateTimestamp)}</strong>{' '}
            by <span>{researchFile?.appLastUpdateUserid}</span>
          </Col>
        </Row>
        <Row>
          <Col xs={leftColumnWidth}>
            <HeaderField label="MoTI region:" labelWidth={leftColumnLabel}>
              {region}
            </HeaderField>
          </Col>
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              {researchFile?.researchFileStatusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
        <Row>
          <Col xs={leftColumnWidth}>
            <HeaderField label="Ministry district:" labelWidth={leftColumnLabel}>
              {district}
            </HeaderField>
          </Col>
        </Row>
        <StyledDivider />
      </Col>
    </Row>
  );
};

export default ResearchHeader;

const StyledDivider = styled.div`
  margin-top: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;
