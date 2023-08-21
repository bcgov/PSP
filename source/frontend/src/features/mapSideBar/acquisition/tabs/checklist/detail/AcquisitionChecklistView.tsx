import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FiCheck, FiMinus, FiX } from 'react-icons/fi';
import styled from 'styled-components';

import { EditButton } from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import * as API from '@/constants/API';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import {
  Api_AcquisitionFile,
  isDefaultState,
  lastModifiedBy,
  sortByDisplayOrder,
} from '@/models/api/AcquisitionFile';
import { prettyFormatUTCDate } from '@/utils';

import { StyledChecklistItemStatus, StyledSectionCentered } from './styles';

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
            {`This checklist was last updated ${prettyFormatUTCDate(
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
                <Row className="no-gutters">
                  <Col xs="6">
                    <StyledChecklistItemAudit>
                      {!isDefaultState(checklistItem) && (
                        <>
                          <UserNameTooltip
                            userName={checklistItem.appLastUpdateUserid}
                            userGuid={checklistItem.appLastUpdateUserGuid}
                          />
                          <em> {prettyFormatUTCDate(checklistItem.appLastUpdateTimestamp)}</em>
                        </>
                      )}
                    </StyledChecklistItemAudit>
                  </Col>
                  <Col xs="5">
                    <StyledChecklistItemStatus
                      color={mapStatusToColor(checklistItem.statusTypeCode?.id)}
                    >
                      {checklistItem.statusTypeCode?.description}
                    </StyledChecklistItemStatus>
                  </Col>
                  <Col xs="1">
                    <StyledChecklistItemIcon>
                      <StatusIcon status={checklistItem.statusTypeCode?.id} />
                    </StyledChecklistItemIcon>
                  </Col>
                </Row>
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

const StyledChecklistItemIcon = styled.span`
  min-width: 2.4rem;
  text-align: right;
`;
