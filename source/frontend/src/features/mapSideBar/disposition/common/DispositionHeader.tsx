import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';

import HistoricalNumbersContainer from '../../shared/header/HistoricalNumberContainer';
import HistoricalNumberFieldView from '../../shared/header/HistoricalNumberSectionView';

export interface IDispositionHeaderProps {
  dispositionFile?: ApiGen_Concepts_DispositionFile;

  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const DispositionHeader: React.FunctionComponent<
  React.PropsWithChildren<IDispositionHeaderProps>
> = ({ dispositionFile, lastUpdatedBy }) => {
  const leftColumnWidth = '7';
  const leftColumnLabel = '3';

  const propertyIds = dispositionFile?.fileProperties?.map(fp => fp.propertyId) ?? [];

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
        <HistoricalNumbersContainer propertyIds={propertyIds} View={HistoricalNumberFieldView} />
      </Col>
      <Col xs="5">
        <Row className="no-gutters">
          <Col className="text-right">
            <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={dispositionFile} />
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
