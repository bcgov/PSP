import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { prettyFormatUTCDate } from '@/utils/dateUtils';
import { formatMinistryProject } from '@/utils/formUtils';

export interface IDispositionHeaderProps {
  dispositionFile?: Api_DispositionFile;
  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const DispositionHeader: React.FunctionComponent<
  React.PropsWithChildren<IDispositionHeaderProps>
> = ({ dispositionFile, lastUpdatedBy }) => {
  const leftColumnWidth = '7';
  const leftColumnLabel = '3';

  return (
    <StyledRow className="no-gutters">
      <Col xs={leftColumnWidth}>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="File:" labelWidth={leftColumnLabel} contentWidth="9">
              D-{dispositionFile?.fileNumber}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="Ministry project:" labelWidth={leftColumnLabel} contentWidth="9">
              {formatMinistryProject(
                dispositionFile?.project?.code,
                dispositionFile?.project?.description,
              )}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField
              label="Ministry product:"
              labelWidth={leftColumnLabel}
              valueTestId={'disp-header-product-val'}
              contentWidth="9"
            >
              {dispositionFile?.product && (
                <>
                  {dispositionFile.product.code} - {dispositionFile.product.description}
                </>
              )}
            </HeaderField>
          </Col>
        </Row>
      </Col>
      <Col xs="5">
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Created: <strong>{prettyFormatUTCDate(dispositionFile?.appCreateTimestamp)}</strong>{' '}
              by{' '}
              <UserNameTooltip
                userName={dispositionFile?.appCreateUserid}
                userGuid={dispositionFile?.appCreateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Last updated:{' '}
              <strong>{prettyFormatUTCDate(lastUpdatedBy?.appLastUpdateTimestamp)}</strong> by{' '}
              <UserNameTooltip
                userName={lastUpdatedBy?.appLastUpdateUserid}
                userGuid={lastUpdatedBy?.appLastUpdateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              {dispositionFile?.fileStatusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
      </Col>
    </StyledRow>
  );
};

export default DispositionHeader;

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
