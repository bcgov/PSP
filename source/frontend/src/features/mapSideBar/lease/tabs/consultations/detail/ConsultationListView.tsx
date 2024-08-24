import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus, FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledRemoveLinkButton } from '@/components/common/buttons/RemoveButton';
import ContactFieldContainer from '@/components/common/ContactFieldContainer';
import EditButton from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import * as API from '@/constants/API';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { prettyFormatDate } from '@/utils';
import { booleanToYesNoUnknownString } from '@/utils/formUtils';

export interface IConsultationListViewProps {
  loading: boolean;
  consultations: ApiGen_Concepts_ConsultationLease[];
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
            title="Consultations"
            addButtonText="Add Consultation"
            addButtonIcon={<FaPlus size="2rem" />}
            onAdd={onAdd}
          />
        }
      >
        {groupedConsultations.map((group, index) => (
          <Section
            key={index}
            header={
              <Row>
                <Col xs="auto">
                  <span className="pl-2">{group.consultationTypeDescription}</span>
                </Col>
                <Col className="px-0">
                  {group.consultations.length > 0 && (
                    <StyledIconWrapper>{group.consultations.length}</StyledIconWrapper>
                  )}
                </Col>
              </Row>
            }
            noPadding
            isCollapsable={!group.hasItems}
            initiallyExpanded={group.hasItems}
          >
            {group.consultations.map((consultation, index) => (
              <StyledBorder key={`consultation-section-${index}`}>
                <Section
                  header={
                    <div>
                      <Row>
                        <Col>
                          {group.consultationTypeCode === 'OTHER'
                            ? `${consultation.otherDescription} Consultation`
                            : `${group.consultationTypeDescription} Consultation`}
                        </Col>

                        {keycloak.hasClaim(Claims.LEASE_EDIT) && (
                          <>
                            <Col xs="auto" className="px-1">
                              <StyledRemoveLinkButton
                                title="Delete Consultation"
                                data-testid={`consultations[${index}].delete-btn`}
                                variant="light"
                                onClick={() => {
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
                              >
                                <FaTrash size="2rem" />
                              </StyledRemoveLinkButton>
                            </Col>
                            <Col xs="auto" className="px-2">
                              <EditButton
                                title="Edit Consultation"
                                dataTestId={`consultations[${index}].edit-btn`}
                                onClick={() => onEdit(consultation.id)}
                              />
                            </Col>
                          </>
                        )}
                      </Row>
                    </div>
                  }
                >
                  <SectionField labelWidth="4" label="Requested on">
                    {prettyFormatDate(consultation.requestedOn)}
                  </SectionField>
                  <ContactFieldContainer
                    labelWidth="4"
                    label="Contact"
                    personId={consultation.personId}
                    organizationId={consultation.organizationId}
                    primaryContact={consultation.primaryContactId}
                  />
                  <SectionField labelWidth="4" label="Response received">
                    {booleanToYesNoUnknownString(consultation.isResponseReceived)}
                  </SectionField>
                  <SectionField labelWidth="4" label="Response received on">
                    {prettyFormatDate(consultation.responseReceivedDate)}
                  </SectionField>
                  <SectionField labelWidth="4" label="Comments">
                    {consultation.comment}
                  </SectionField>
                  <SectionField labelWidth="4" label="Type">
                    {consultation.consultationTypeCode.id}
                  </SectionField>
                </Section>
              </StyledBorder>
            ))}
            {group.consultations.length === 0 && (
              <p className="pl-2">There are no consultations.</p>
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

const StyledIconWrapper = styled.div`
  background-color: ${props => props.theme.css.activeActionColor};
  color: white;
  font-size: 1.4rem;
  border-radius: 50%;
  opacity: 0.8;
  width: 2.2rem;
  height: 2.2rem;
  padding: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
`;
