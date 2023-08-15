import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { Api_ResearchFile } from '@/models/api/ResearchFile';
import Api_TypeCode from '@/models/api/TypeCode';
import { prettyFormatUTCDate } from '@/utils';

export interface IResearchHeaderProps {
  researchFile?: Api_ResearchFile;
}

const ResearchHeader: React.FunctionComponent<
  React.PropsWithChildren<IResearchHeaderProps>
> = props => {
  const leftColumnWidth = '7';
  const leftColumnLabel = '3';
  const researchFile = props.researchFile;

  const regions = removeDuplicates(researchFile?.fileProperties?.map(x => x.property?.region) || [])
    .map(x => x.description)
    .join(', ');

  const districts = removeDuplicates(
    researchFile?.fileProperties?.map(x => x.property?.district) || [],
  )
    .map(x => x.description)
    .join(', ');

  function removeDuplicates(list: (Api_TypeCode<number> | undefined)[]): Api_TypeCode<number>[] {
    return list
      .filter((x): x is Api_TypeCode<number> => !!x && x.description !== '')
      .reduce((acc: Api_TypeCode<number>[], curr: Api_TypeCode<number>) => {
        if (acc.find(x => curr.id === x.id) === undefined) {
          acc.push(curr);
        }
        return acc;
      }, []);
  }
  return (
    <StyledRow className="no-gutters">
      <Col xs={leftColumnWidth}>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="File #:" labelWidth={leftColumnLabel} contentWidth="9">
              {researchFile?.fileNumber}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="File name:" labelWidth={leftColumnLabel} contentWidth="9">
              {researchFile?.fileName}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="MoTI region:" labelWidth={leftColumnLabel} contentWidth="9">
              {regions}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="Ministry district:" labelWidth={leftColumnLabel} contentWidth="9">
              {districts}
            </HeaderField>
          </Col>
        </Row>
      </Col>
      <Col xs="5">
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Created: <strong>{prettyFormatUTCDate(researchFile?.appCreateTimestamp)}</strong> by{' '}
              <UserNameTooltip
                userName={researchFile?.appCreateUserid}
                userGuid={researchFile?.appCreateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Last updated:{' '}
              <strong>{prettyFormatUTCDate(researchFile?.appLastUpdateTimestamp)}</strong> by{' '}
              <UserNameTooltip
                userName={researchFile?.appLastUpdateUserid}
                userGuid={researchFile?.appLastUpdateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              {researchFile?.fileStatusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
      </Col>
    </StyledRow>
  );
};

export default ResearchHeader;

const StyledRow = styled(Row)`
  margin-top: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;

const StyleSmallText = styled.span`
  font-size: 0.87em;
  line-height: 1.9;
`;
