import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { formatMinistryProject } from '@/utils/formUtils';

import HistoricalNumbersContainer from '../../shared/header/HistoricalNumberContainer';
import HistoricalNumberFieldView from '../../shared/header/HistoricalNumberSectionView';

export interface IAcquisitionHeaderProps {
  acquisitionFile?: ApiGen_Concepts_AcquisitionFile;
  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const AcquisitionHeader: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionHeaderProps>
> = ({ acquisitionFile, lastUpdatedBy }) => {
  const leftColumnWidth = '7';
  const leftColumnLabel = '3';

  const propertyIds = acquisitionFile?.fileProperties?.map(fp => fp.propertyId) ?? [];

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
        <HistoricalNumbersContainer propertyIds={propertyIds} View={HistoricalNumberFieldView} />
      </Col>
      <Col xs="5">
        <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={acquisitionFile} />
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
