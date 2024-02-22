import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaMailBulk } from 'react-icons/fa';
import styled from 'styled-components';

import EditButton from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { StyledAddButton } from '@/components/common/styles';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_AgreementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementStatusTypes';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { formatMoney, prettyFormatDate } from '@/utils';

import { StyledSectionSubheader } from '../styles';

export interface IAgreementViewProps {
  loading: boolean;
  agreements: ApiGen_Concepts_Agreement[];
  onEdit: () => void;
  onGenerate: (agreement: ApiGen_Concepts_Agreement) => void;
}

export const AgreementView: React.FunctionComponent<IAgreementViewProps> = ({
  loading,
  agreements,
  onEdit,
  onGenerate,
}) => {
  const keycloak = useKeycloakWrapper();
  const H0074Type = 'H0074';

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.ACQUISITION_EDIT) ? (
          <EditButton title="Edit agreements file" onClick={onEdit} />
        ) : null}
      </StyledEditWrapper>
      {agreements.length === 0 && (
        <StyledNoData>
          <p>There are no agreements associated with this file.</p>
          <p> To begin an agreement, click the edit button.</p>
        </StyledNoData>
      )}
      {agreements.map((agreement, index) => (
        <Section
          key={`agreement-section-${index}`}
          header={
            <Row>
              <Col md={6}>{`Agreement ${index + 1}`}</Col>
              <Col md={6} style={{ paddingRight: '0px' }}>
                {agreement.agreementType !== null && (
                  <StyledButtonContainer>
                    <StyledAddButton
                      onClick={() => {
                        onGenerate(agreement);
                      }}
                    >
                      <FaMailBulk className="mr-2" />
                      Generate document
                    </StyledAddButton>
                  </StyledButtonContainer>
                )}
              </Col>
            </Row>
          }
          isCollapsable
          initiallyExpanded
        >
          <StyledSectionSubheader>Agreement details</StyledSectionSubheader>
          <SectionField labelWidth="5" label="Agreement status">
            {agreement.agreementStatusType?.description ?? ''}
          </SectionField>
          {agreement.agreementStatusType?.id ===
            ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED && (
            <SectionField labelWidth="5" label="Cancellation reason">
              {agreement.cancellationNote ?? ''}
            </SectionField>
          )}
          <SectionField labelWidth="5" label="Legal survey plan">
            {agreement.legalSurveyPlanNum}
          </SectionField>
          <SectionField labelWidth="5" label="Agreement type">
            {agreement.agreementType?.description}
          </SectionField>
          <SectionField labelWidth="5" label="Agreement date">
            {prettyFormatDate(agreement.agreementDate)}
          </SectionField>
          {agreement.agreementType?.id === H0074Type && (
            <SectionField labelWidth="5" label="Commencement date">
              {prettyFormatDate(agreement.commencementDate)}
            </SectionField>
          )}
          <SectionField labelWidth="5" label="Completion date">
            {prettyFormatDate(agreement.completionDate)}
          </SectionField>
          <SectionField labelWidth="5" label="Termination date">
            {prettyFormatDate(agreement.terminationDate)}
          </SectionField>
          <SectionField labelWidth="5" label="Possession date">
            {prettyFormatDate(agreement.possessionDate)}
          </SectionField>

          <StyledSectionSubheader>Financial</StyledSectionSubheader>
          <SectionField labelWidth="5" label="Purchase price">
            {formatMoney(agreement.purchasePrice)}
          </SectionField>
          <SectionField
            labelWidth="5"
            label="Deposit due no later than"
            tooltip="Generally, if applicable, this is number of days from the execution of the agreement."
          >
            {agreement.noLaterThanDays ? (
              <span>
                {agreement.noLaterThanDays} <strong>days</strong>
              </span>
            ) : (
              ''
            )}
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

export const StyledNoData = styled.div`
  font-style: italic;
  margin: 1.5rem;
  padding: 1rem;
  background-color: white;
  text-align: left;
  border-radius: 0.5rem;
`;

export const StyledButtonContainer = styled.div`
  float: right;
`;
