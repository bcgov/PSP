import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller } from '@/components/common/HeaderField/styles';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { exists } from '@/utils';

import HistoricalNumbersContainer from '../../shared/header/HistoricalNumberContainer';
import { HistoricalNumberSectionView } from '../../shared/header/HistoricalNumberSectionView';
import { StyledLeftHeaderPane } from '../../shared/header/styles';

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
    <Container>
      <Row className="no-gutters">
        <StyledLeftHeaderPane xs={leftColumnWidth}>
          <HeaderField label="File:" labelWidth={{ xs: leftColumnLabel }} contentWidth={{ xs: 9 }}>
            D-{dispositionFile?.fileNumber}
          </HeaderField>
          <HistoricalNumbersContainer
            propertyIds={propertyIds}
            View={HistoricalNumberSectionView}
          />
        </StyledLeftHeaderPane>

        <Col>
          <StyledFiller>
            <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={dispositionFile} />
            {exists(dispositionFile?.fileStatusTypeCode) && (
              <StatusField preText="File:" statusCodeType={dispositionFile.fileStatusTypeCode} />
            )}
          </StyledFiller>
        </Col>
      </Row>
    </Container>
  );
};

export default DispositionHeader;

const Container = styled.div`
  margin-top: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
  max-height: 25rem;
  overflow-y: auto;
  overflow-x: hidden;
`;
