import React from 'react';
import { Col } from 'react-bootstrap';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller, StyledRow } from '@/components/common/HeaderField/styles';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { exists } from '@/utils';

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
        <HeaderField label="File:" labelWidth={leftColumnLabel} contentWidth="9">
          D-{dispositionFile?.fileNumber}
        </HeaderField>
        <HistoricalNumbersContainer
          propertyIds={propertyIds}
          displayValuesOnly={false}
          View={HistoricalNumberFieldView}
        />
      </Col>
      <Col xs="5">
        <StyledFiller>
          <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={dispositionFile} />
          {exists(dispositionFile?.fileStatusTypeCode) && (
            <StatusField statusCodeType={dispositionFile.fileStatusTypeCode} />
          )}
        </StyledFiller>
      </Col>
    </StyledRow>
  );
};

export default DispositionHeader;
