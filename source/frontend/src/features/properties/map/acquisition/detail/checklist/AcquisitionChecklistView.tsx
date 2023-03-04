import { EditButton } from 'components/common/EditButton';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import { Claims } from 'constants/index';
import { Section } from 'features/mapSideBar/tabs/Section';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';
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
