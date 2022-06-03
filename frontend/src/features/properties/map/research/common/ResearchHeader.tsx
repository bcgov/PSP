import { UserNameTooltip } from 'components/common/UserNameTooltip';
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
  const leftColumnWidth = '8';
  const leftColumnLabel = '3';
  const researchFile = props.researchFile;

  const region = researchFile?.researchProperties
    ?.map(x => x.property?.region?.description)
    .filter(x => x !== undefined && x !== '')
    .join(', ');
  const district = researchFile?.researchProperties
    ?.map(x => x.property?.district?.description)
    .filter(x => x !== undefined && x !== '')
    .join(', ');
  return (
    <Row className="no-gutters">
      <Col>
        <Row className="no-gutters">
          <Col xs={leftColumnWidth} className="">
            <HeaderField label="Research file #:" labelWidth={leftColumnLabel}>
              {researchFile?.rfileNumber}
            </HeaderField>
          </Col>
          <Col className="text-right">
            Created: <strong>{prettyFormatDate(researchFile?.appCreateTimestamp)}</strong> by{' '}
            <UserNameTooltip
              userName={researchFile?.appCreateUserid}
              userGuid={researchFile?.appCreateUserGuid}
            />
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col xs={leftColumnWidth}>
            <HeaderField label="R-file name:" labelWidth={leftColumnLabel}>
              {researchFile?.name}
            </HeaderField>
          </Col>
          <Col className="text-right">
            Last updated: <strong>{prettyFormatDate(researchFile?.appLastUpdateTimestamp)}</strong>{' '}
            by{' '}
            <UserNameTooltip
              userName={researchFile?.appLastUpdateUserid}
              userGuid={researchFile?.appLastUpdateUserGuid}
            />
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col xs={leftColumnWidth}>
            <HeaderField label="MoTI region:" labelWidth={leftColumnLabel} contentWidth={'9'}>
              {region}
            </HeaderField>
          </Col>
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              {researchFile?.researchFileStatusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col xs={leftColumnWidth}>
            <HeaderField label="Ministry district:" labelWidth={leftColumnLabel} contentWidth={'9'}>
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
