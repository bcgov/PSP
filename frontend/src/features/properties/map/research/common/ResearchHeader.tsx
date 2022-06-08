import { UserNameTooltip } from 'components/common/UserNameTooltip';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import Api_TypeCode from 'models/api/TypeCode';
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

  const regions = researchFile?.researchProperties
    ?.map(x => x.property?.region)
    .filter((x): x is Api_TypeCode<number> => !!x && x.description !== '')
    .reduce((acc: Api_TypeCode<number>[], curr: Api_TypeCode<number>) => {
      if (acc.find(x => curr.id === x.id) === undefined) {
        acc.push(curr);
      }
      return acc;
    }, [])
    .map(x => x.description)
    .join(', ');

  const districts = researchFile?.researchProperties
    ?.map(x => x.property?.district)
    .filter((x): x is Api_TypeCode<number> => !!x && x.description !== '')
    .reduce((acc: Api_TypeCode<number>[], curr: Api_TypeCode<number>) => {
      if (acc.find(x => curr.id === x.id) === undefined) {
        acc.push(curr);
      }
      return acc;
    }, [])
    .map(x => x.description)
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
              {regions}
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
              {districts}
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
