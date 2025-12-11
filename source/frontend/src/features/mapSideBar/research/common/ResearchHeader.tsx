import { Col } from 'react-bootstrap';
import styled from 'styled-components';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller, StyledRow } from '@/components/common/HeaderField/styles';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { exists } from '@/utils';

import HistoricalNumbersContainer from '../../shared/header/HistoricalNumberContainer';
import { HistoricalNumberSectionView } from '../../shared/header/HistoricalNumberSectionView';

export interface IResearchHeaderProps {
  researchFile?: ApiGen_Concepts_ResearchFile;
  lastUpdatedBy: Api_LastUpdatedBy | null;
}

const ResearchHeader: React.FunctionComponent<
  React.PropsWithChildren<IResearchHeaderProps>
> = props => {
  const leftColumnWidth = '8';
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

  function removeDuplicates(
    list: (ApiGen_Base_CodeType<number> | undefined | null)[],
  ): ApiGen_Base_CodeType<number>[] {
    return list
      .filter((x): x is ApiGen_Base_CodeType<number> => exists(x) && x.description !== '')
      .reduce((acc: ApiGen_Base_CodeType<number>[], curr: ApiGen_Base_CodeType<number>) => {
        if (acc.find(x => curr.id === x.id) === undefined) {
          acc.push(curr);
        }
        return acc;
      }, []);
  }

  const propertyIds = researchFile?.fileProperties?.map(fp => fp.propertyId) ?? [];

  return (
    <StyledRow className="no-gutters">
      <StyledLeftHeaderPane xl={leftColumnWidth} xs="12">
        <HeaderField label="File #:" labelWidth={{ xs: leftColumnLabel }} contentWidth={{ xs: 9 }}>
          {researchFile?.fileNumber}
        </HeaderField>
        <HeaderField
          label="File name:"
          labelWidth={{ xs: leftColumnLabel }}
          contentWidth={{ xs: 9 }}
        >
          {researchFile?.fileName}
        </HeaderField>
        <HeaderField
          label="MOTT region:"
          labelWidth={{ xs: leftColumnLabel }}
          contentWidth={{ xs: 9 }}
        >
          {regions}
        </HeaderField>
        <HeaderField
          label="Ministry district:"
          labelWidth={{ xs: leftColumnLabel }}
          contentWidth={{ xs: 9 }}
        >
          {districts}
        </HeaderField>
        <HistoricalNumbersContainer propertyIds={propertyIds} View={HistoricalNumberSectionView} />
      </StyledLeftHeaderPane>
      <Col>
        <StyledFiller>
          <AuditSection lastUpdatedBy={props.lastUpdatedBy} baseAudit={researchFile} />
          {exists(researchFile?.fileStatusTypeCode) && (
            <StatusField preText="File:" statusCodeType={researchFile.fileStatusTypeCode} />
          )}
        </StyledFiller>
      </Col>
    </StyledRow>
  );
};

export default ResearchHeader;

const StyledLeftHeaderPane = styled(Col)`
  max-width: 60rem;
  //border: 1px solid red;
`;
