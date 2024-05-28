import { FaMailBulk, FaPlus, FaTrash } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { StyledRemoveLinkButton } from '@/components/common/buttons/RemoveButton';
import EditButton from '@/components/common/EditButton';
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
import StatusUpdateSolver from '../../fileDetails/detail/statusUpdateSolver';

export interface IAgreementViewProps {
  loading: boolean;
  agreements: ApiGen_Concepts_Agreement[];
  statusUpdateSolver: StatusUpdateSolver;
  onGenerate: (agreement: ApiGen_Concepts_Agreement) => void;
  onDelete: (agreementId: number) => void;
}

export const AgreementView: React.FunctionComponent<IAgreementViewProps> = ({
  loading,
  agreements,
  statusUpdateSolver,
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
            onAdd={() => {
              history.push(`${match.url}/add`);
            }}
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
                        <StyledAddButton
                          onClick={() => {
                            onGenerate(agreement);
                          }}
                        >
                          <FaMailBulk className="mr-2" />
                          Generate
                        </StyledAddButton>

                        {!statusUpdateSolver.canEditOrDeleteAgreement(
                          agreement.agreementStatusType?.id ?? null,
                        ) && (
                          <TooltipIcon
                            toolTipId={`${agreement?.agreementId}-agreement-cannot-edit-tooltip`}
                            toolTip={cannotEditMessage}
                          />
                        )}

                        {statusUpdateSolver.canEditOrDeleteAgreement(
                          agreement.agreementStatusType?.id ?? null,
                        ) &&
                          keycloak.hasClaim(Claims.ACQUISITION_EDIT) && (
                            <>
                              <EditButton
                                title="Edit Agreement"
                                dataTestId={`agreements[${index}].edit-btn`}
                                onClick={() =>
                                  history.push(`${match.url}/${agreement.agreementId}/update`)
                                }
                              />
                              <StyledRemoveLinkButton
                                title="Delete Agreement"
                                data-testid={`agreements[${index}].delete-btn`}
                                variant="light"
                                onClick={() => {
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
                              >
                                <FaTrash size="2rem" />
                              </StyledRemoveLinkButton>
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
              <SectionField labelWidth="6" label="Agreement status">
                {agreement.agreementStatusType?.description ?? ''}
              </SectionField>
              {agreement.agreementStatusType?.id ===
                ApiGen_CodeTypes_AgreementStatusTypes.CANCELLED && (
                <SectionField labelWidth="6" label="Cancellation reason">
                  {agreement.cancellationNote ?? ''}
                </SectionField>
              )}
              <SectionField labelWidth="6" label="Legal survey plan">
                {agreement.legalSurveyPlanNum}
              </SectionField>
              <SectionField labelWidth="6" label="Agreement type">
                {agreement.agreementType?.description}
              </SectionField>
              <SectionField labelWidth="6" label="Agreement date">
                {prettyFormatDate(agreement.agreementDate)}
              </SectionField>
              {agreement.agreementType?.id === ApiGen_CodeTypes_AgreementTypes.H0074 && (
                <SectionField labelWidth="6" label="Commencement date">
                  {prettyFormatDate(agreement.commencementDate)}
                </SectionField>
              )}
              <SectionField labelWidth="6" label="Completion date">
                {prettyFormatDate(agreement.completionDate)}
              </SectionField>
              <SectionField labelWidth="6" label="Termination date">
                {prettyFormatDate(agreement.terminationDate)}
              </SectionField>
              <SectionField labelWidth="6" label="Possession date">
                {prettyFormatDate(agreement.possessionDate)}
              </SectionField>

              <StyledAgreementSubheader>Financial</StyledAgreementSubheader>
              <SectionField labelWidth="6" label="Purchase price">
                {formatMoney(agreement.purchasePrice)}
              </SectionField>
              <SectionField
                labelWidth="6"
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
              <SectionField labelWidth="6" label="Deposit amount">
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
  align-items: center;
`;

const StyledAgreementBorder = styled.div`
  border: solid 0.2rem ${props => props.theme.css.discardedColor};
  margin-bottom: 1.5rem;
  border-radius: 0.5rem;
`;

export const StyledAgreementSubheader = styled.div`
  font-weight: bold;
  border-bottom: 0.2rem ${props => props.theme.css.discardedColor} solid;
  margin-top: 2rem;
  margin-bottom: 2rem;
`;
