import React from 'react';
import { Col } from 'react-bootstrap';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller, StyledRow } from '@/components/common/HeaderField/styles';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists } from '@/utils';
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
  const leftColumnLabel = '3';

  const propertyIds = acquisitionFile?.fileProperties?.map(fp => fp.propertyId) ?? [];

  return (
    <StyledRow className="no-gutters">
      <Col xs="8">
        <HeaderField label="File:" labelWidth={leftColumnLabel} contentWidth="9">
          {acquisitionFile?.fileNumber} - {acquisitionFile?.fileName}
        </HeaderField>
        <HeaderField label="Ministry project:" labelWidth={leftColumnLabel} contentWidth="9">
          {formatMinistryProject(
            acquisitionFile?.project?.code,
            acquisitionFile?.project?.description,
          )}
        </HeaderField>
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
        <HistoricalNumbersContainer
          propertyIds={propertyIds}
          displayValuesOnly={false}
          View={HistoricalNumberFieldView}
        />
      </Col>
      <Col>
        <StyledFiller>
          <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={acquisitionFile} />
          {exists(acquisitionFile?.fileStatusTypeCode) && (
            <StatusField statusCodeType={acquisitionFile.fileStatusTypeCode} />
          )}
        </StyledFiller>
      </Col>
    </StyledRow>
  );
};

export default AcquisitionHeader;
