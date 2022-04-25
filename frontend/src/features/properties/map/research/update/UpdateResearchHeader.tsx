import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { prettyFormatDate } from 'utils';

interface IUpdateResearchHeaderProps {
  researchFile?: Api_ResearchFile;
}

const UpdateResearchHeader: React.FunctionComponent<IUpdateResearchHeaderProps> = props => {
  const lefttColumnWidth = '6';
  const leftColumnLabel = '4';
  const researchFile = props.researchFile;

  const region = researchFile?.researchProperties?.map(x => x.property?.region).join(', ');
  const district = researchFile?.researchProperties?.map(x => x.property?.district).join(', ');
  return (
    <Row>
      <Col>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="Research file #:" labelWidth={leftColumnLabel}>
              {researchFile?.rfileNumber}
            </HeaderField>
          </Col>
          <Col className="text-right">
            Created: <strong>{prettyFormatDate(researchFile?.appCreateTimestamp)}</strong> by{' '}
            {researchFile?.appCreateUserid}
          </Col>
        </Row>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="R-file name:" labelWidth={leftColumnLabel}>
              {researchFile?.name}
            </HeaderField>
          </Col>
          <Col className="text-right">
            Last updated: <strong>{prettyFormatDate(researchFile?.appLastUpdateTimestamp)}</strong>{' '}
            by {researchFile?.appLastUpdateUserid}
          </Col>
        </Row>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="MoTI region:" labelWidth={leftColumnLabel}>
              {region}
            </HeaderField>
          </Col>
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              {researchFile?.statusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
        <Row>
          <Col xs={lefttColumnWidth}>
            <HeaderField label="Ministry district:" labelWidth={leftColumnLabel}>
              {district}
            </HeaderField>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default UpdateResearchHeader;
