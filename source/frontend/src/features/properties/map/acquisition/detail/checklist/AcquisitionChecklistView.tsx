import { EditButton } from 'components/common/EditButton';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import { Claims } from 'constants/index';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';
import { FiCheck, FiX } from 'react-icons/fi';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

export interface IAcquisitionChecklistViewProps {
  acquisitionFile?: Api_AcquisitionFile;
  sectionTypes?: ILookupCode[];
  itemTypes?: ILookupCode[];
  itemStatusTypes?: ILookupCode[];
  onEdit: () => void;
}

export const AcquisitionChecklistView: React.FC<IAcquisitionChecklistViewProps> = ({
  acquisitionFile,
  onEdit,
  sectionTypes = [],
  itemTypes = [],
  itemStatusTypes = [],
}) => {
  const keycloak = useKeycloakWrapper();

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
          {itemTypes
            .filter(checklistItem => checklistItem.parentId === section.id)
            .map((checklistItem, j) => (
              <SectionField
                key={checklistItem.code ?? `acq-checklist-item-${j}`}
                label={checklistItem.name}
                tooltip={checklistItem.hint}
                labelWidth="7"
              >
                <StyledChecklistItem>
                  <StyledChecklistItemAudit>&nbsp;</StyledChecklistItemAudit>
                  <StyledChecklistItemStatus>Incomplete</StyledChecklistItemStatus>
                  <StyledChecklistItemIcon>
                    {(Math.floor(Math.random() * 10) + 1) % 2 === 0 ? (
                      <FiCheck size="2rem" color="#2E8540" />
                    ) : (
                      <FiX size="2rem" />
                    )}
                  </StyledChecklistItemIcon>
                </StyledChecklistItem>
              </SectionField>
            ))}
        </Section>
      ))}
    </StyledSummarySection>
  );
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
  padding-right: 1.5rem;
  gap: 1rem;
  text-align: right;
`;

const StyledChecklistItemAudit = styled.span`
  min-width: 45%;
`;

const StyledChecklistItemStatus = styled.span`
  min-width: 45%;
`;

const StyledChecklistItemIcon = styled.span`
  min-width: 10%;
`;
