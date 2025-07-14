import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaCheckCircle, FaExclamationCircle, FaPlus, FaTimesCircle } from 'react-icons/fa';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { RemoveIconButton } from '@/components/common/buttons/RemoveButton';
import ContactFieldContainer from '@/components/common/ContactFieldContainer';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import TooltipIcon from '@/components/common/TooltipIcon';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import * as API from '@/constants/API';
import Claims from '@/constants/claims';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_ConsultationOutcomeTypes } from '@/models/api/generated/ApiGen_CodeTypes_ConsultationOutcomeTypes';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { prettyFormatDate } from '@/utils';
import { booleanToYesNoString } from '@/utils/formUtils';

export interface IConsultationListViewProps {
  loading: boolean;
  consultations: ApiGen_Concepts_ConsultationLease[];
  isFileFinalStatus?: boolean;
  onAdd: () => void;
  onEdit: (consultationId: number) => void;
  onDelete: (consultationId: number) => void;
}

interface GroupedConsultations {
  consultationTypeCode: string;
  consultationTypeDescription: string;
  consultations: ApiGen_Concepts_ConsultationLease[];
  hasItems: boolean;
}

export const ConsultationListView: React.FunctionComponent<IConsultationListViewProps> = ({
  loading,
  consultations,
  isFileFinalStatus,
  onAdd,
  onEdit,
  onDelete,
}) => {
  const keycloak = useKeycloakWrapper();
  const { setModalContent, setDisplayModal } = useModalContext();

  const { getOptionsByType } = useLookupCodeHelpers();
  const consultationTypeCodes = getOptionsByType(API.CONSULTATION_TYPES);

  const groupedConsultations = useMemo(() => {
    const grouped: GroupedConsultations[] = [];
    consultationTypeCodes.forEach(ct =>
      grouped.push({
        consultationTypeCode: ct.value.toString(),
        consultationTypeDescription: ct.label,
        consultations: consultations.filter(c => c.consultationTypeCode.id === ct.value),
        hasItems: consultations.filter(c => c.consultationTypeCode.id === ct.value).length === 0,
      }),
    );
    return grouped;
  }, [consultationTypeCodes, consultations]);

  if (loading) {
    return <LoadingBackdrop show={loading} parentScreen={true} />;
  }

  return (
    <StyledSummarySection>
      <Section
        header={
          <SectionListHeader
            claims={[Claims.LEASE_EDIT]}
            title="Approval / Consultations"
            addButtonText="Add Approval / Consultation"
            addButtonIcon={<FaPlus size="2rem" />}
            onButtonAction={onAdd}
            cannotAddComponent={
              <TooltipIcon toolTipId={`agreement-cannot-add-tooltip`} toolTip={cannotEditMessage} />
            }
            isAddEnabled={!isFileFinalStatus}
          />
        }
      >
        {groupedConsultations.map((group, index) => (
          <Section
            key={index}
            header={
              <StyledConsultationHeader>
                <Col xs="auto">
                  <span className="px-2">{group.consultationTypeDescription}</span>
                  {getOutcomeIcon(group.consultations)}
                </Col>
              </StyledConsultationHeader>
            }
            noPadding
            isCollapsable={true}
            initiallyExpanded={false}
          >
            {group.consultations.map((consultation, index) => (
              <StyledBorder key={`consultation-section-${index}`}>
                <Section
                  header={
                    <div>
                      <Row>
                        <Col>{consultation.consultationOutcomeTypeCode?.description}</Col>
                        {keycloak.hasClaim(Claims.LEASE_EDIT) && !isFileFinalStatus && (
                          <>
                            <Col xs="auto" className="px-1">
                              <RemoveIconButton
                                title="Delete Consultation"
                                data-testId={`consultations[${index}].delete-btn`}
                                onRemove={() => {
                                  setModalContent({
                                    ...getDeleteModalProps(),
                                    variant: 'error',
                                    title: 'Delete Consultation',
                                    message: `You have selected to delete this Consultation.
                                        Do you want to proceed?`,
                                    okButtonText: 'Yes',
                                    cancelButtonText: 'No',
                                    handleOk: async () => {
                                      consultation.id && onDelete(consultation.id);
                                      setDisplayModal(false);
                                    },
                                    handleCancel: () => {
                                      setDisplayModal(false);
                                    },
                                  });
                                  setDisplayModal(true);
                                }}
                              />
                            </Col>
                            <Col xs="auto" className="px-2">
                              <StyledButtonContainer>
                                <EditButton
                                  title="Edit Consultation"
                                  data-testId={`consultations[${index}].edit-btn`}
                                  onClick={() => onEdit(consultation.id)}
                                />
                              </StyledButtonContainer>
                            </Col>
                          </>
                        )}
                        {keycloak.hasClaim(Claims.LEASE_EDIT) && isFileFinalStatus && (
                          <TooltipIcon
                            toolTipId={`consultation-edit-actions-cannot-edit-tooltip`}
                            toolTip={cannotEditMessage}
                          />
                        )}
                      </Row>
                    </div>
                  }
                >
                  {consultation?.consultationTypeCode?.id === 'OTHER' && (
                    <SectionField
                      labelWidth={{ xs: 4 }}
                      contentWidth={{ xs: 6 }}
                      label="Description"
                      tooltip={
                        <TooltipIcon
                          toolTipId="lease-consultation-otherdescription-tooltip"
                          toolTip="Short description for the approval / consultation"
                        />
                      }
                    >
                      {consultation?.otherDescription}
                    </SectionField>
                  )}
                  <SectionField
                    labelWidth={{ xs: 4 }}
                    label="Requested on"
                    tooltip={
                      <TooltipIcon
                        toolTipId={`lease-consultation-${consultation.id}-requestedon-tooltip`}
                        toolTip="When the approval / consultation request was sent"
                      />
                    }
                  >
                    {prettyFormatDate(consultation.requestedOn)}
                  </SectionField>
                  <ContactFieldContainer
                    labelWidth={{ xs: 4 }}
                    label="Contact"
                    personId={consultation.personId}
                    organizationId={consultation.organizationId}
                    primaryContact={consultation.primaryContactId}
                    tooltip={
                      <TooltipIcon
                        toolTipId={`lease-consultation-${consultation.id}-contact-tooltip`}
                        toolTip="The point of contact, or one providing the approval / consultation "
                      />
                    }
                  />
                  <SectionField labelWidth={{ xs: 4 }} label="Response received">
                    {booleanToYesNoString(consultation.isResponseReceived)}
                  </SectionField>
                  <SectionField labelWidth={{ xs: 4 }} label="Response received on">
                    {prettyFormatDate(consultation.responseReceivedDate)}
                  </SectionField>
                  <SectionField
                    labelWidth={{ xs: 4 }}
                    label="Comments"
                    tooltip={
                      <TooltipIcon
                        toolTipId={`lease-consultation-${consultation.id}-comments-tooltip`}
                        toolTip="Remarks / summary on the process or its results"
                      />
                    }
                  >
                    {consultation.comment}
                  </SectionField>
                </Section>
              </StyledBorder>
            ))}
            {group.consultations.length === 0 && (
              <p className="pl-2">There are no approvals / consultations.</p>
            )}
          </Section>
        ))}
      </Section>
    </StyledSummarySection>
  );
};

