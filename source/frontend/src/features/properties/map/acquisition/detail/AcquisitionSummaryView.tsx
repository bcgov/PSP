import EditButton from 'components/common/EditButton';
import Claims from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

import { DetailAcquisitionFile } from './models';

export interface IAcquisitionSummaryViewProps {
  acquisitionFile?: Api_AcquisitionFile;
  onEdit: () => void;
}

const AcquisitionSummaryView: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionSummaryViewProps>
> = ({ acquisitionFile, onEdit }) => {
  const keycloak = useKeycloakWrapper();
  const detail: DetailAcquisitionFile = DetailAcquisitionFile.fromApi(acquisitionFile);

  const projectName =
    acquisitionFile?.project !== undefined
      ? acquisitionFile?.project?.code + ' - ' + acquisitionFile?.project?.description
      : '';

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && acquisitionFile !== undefined ? (
          <EditButton title="Edit acquisition file" onClick={onEdit} />
        ) : null}
      </StyledEditWrapper>
      <Section header="Project">
        <SectionField label="Ministry project">{projectName}</SectionField>
        <SectionField label="Funding">{acquisitionFile?.fundingTypeCode?.description}</SectionField>

        {acquisitionFile?.fundingTypeCode?.id === 'OTHER' && (
          <SectionField label="Other funding">{acquisitionFile.fundingOther}</SectionField>
        )}
      </Section>
      <Section header="Schedule">
        <SectionField label="Assigned date">{prettyFormatDate(detail.assignedDate)}</SectionField>
        <SectionField
          label="Delivery date"
          tooltip="Date for delivery of the property to the project"
        >
          {prettyFormatDate(detail.deliveryDate)}
        </SectionField>
      </Section>
      <Section header="Acquisition details">
        <SectionField label="Acquisition file name">{detail.fileName}</SectionField>
        <SectionField label="Physical file status">
          {detail.acquisitionPhysFileStatusTypeDescription}
        </SectionField>
        <SectionField label="Acquisition type">{detail.acquisitionTypeDescription}</SectionField>
        <SectionField label="Ministry region">{detail.regionDescription}</SectionField>
      </Section>
      <Section header="Acquisition team">
        {detail.acquisitionTeam.map((person, index) => (
          <SectionField key={index} label={person.personProfileTypeCodeDescription || ''}>
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={`/contact/P${person.personId}`}
            >
              <span>{person.personName}</span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          </SectionField>
        ))}
      </Section>
    </StyledSummarySection>
  );
};

export default AcquisitionSummaryView;

const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};

  text-align: right;
`;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledLink = styled(Link)`
  display: flex;
  align-items: center;
`;
