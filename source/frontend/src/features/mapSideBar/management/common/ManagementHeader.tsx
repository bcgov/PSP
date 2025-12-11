import React from 'react';
import { Col } from 'react-bootstrap';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller, StyledRow } from '@/components/common/HeaderField/styles';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { exists } from '@/utils';
import { formatMinistryProject } from '@/utils/formUtils';

import HistoricalNumbersContainer from '../../shared/header/HistoricalNumberContainer';
import { HistoricalNumberSectionView } from '../../shared/header/HistoricalNumberSectionView';
import { StyledLeftHeaderPane } from '../../shared/header/styles';

export interface IManagementHeaderProps {
  managementFile?: ApiGen_Concepts_ManagementFile;

  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const ManagementHeader: React.FunctionComponent<
  React.PropsWithChildren<IManagementHeaderProps>
> = ({ managementFile, lastUpdatedBy }) => {
  const leftColumnWidth = '8';
  const leftColumnLabel = '4';

  const propertyIds = managementFile?.fileProperties?.map(fp => fp.propertyId) ?? [];

  return (
    <StyledRow className="no-gutters" data-testid="mgmt-file-header">
      <StyledLeftHeaderPane xl="8" xs="12">
        <HeaderField
          label="File #:"
          labelWidth={{ xs: leftColumnLabel }}
          contentWidth={{ xs: leftColumnWidth }}
        >
          <div data-testid="mgmt-fileId">M-{managementFile?.id?.toString()}</div>
        </HeaderField>
        <HeaderField
          label="File name:"
          labelWidth={{ xs: leftColumnLabel }}
          contentWidth={{ xs: leftColumnWidth }}
        >
          <div data-testid="mgmt-file-name">{managementFile?.fileName}</div>
        </HeaderField>
        <HeaderField
          label="Ministry project:"
          labelWidth={{ xs: leftColumnLabel }}
          contentWidth={{ xs: leftColumnWidth }}
        >
          <div data-testid="mgmt-file-project">
            {formatMinistryProject(
              managementFile?.project?.code,
              managementFile?.project?.description,
            )}
          </div>
        </HeaderField>
        <HeaderField
          label="Ministry product:"
          labelWidth={{ xs: leftColumnLabel }}
          valueTestId={'acq-header-product-val'}
          contentWidth={{ xs: leftColumnWidth }}
        >
          <div data-testid="mgmt-file-product">
            {managementFile?.product && (
              <>
                {managementFile.product.code} - {managementFile.product.description}
              </>
            )}
          </div>
        </HeaderField>
        <HistoricalNumbersContainer
          propertyIds={propertyIds}
          View={HistoricalNumberSectionView}
          labelWidth={{ xs: leftColumnLabel }}
          contentWidth={{ xs: leftColumnWidth }}
        />
      </StyledLeftHeaderPane>
      <Col>
        <StyledFiller>
          <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={managementFile} />
          {exists(managementFile?.fileStatusTypeCode) && (
            <StatusField preText="File:" statusCodeType={managementFile.fileStatusTypeCode} />
          )}
        </StyledFiller>
      </Col>
    </StyledRow>
  );
};

export default ManagementHeader;
