import EditButton from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { exists, formatApiProjectName, prettyFormatDate } from '@/utils';
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
  researchFile?: ApiGen_Concepts_ResearchFile;
  setEditMode: (editable: boolean) => void;
}

const ResearchSummaryView: React.FunctionComponent<IResearchSummaryViewProps> = props => {
  const keycloak = useKeycloakWrapper();
  const detail: DetailResearchFile = {
    id: props.researchFile?.id,
    name: props.researchFile?.fileName ?? undefined,
    roadName: props.researchFile?.roadName ?? undefined,
    roadAlias: props.researchFile?.roadAlias ?? undefined,
    rfileNumber: props.researchFile?.fileNumber ?? undefined,
    statusTypeCodeDescription: props.researchFile?.fileStatusTypeCode?.description ?? undefined,
    requestDate: props.researchFile?.requestDate ?? undefined,
    requestDescription: props.researchFile?.requestDescription ?? undefined,
    requestSourceDescription: props.researchFile?.requestSourceDescription ?? undefined,
    researchResult: props.researchFile?.researchResult ?? undefined,
    researchCompletionDate: props.researchFile?.researchCompletionDate ?? undefined,
    isExpropriation: props.researchFile?.isExpropriation ?? undefined,
    expropriationNotes: props.researchFile?.expropriationNotes ?? undefined,
    requestSourceTypeDescription: props.researchFile?.requestSourceType?.description ?? undefined,
  };
  // todo:the method 'exists' here should allow the compiler to validate the child property. this works correctly in typescropt 5.3 +
  if (exists(props.researchFile?.requestorPerson)) {
    detail.requestorName = formatApiPersonNames(props.researchFile!.requestorPerson);
    const personOrganizations = props.researchFile!.requestorPerson.personOrganizations;
    const organization =
      exists(personOrganizations) && personOrganizations.length > 0
        ? personOrganizations[0].organization
        : undefined;
    detail.requestorOrganization = organization?.name ?? undefined;
  } else if (exists(props.researchFile?.requestorOrganization)) {
    detail.requestorName = props.researchFile!.requestorOrganization.name ?? undefined;
  }
  // todo:the method 'exists' here should allow the compiler to validate the child property. this works correctly in typescropt 5.3 +
  detail.researchFilePurposes = exists(props.researchFile?.researchFilePurposes)
    ? props
        .researchFile!.researchFilePurposes.map(x => x.researchPurposeTypeCode!.description)
        .filter(exists)
    : [];
  // todo:the method 'exists' here should allow the compiler to validate the child property. this works correctly in typescropt 5.3 +
  detail.researchFileProjects = exists(props.researchFile?.researchFileProjects)
    ? props
        .researchFile!.researchFileProjects.filter(exists)
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
          {detail.researchFileProjects.map((formattedName, i) => (
            <div key={`project-key-${i}`}>{formattedName}</div>
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
