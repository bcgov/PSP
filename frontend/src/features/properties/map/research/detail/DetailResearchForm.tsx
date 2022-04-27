import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledFormSection, StyledSectionHeader } from 'features/mapSideBar/tabs/SectionStyles';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';
import { formatApiPersonNames } from 'utils/personUtils';

interface DetailResearchFile {
  id?: number;
  name?: string;
  roadName?: string;
  roadAlias?: string;
  rfileNumber?: string;
  statusTypeCodeDescription?: string;
  requestDate?: string;
  requestDescription?: string;
  requestSourceDescription?: string;
  researchResult?: string;
  researchCompletionDate?: string;
  isExpropriation?: boolean;
  expropriationNotes?: string;
  requestSourceTypeDescription?: string;
  requestorName?: string;
  requestorOrganization?: string;
  researchFilePurposes?: string[];
}

export interface IDetailResearchFormProps {
  researchFile: Api_ResearchFile;
}

const DetailResearchForm: React.FunctionComponent<IDetailResearchFormProps> = props => {
  const detail: DetailResearchFile = {
    id: props.researchFile.id,
    name: props.researchFile.name,
    roadName: props.researchFile.roadName,
    roadAlias: props.researchFile.roadAlias,
    rfileNumber: props.researchFile.rfileNumber,
    statusTypeCodeDescription: props.researchFile.researchFileStatusTypeCode?.description,
    requestDate: props.researchFile.requestDate,
    requestDescription: props.researchFile.requestDescription,
    requestSourceDescription: props.researchFile.requestSourceDescription,
    researchResult: props.researchFile.researchResult,
    researchCompletionDate: props.researchFile.researchCompletionDate,
    isExpropriation: props.researchFile.isExpropriation,
    expropriationNotes: props.researchFile.expropriationNotes,
    requestSourceTypeDescription: props.researchFile.requestSourceType?.description,
  };

  if (props.researchFile.requestorPerson !== undefined) {
    detail.requestorName = formatApiPersonNames(props.researchFile.requestorPerson);
    var personOrganizations = props.researchFile.requestorPerson.personOrganizations;
    var organization =
      personOrganizations !== undefined && personOrganizations.length > 0
        ? personOrganizations[0].organization
        : undefined;
    detail.requestorOrganization = organization?.name;
  } else if (props.researchFile.requestorOrganization !== undefined) {
    detail.requestorName = props.researchFile.requestorOrganization.name;
  }

  function isString(str: string | undefined): str is string {
    return str !== undefined;
  }

  detail.researchFilePurposes =
    props.researchFile.researchFilePurposes !== undefined
      ? props.researchFile.researchFilePurposes
          .map(x => x.researchPurposeTypeCode?.description)
          .filter(isString)
      : [];

  return (
    <StyledSummarySection>
      <StyledFormSection>
        <StyledSectionHeader>Roads</StyledSectionHeader>
        <SectionField label="Road name">{detail.roadName}</SectionField>
        <SectionField label="Road alias">{detail.roadAlias}</SectionField>
      </StyledFormSection>
      <StyledFormSection>
        <StyledSectionHeader>Research Request</StyledSectionHeader>
        <SectionField label="Research purpose">
          {detail.researchFilePurposes.join(', ')}
        </SectionField>
        <SectionField label="Request date"> {prettyFormatDate(detail.requestDate)}</SectionField>
        <SectionField label="Source of request">{detail.requestSourceTypeDescription}</SectionField>
        <SectionField label="Requester">{detail.requestorName}</SectionField>
        {detail.requestorOrganization && (
          <SectionField label="Organization" className="pb-4">
            {detail.requestorOrganization ?? 'none'}
          </SectionField>
        )}
        <SectionField label="Description of request" />
        {detail.requestDescription}
      </StyledFormSection>
      <StyledFormSection>
        <StyledSectionHeader>Result</StyledSectionHeader>
        <SectionField label="Research completed on">
          {detail.researchCompletionDate !== undefined
            ? prettyFormatDate(detail.researchCompletionDate)
            : 'not complete'}
        </SectionField>
        <SectionField label="Result of request" />
        {detail.researchResult}
      </StyledFormSection>
      <StyledFormSection>
        <StyledSectionHeader>Expropriation</StyledSectionHeader>
        <SectionField label="Expropriation?">{detail.isExpropriation ? 'Yes' : 'No'}</SectionField>
        <SectionField label="Expropriation notes" />
        {detail.expropriationNotes}
      </StyledFormSection>
    </StyledSummarySection>
  );
};

export default DetailResearchForm;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;