export default ConsultationListView;

export const StyledButtonContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: center;
  margin-bottom: 0.5rem;
  align-items: center;
`;

const StyledBorder = styled.div`
  border: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  margin-bottom: 1.5rem;
  border-radius: 0.5rem;
`;

const StyledConsultationHeader = styled(Row)`
  svg.info {
    color: ${props => props.theme.bcTokens.surfaceColorBackgroundDarkBlue};
  }
  svg.error {
    color: ${props => props.theme.bcTokens.typographyColorDanger};
  }
  svg.ok {
    color: ${props => props.theme.css.completedColor};
  }
  svg {
    vertical-align: baseline;
  }
`;

const getOutcomeIcon = (consultations: ApiGen_Concepts_ConsultationLease[]) => {
  if (consultations.length === 0) {
    return null;
  }

  if (
    consultations.find(c =>
      [
        ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRDENIED.toString(),
        ApiGen_CodeTypes_ConsultationOutcomeTypes.CONSDISCONT.toString(),
      ].includes(c?.consultationOutcomeTypeCode?.id),
    )
  ) {
    return (
      <TooltipWrapper
        tooltipId="error-outcome-tooltip"
        tooltip="At least one approval declined or consultation discontinued"
      >
        <FaTimesCircle size={22} className="error" data-testid="error-icon" />
      </TooltipWrapper>
    );
  } else if (
    consultations.every(c =>
      [
        ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRGRANTED.toString(),
        ApiGen_CodeTypes_ConsultationOutcomeTypes.CONSCOMPLTD.toString(),
      ].includes(c?.consultationOutcomeTypeCode?.id),
    )
  ) {
    return (
      <TooltipWrapper
        tooltipId="ok-outcome-tooltip"
        tooltip="All approvals granted and/or consultations completed"
      >
        <FaCheckCircle size={22} className="ok" data-testid="ok-icon" />
      </TooltipWrapper>
    );
  } else {
    return (
      <TooltipWrapper
        tooltipId="info-outcome-tooltip"
        tooltip="One or more approval(s) / consultation(s) is in-progress"
      >
        <FaExclamationCircle size={22} className="info" data-testid="info-icon" />
      </TooltipWrapper>
    );
  }
};
