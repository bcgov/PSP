import { EditButton } from 'components/common/EditButton';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import * as API from 'constants/API';
import { Claims } from 'constants/index';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';
import { FiCheck, FiMinus, FiX } from 'react-icons/fi';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

export interface IAcquisitionChecklistViewProps {
  acquisitionFile?: Api_AcquisitionFile;
  onEdit: () => void;
}

export const AcquisitionChecklistView: React.FC<IAcquisitionChecklistViewProps> = ({
  acquisitionFile,
  onEdit,
}) => {
  const keycloak = useKeycloakWrapper();
  const { getByType } = useLookupCodeHelpers();
  const sectionTypes = getByType(API.ACQUISITION_CHECKLIST_SECTION_TYPES);

  const checklist = acquisitionFile?.acquisitionFileChecklist || [];

  // TODO: get from model instead of this mock
  const mockChecklistAudit = {
    appLastUpdateTimestamp: '2022-03-18',
    appLastUpdateUserid: 'ALESANCH',
    appLastUpdateUserGuid: '4109e6b4-585c-4678-8a24-1a99b45e3a5d',
  };

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && acquisitionFile !== undefined ? (
          <EditButton title="Edit acquisition checklist" onClick={onEdit} />
        ) : null}
      </StyledEditWrapper>
      <StyledSectionCentered>
        <span>
          {`This checklist was last updated ${prettyFormatDate(
            mockChecklistAudit?.appLastUpdateTimestamp,
          )} by `}
        </span>
        <UserNameTooltip
          userName={mockChecklistAudit?.appLastUpdateUserid}
          userGuid={mockChecklistAudit?.appLastUpdateUserGuid}
        />
      </StyledSectionCentered>

      {sectionTypes.map((section, i) => (
        <Section key={section.id ?? `acq-checklist-section-${i}`} header={section.name}>
          {checklist
            .filter(checklistItem => checklistItem.itemType?.sectionCode === section.id)
            .map((checklistItem, j) => (
              <SectionField
                key={checklistItem.itemType?.code ?? `acq-checklist-item-${j}`}
                label={checklistItem.itemType?.description ?? ''}
                tooltip={checklistItem.itemType?.hint}
                labelWidth="7"
              >
                <StyledChecklistItem>
                  <StyledChecklistItemAudit>&nbsp;</StyledChecklistItemAudit>
                  <StyledChecklistItemStatus
                    color={mapStatusToColor(checklistItem.statusTypeCode?.id)}
                  >
                    {checklistItem.statusTypeCode?.description}
                  </StyledChecklistItemStatus>
                  <StyledChecklistItemIcon>
                    <StatusIcon status={checklistItem.statusTypeCode?.id} />
                  </StyledChecklistItemIcon>
                </StyledChecklistItem>
              </SectionField>
            ))}
        </Section>
      ))}
    </StyledSummarySection>
  );
};

function mapStatusToColor(status?: string): string | undefined {
  switch (status) {
    case 'COMPLT':
      return '#2E8540';

    case 'NOTAPP':
      return '#aaaaaa';

    default:
      return undefined;
  }
}

const StatusIcon: React.FC<{ status?: string }> = ({ status }) => {
  const color = mapStatusToColor(status);
  switch (status) {
    case 'INCOMP':
      return <FiX size="2rem" color={color} />;

    case 'COMPLT':
      return <FiCheck size="2rem" color={color} />;

    case 'NOTAPP':
      return <FiMinus size="2rem" color={color} />;

    default:
      return null;
  }
};

const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};
  text-align: right;
`;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledSectionCentered = styled(Section)`
  text-align: center;
`;

const StyledChecklistItem = styled.div`
  display: flex;
  width: 100%;
  padding-right: 0.5rem;
  gap: 0.4rem;
`;

const StyledChecklistItemAudit = styled.span`
  min-width: 35%;
  text-align: right;
`;

const StyledChecklistItemStatus = styled.span<{ color?: string }>`
  color: ${props => props.color ?? props.theme.css.textColor};
  min-width: 55%;
`;

const StyledChecklistItemIcon = styled.span`
  min-width: 10%;
  text-align: right;
`;
