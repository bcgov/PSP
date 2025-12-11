import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt, FaFileContract } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { RemoveIconButton } from '@/components/common/buttons';
import EditButton from '@/components/common/buttons/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { H3, StyledLink, StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import { DetailAcquisitionFileOwner } from '@/features/mapSideBar/acquisition/models/DetailAcquisitionFileOwner';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { formatInterestHolderName } from '@/utils/formUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

import ExpropriationPaymentItemsTable from './ExpropriationPaymentItemsTable';

export interface IExpropriationForm8DetailsProps {
  form8Index: number;
  form8: ApiGen_Concepts_ExpropriationPayment;
  acquisitionFileNumber: string;
  isFileFinalStatus: boolean;
  onDelete: (form8Id: number) => void;
  onGenerate: (form8Id: number, acquisitionFileNumber: string) => void;
}

export const ExpropriationForm8Details: React.FunctionComponent<
  IExpropriationForm8DetailsProps
> = ({ form8, form8Index, acquisitionFileNumber, isFileFinalStatus, onDelete, onGenerate }) => {
  const history = useHistory();
  const match = useRouteMatch();
  const keycloak = useKeycloakWrapper();
  const { setModalContent, setDisplayModal } = useModalContext();

  const expropriationPayeeOwner =
    form8.acquisitionOwnerId && form8.acquisitionOwner
      ? DetailAcquisitionFileOwner.fromApi(form8.acquisitionOwner)
      : null;

  const interestHolderContactLink = getInterestHolderLink(form8.interestHolder);
  const interestHolderDisplayName = formatInterestHolderName(form8.interestHolder);
  const paymentItemsTotal =
    form8.paymentItems?.map(x => x.totalAmount ?? 0).reduce((prev, next) => prev + next, 0) ?? 0;

  return (
    <StyledForm8Border>
      <Section isCollapsable initiallyExpanded>
        <StyledSubHeader>
          <StyledSectionAddButton
            title="Download File"
            data-testid={`form8[${form8Index}].generate-form8`}
            onClick={() => onGenerate(form8.id as number, acquisitionFileNumber)}
          >
            <FaFileContract size={28} className="mr-2" />
            Generate Form 8
          </StyledSectionAddButton>

          {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && !isFileFinalStatus && (
            <>
              <EditButton
                title="Edit form 8"
                data-testId={`form8[${form8Index}].edit-form8`}
                onClick={() => history.push(`${match.url}/${form8.id}`)}
                style={{ float: 'right' }}
              />
              <RemoveIconButton
                title="Delete Form 8"
                data-testId={`form8[${form8Index}].delete-form8`}
                onRemove={() => {
                  setModalContent({
                    ...getDeleteModalProps(),
                    title: 'Remove Form 8',
                    message: 'Do you wish to remove this Form 8?',
                    okButtonText: 'Remove',
                    handleOk: async () => {
                      form8?.id && onDelete(form8.id);
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
        </StyledSubHeader>

        <SectionField
          label="Payee"
          labelWidth={{ xs: 4 }}
          valueTestId={`form8[${form8Index}].payee-name`}
        >
          <StyledPayeeDisplayName>
            {form8?.acquisitionOwnerId && expropriationPayeeOwner && (
              <label>{expropriationPayeeOwner.ownerName ?? ''}</label>
            )}

            {form8?.interestHolderId && interestHolderContactLink && interestHolderDisplayName && (
              <StyledLink
                target="_blank"
                rel="noopener noreferrer"
                to={`/contact/${interestHolderContactLink}`}
              >
                <span>{interestHolderDisplayName}</span>
                <FaExternalLinkAlt className="ml-2" size="1rem" />
              </StyledLink>
            )}
          </StyledPayeeDisplayName>
        </SectionField>

        <SectionField
          label="Expropriation Authority"
          labelWidth={{ xs: 4 }}
          valueTestId={`form8[${form8Index}].exp-authority`}
        >
          <StyledLink
            target="_blank"
            rel="noopener noreferrer"
            to={`/contact/O${form8.expropriatingAuthority?.id}`}
          >
            <span>{form8?.expropriatingAuthority?.name ?? ''}</span>
            <FaExternalLinkAlt className="ml-2" size="1rem" />
          </StyledLink>
        </SectionField>

        <SectionField
          label="Description"
          labelWidth={{ xs: 4 }}
          valueTestId={`form8[${form8Index}].description`}
        >
          {form8.description}
        </SectionField>

        <H3>Payment Details</H3>
        <ExpropriationPaymentItemsTable
          paymentItems={form8.paymentItems ?? []}
        ></ExpropriationPaymentItemsTable>

        <StyledForm8Summary>
          <Row>
            <Col className="pr-0 text-right">
              <label>Total:</label>
            </Col>
            <Col
              xs="3"
              className="pl-1 text-right"
              data-testid={`form8[${form8Index}].total-amount`}
            >
              <span>{formatMoney(paymentItemsTotal ?? 0)}</span>
            </Col>
          </Row>
        </StyledForm8Summary>
      </Section>
    </StyledForm8Border>
  );
};

export default ExpropriationForm8Details;

const StyledForm8Border = styled.div`
  border: solid 0.2rem ${props => props.theme.css.iconBlueHover};
  margin-bottom: 0.5rem;
  border-radius: 0.5rem;
`;

const StyledPayeeDisplayName = styled.div`
  display: flex;
  flex-direction: row;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-bottom: 1rem;
`;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: center;
  border-bottom: solid 0.2rem ${props => props.theme.bcTokens.surfaceColorPrimaryButtonDefault};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.bcTokens.surfaceColorPrimaryButtonDefault};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;

const StyledForm8Summary = styled.div`
  font-size: 16px;
  font-weight: 600;
`;

const getInterestHolderLink = (
  interestHolder: ApiGen_Concepts_InterestHolder | null,
): string | null => {
  if (!interestHolder) {
    return null;
  }

  if (interestHolder.personId) {
    return 'P' + interestHolder.person?.id;
  }

  return 'O' + interestHolder.organization?.id;
};
