import { FaEdit, FaFileContract, FaPlus, FaTrash } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { RemoveIconButton } from '@/components/common/buttons/RemoveButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { StyledAddButton } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_AgreementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementStatusTypes';
import { ApiGen_CodeTypes_AgreementTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementTypes';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { exists, formatMoney, prettyFormatDate } from '@/utils';

import { cannotEditMessage } from '../../../common/constants';

export interface IAgreementViewProps {
  loading: boolean;
  agreements: ApiGen_Concepts_Agreement[];
  isFileFinalStatus?: boolean;
  onGenerate: (agreement: ApiGen_Concepts_Agreement) => void;
  onDelete: (agreementId: number) => void;
}

export const AgreementView: React.FunctionComponent<IAgreementViewProps> = ({
  loading,
  agreements,
  isFileFinalStatus,
  onGenerate,
  onDelete,
}) => {
  const keycloak = useKeycloakWrapper();
  const history = useHistory();
  const match = useRouteMatch();
  const { setModalContent, setDisplayModal } = useModalContext();

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />

      <Section
        header={
          <SectionListHeader
            claims={[Claims.ACQUISITION_EDIT]}
            title="Agreements"
            addButtonText="Add Agreement"
            addButtonIcon={<FaPlus size={'2rem'} />}
            onButtonAction={() => {
              history.push(`${match.url}/add`);
            }}
            isAddEnabled={!isFileFinalStatus}
            cannotAddComponent={
              <TooltipIcon toolTipId={`agreement-cannot-add-tooltip`} toolTip={cannotEditMessage} />
            }
          />
        }
      >
        {agreements.map((agreement, index) => (
          <StyledAgreementBorder key={`agreement-section-${index}`}>
            <Section
              header={
                <StyledHeaderContainer>
                  <div>{`Agreement ${++index}`}</div>
                  <div>
                    {exists(agreement.agreementType) && (
                      <StyledButtonContainer>
                        {isFileFinalStatus ? (
                          <></>
                        ) : (
                          <>
                            <StyledAddButton
                              title="Download File"
                              onClick={() => {
                                onGenerate(agreement);
                              }}
                            >
                              <FaFileContract size={28} className="mr-2" />
                              {`Generate ${getAgreementFormName(agreement.agreementType.id)}`}
                            </StyledAddButton>

                            {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && (
                              <>
                                <EditButton
                                  title="Edit Agreement"
                                  data-testId={`agreements[${index}].edit-btn`}
                                  onClick={() =>
                                    history.push(`${match.url}/${agreement.agreementId}/update`)
                                  }
                                  icon={<FaEdit size={'2rem'} />}
                                />
                                <RemoveIconButton
                                  title="Delete Agreement"
                                  data-testId={`agreements[${index}].delete-btn`}
                                  icon={<FaTrash size={'1.75rem'} />}
                                  onRemove={() => {
                                    setModalContent({
                                      ...getDeleteModalProps(),
                                      variant: 'error',
                                      title: 'Delete Agreement',
                                      message: `You have selected to delete this Agreement.
                                        Do you want to proceed?`,
                                      okButtonText: 'Yes',
                                      cancelButtonText: 'No',
                                      handleOk: async () => {
                                        agreement.agreementId && onDelete(agreement.agreementId);
                                        setDisplayModal(false);
                                      },
                                      handleCancel: () => {
                                        setDisplayModal(false);
                                      },
                                    });
                                    setDisplayModal(true);
                                  }}
                                />
                              </>
                            )}
                          </>
                        )}
                      </StyledButtonContainer>
                    )}
                  </div>
                </StyledHeaderContainer>
              }
              isCollapsable
              initiallyExpanded
            >
              <SectionField labelWidth={{ xs: 6 }} label="Agreement status">
                {agreement.agreementStatusType?.description ?? ''}
              </SectionField>
              {agreement.agreementStatusType?.id ===
                ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED && (
                <SectionField labelWidth={{ xs: 6 }} label="Cancellation reason">
                  {agreement.cancellationNote ?? ''}
                </SectionField>
              )}
              <SectionField labelWidth={{ xs: 6 }} label="Legal survey plan">
                {agreement.legalSurveyPlanNum}
              </SectionField>
              <SectionField labelWidth={{ xs: 6 }} label="Agreement type">
                {agreement.agreementType?.description}
              </SectionField>
              <SectionField labelWidth={{ xs: 6 }} label="Agreement date">
                {prettyFormatDate(agreement.agreementDate)}
              </SectionField>
              {agreement.agreementType?.id === ApiGen_CodeTypes_AgreementTypes.H0074 && (
                <SectionField labelWidth={{ xs: 6 }} label="Commencement date">
                  {prettyFormatDate(agreement.commencementDate)}
                </SectionField>
              )}
              <SectionField labelWidth={{ xs: 6 }} label="Completion date">
                {prettyFormatDate(agreement.completionDate)}
              </SectionField>
              <SectionField labelWidth={{ xs: 6 }} label="Termination date">
                {prettyFormatDate(agreement.terminationDate)}
              </SectionField>
              <SectionField labelWidth={{ xs: 6 }} label="Possession date">
                {prettyFormatDate(agreement.possessionDate)}
              </SectionField>

              <StyledAgreementSubheader>Financial</StyledAgreementSubheader>
              <SectionField labelWidth={{ xs: 6 }} label="Purchase price">
                {formatMoney(agreement.purchasePrice)}
              </SectionField>
              <SectionField
                labelWidth={{ xs: 6 }}
                label="Deposit due no later than"
                tooltip="Generally, if applicable, this is number of days from the execution of the agreement"
              >
                {agreement.noLaterThanDays ? (
                  <span>
                    {agreement.noLaterThanDays} <strong>days</strong>
                  </span>
                ) : (
                  ''
                )}
              </SectionField>
              <SectionField labelWidth={{ xs: 6 }} label="Deposit amount">
                {formatMoney(agreement.depositAmount)}
              </SectionField>
            </Section>
          </StyledAgreementBorder>
        ))}
        {agreements.length === 0 && (
          <p>There are no agreements indicated in this acquisition file.</p>
        )}
      </Section>
    </StyledSummarySection>
  );
};

export default AgreementView;

function getAgreementFormName(agreementType: string | null): string {
  switch (agreementType) {
    case ApiGen_CodeTypes_AgreementTypes.H179A:
      return 'H-179A';
    case ApiGen_CodeTypes_AgreementTypes.H179P:
      return 'H-179P';
    case ApiGen_CodeTypes_AgreementTypes.H179T:
      return 'H-179T';
    case ApiGen_CodeTypes_AgreementTypes.H0074:
      return 'H-0074';
    case ApiGen_CodeTypes_AgreementTypes.H179FSPART:
      return 'H-179PFS';
    case ApiGen_CodeTypes_AgreementTypes.H179PTO:
      return 'H-179PTO';
    default:
      return '';
  }
}

export const StyledButtonContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: center;
  margin-bottom: 0.5rem;
  align-items: center;
`;

const StyledHeaderContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: end;
`;

const StyledAgreementBorder = styled.div`
  border: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  margin-bottom: 1.5rem;
  border-radius: 0.5rem;
`;

export const StyledAgreementSubheader = styled.div`
  font-weight: bold;
  border-bottom: 0.2rem ${props => props.theme.css.headerBorderColor} solid;
  margin-top: 2rem;
  margin-bottom: 2rem;
`;
