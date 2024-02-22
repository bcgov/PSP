import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { prettyFormatUTCDate } from '@/utils';
import { formatMinistryProject } from '@/utils/formUtils';

export interface IAcquisitionHeaderProps {
  acquisitionFile?: ApiGen_Concepts_AcquisitionFile;
  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const AcquisitionHeader: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionHeaderProps>
> = ({ acquisitionFile, lastUpdatedBy }) => {
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
        <Row className="no-gutters">
          <Col>
            <HeaderField
              label="Ministry product:"
              labelWidth={leftColumnLabel}
              valueTestId={'acq-header-product-val'}
              contentWidth="9"
            >
              {acquisitionFile?.product && (
                <>
                  {acquisitionFile.product.code} - {acquisitionFile.product.description}
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
              Created: <strong>{prettyFormatUTCDate(acquisitionFile?.appCreateTimestamp)}</strong>{' '}
              by{' '}
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
