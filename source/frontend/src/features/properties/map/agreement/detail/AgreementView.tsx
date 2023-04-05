import EditButton from 'components/common/EditButton';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import Claims from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledEditWrapper, StyledSummarySection } from 'features/mapSideBar/tabs/SectionStyles';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Agreement } from 'models/api/Agreement';
import * as React from 'react';
import styled from 'styled-components';
import { formatMoney, prettyFormatDate } from 'utils';

export interface IAgreementViewProps {
  loading: boolean;
  agreements: Api_Agreement[];
  onEdit: () => void;
}

export const AgreementView: React.FunctionComponent<IAgreementViewProps> = ({
  loading,
  agreements,
  onEdit,
}) => {
  const keycloak = useKeycloakWrapper();

  if (loading) {
    return <LoadingBackdrop show={loading} parentScreen={true} />;
  }

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.ACQUISITION_EDIT) ? (
          <EditButton title="Edit agreements file" onClick={onEdit} />
        ) : null}
      </StyledEditWrapper>

      {agreements.map((agreement, index) => (
        <Section header={`Agreement ${index + 1}`} isCollapsable initiallyExpanded>
          <StyledSectionSubheader>Agreement details</StyledSectionSubheader>
          <SectionField labelWidth="5" label="Agreement status">
            {agreement.agreementStatus === true ? 'Yes' : 'No'}
          </SectionField>
          <SectionField labelWidth="5" label="Legal survey plan">
            {agreement.legalSurveyPlanNum}
          </SectionField>
          <SectionField labelWidth="5" label="Agreement type">
            {agreement.agreementType?.description}
          </SectionField>
          <SectionField labelWidth="5" label="Agreement date">
            {prettyFormatDate(agreement.agreementDate)}
          </SectionField>
          <SectionField labelWidth="5" label="Commencement date">
            {prettyFormatDate(agreement.commencementDate)}
          </SectionField>
          <SectionField labelWidth="5" label="Completion date">
            {prettyFormatDate(agreement.completionDate)}
          </SectionField>
          <SectionField labelWidth="5" label="Termination date">
            {prettyFormatDate(agreement.terminationDate)}
          </SectionField>

          <StyledSectionSubheader>Financial</StyledSectionSubheader>
          <SectionField labelWidth="5" label="Purchase price">
            {formatMoney(agreement.purchasePrice)}
          </SectionField>
          <SectionField labelWidth="5" label="Deposit due no later than">
            {agreement.noLaterThanDays !== null
              ? agreement.noLaterThanDays && <strong>days</strong>
              : ''}
          </SectionField>
          <SectionField labelWidth="5" label="Deposit amount">
            {formatMoney(agreement.depositAmount)}
          </SectionField>
        </Section>
      ))}
    </StyledSummarySection>
  );
};

export default AgreementView;

const StyledSectionSubheader = styled.div`
  font-weight: bold;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: 0.2rem ${props => props.theme.css.primaryColor} solid;
  margin-bottom: 2rem;
`;
