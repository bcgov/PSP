import * as React from 'react';

import EditButton from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_ResearchFile } from '@/models/api/ResearchFile';
import { formatApiProjectName, prettyFormatDate } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

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
  researchFileProjects?: string[];
}

export interface IResearchSummaryViewProps {
  researchFile?: Api_ResearchFile;
  setEditMode: (editable: boolean) => void;
}

const ResearchSummaryView: React.FunctionComponent<IResearchSummaryViewProps> = props => {
  const keycloak = useKeycloakWrapper();
  const detail: DetailResearchFile = {
    id: props.researchFile?.id,
    name: props.researchFile?.fileName,
    roadName: props.researchFile?.roadName,
    roadAlias: props.researchFile?.roadAlias,
    rfileNumber: props.researchFile?.fileNumber,
    statusTypeCodeDescription: props.researchFile?.fileStatusTypeCode?.description,
    requestDate: props.researchFile?.requestDate,
    requestDescription: props.researchFile?.requestDescription,
    requestSourceDescription: props.researchFile?.requestSourceDescription,
    researchResult: props.researchFile?.researchResult,
    researchCompletionDate: props.researchFile?.researchCompletionDate,
    isExpropriation: props.researchFile?.isExpropriation,
    expropriationNotes: props.researchFile?.expropriationNotes,
    requestSourceTypeDescription: props.researchFile?.requestSourceType?.description,
  };

  if (props.researchFile?.requestorPerson !== undefined) {
    detail.requestorName = formatApiPersonNames(props.researchFile.requestorPerson);
    var personOrganizations = props.researchFile.requestorPerson.personOrganizations;
    var organization =
      personOrganizations !== undefined && personOrganizations.length > 0
        ? personOrganizations[0].organization
        : undefined;
    detail.requestorOrganization = organization?.name;
  } else if (props.researchFile?.requestorOrganization !== undefined) {
    detail.requestorName = props.researchFile.requestorOrganization.name;
  }

  function isString(str: string | undefined): str is string {
    return str !== undefined;
  }

  detail.researchFilePurposes =
    props.researchFile?.researchFilePurposes !== undefined
      ? props.researchFile.researchFilePurposes
          .map(x => x.researchPurposeTypeCode?.description)
          .filter(isString)
      : [];

  detail.researchFileProjects =
    props.researchFile?.researchFileProjects !== undefined
      ? props.researchFile.researchFileProjects
          .filter(rp => rp?.project !== undefined)
          .map(rp => formatApiProjectName(rp.project))
      : [];

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.RESEARCH_EDIT) && props.researchFile !== undefined ? (
          <EditButton
            title="Edit research file"
            onClick={() => {
              props.setEditMode(true);
            }}
          />
        ) : null}
      </StyledEditWrapper>
      <Section header="Project">
        <SectionField label="Ministry project">
          {detail.researchFileProjects.map(formattedName => (
            <div>{formattedName}</div>
          ))}
        </SectionField>
      </Section>
      <Section header="Roads">
        <SectionField label="Road name">{detail.roadName}</SectionField>
        <SectionField label="Road alias">{detail.roadAlias}</SectionField>
      </Section>
      <Section header="Research Request">
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
      </Section>
      <Section header="Result">
        <SectionField label="Research completed on">
          {detail.researchCompletionDate !== undefined
            ? prettyFormatDate(detail.researchCompletionDate)
            : 'not complete'}
        </SectionField>
        <SectionField label="Result of request" />
        {detail.researchResult}
      </Section>
      <Section header="Expropriation">
        <SectionField label="Expropriation?">{detail.isExpropriation ? 'Yes' : 'No'}</SectionField>
        <SectionField label="Expropriation notes" />
        {detail.expropriationNotes}
      </Section>
    </StyledSummarySection>
  );
};

export default ResearchSummaryView;
