import { UserNameTooltip } from 'components/common/UserNameTooltip';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

export interface IAcquisitionHeaderProps {
  acquisitionFile?: Api_AcquisitionFile;
}

export const AcquisitionHeader: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionHeaderProps>
> = ({ acquisitionFile }) => {
  const leftColumnWidth = '7';
  const leftColumnLabel = '3';

  return (
    <StyledRow className="no-gutters">
      <Col xs={leftColumnWidth}>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="File:" labelWidth={leftColumnLabel} contentWidth="9">
              {acquisitionFile?.fileNumber} - {acquisitionFile?.fileName}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="Ministry project:" labelWidth={leftColumnLabel} contentWidth="9">
              {formatMinistryProject(
                acquisitionFile?.project?.code,
                acquisitionFile?.project?.description,
              )}
            </HeaderField>
          </Col>
        </Row>
      </Col>
      <Col xs="5">
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Created: <strong>{prettyFormatDate(acquisitionFile?.appCreateTimestamp)}</strong> by{' '}
              <UserNameTooltip
                userName={acquisitionFile?.appCreateUserid}
                userGuid={acquisitionFile?.appCreateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Last updated:{' '}
              <strong>{prettyFormatDate(acquisitionFile?.appLastUpdateTimestamp)}</strong> by{' '}
              <UserNameTooltip
                userName={acquisitionFile?.appLastUpdateUserid}
                userGuid={acquisitionFile?.appLastUpdateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              {acquisitionFile?.fileStatusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
      </Col>
    </StyledRow>
  );
};

export default AcquisitionHeader;

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

function formatMinistryProject(projectNumber?: string | null, projectName?: string | null) {
  const formattedValue = [projectNumber, projectName].filter(x => x).join(' - ');
  return formattedValue;
}
