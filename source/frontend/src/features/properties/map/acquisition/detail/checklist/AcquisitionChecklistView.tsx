import { EditButton } from 'components/common/EditButton';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import * as API from 'constants/API';
import { Claims } from 'constants/index';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import {
  Api_AcquisitionFile,
  isDefaultState,
  lastModifiedBy,
  sortByDisplayOrder,
} from 'models/api/AcquisitionFile';
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
  const lastUpdated = lastModifiedBy(checklist);

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && acquisitionFile !== undefined ? (
          <EditButton title="Edit acquisition checklist" onClick={onEdit} />
        ) : null}
      </StyledEditWrapper>
      {lastUpdated && (
        <StyledSectionCentered>
          <em>
            {`This checklist was last updated ${prettyFormatDate(
              lastUpdated.appLastUpdateTimestamp,
            )} by `}
            <UserNameTooltip
              userName={lastUpdated.appLastUpdateUserid}
              userGuid={lastUpdated.appLastUpdateUserGuid}
            />
          </em>
        </StyledSectionCentered>
      )}

      {sectionTypes.map((section, i) => (
        <Section
          key={section.id ?? `acq-checklist-section-${i}`}
          header={section.name}
          isCollapsable
          initiallyExpanded
        >
          {checklist
            .filter(checklistItem => checklistItem.itemType?.sectionCode === section.id)
            .sort(sortByDisplayOrder)
            .map((checklistItem, j) => (
              <SectionField
                key={checklistItem.itemType?.code ?? `acq-checklist-item-${j}`}
                label={checklistItem.itemType?.description ?? ''}
                tooltip={checklistItem.itemType?.hint}
                labelWidth="6"
                noGutters
              >
                <StyledChecklistItem>
                  <StyledChecklistItemAudit>
                    {!isDefaultState(checklistItem) && (
                      <>
                        <UserNameTooltip
                          userName={checklistItem.appLastUpdateUserid}
                          userGuid={checklistItem.appLastUpdateUserGuid}
                        />
                        <em> {prettyFormatDate(checklistItem.appLastUpdateTimestamp)}</em>
                      </>
                    )}
                  </StyledChecklistItemAudit>
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
  font-size: 1.4rem;
  text-align: center;
`;

const StyledChecklistItem = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  line-height: 2.4rem;
  width: 100%;
  gap: 0.2rem;
`;

const StyledChecklistItemAudit = styled.span`
  min-width: 13rem;
  font-size: 1.1rem;
  font-style: italic;
  text-align: right;
  color: ${props => props.theme.css.discardedColor};

  .tooltip-icon {
    color: ${props => props.theme.css.discardedColor};
  }
`;

const StyledChecklistItemStatus = styled.span<{ color?: string }>`
  color: ${props => props.color ?? props.theme.css.textColor};
  min-width: 11rem;
`;

const StyledChecklistItemIcon = styled.span`
  min-width: 2.4rem;
  text-align: right;
`;
