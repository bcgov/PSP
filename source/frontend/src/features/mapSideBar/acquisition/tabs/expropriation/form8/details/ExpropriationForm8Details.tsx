import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt, FaTrash } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import EditButton from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { H3 } from '@/components/common/styles';
import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import { DetailAcquisitionFileOwner } from '@/features/mapSideBar/acquisition/models/DetailAcquisitionFileOwner';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { formatMoney } from '@/utils/numberFormatUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

import ExpropriationPaymentItemsTable from './ExpropriationPaymentItemsTable';

export interface IExpropriationForm8DetailsProps {
  form8Index: number;
  form8: Api_ExpropriationPayment;
  onDelete: (form8Id: number) => void;
}

export const ExpropriationForm8Details: React.FunctionComponent<
  IExpropriationForm8DetailsProps
> = ({ form8, form8Index, onDelete }) => {
  const history = useHistory();
  const match = useRouteMatch();
  const { setModalContent, setDisplayModal } = useModalContext();

  const expropriationPayeeOwner =
    form8.acquisitionOwnerId && form8.acquisitionOwner
      ? DetailAcquisitionFileOwner.fromApi(form8.acquisitionOwner)
      : null;

  const interestHolderContactLink = getInterestHolderLink(form8.interestHolder);
  const interestHolderDisplayName = getInterestHolderDisplayName(form8.interestHolder);
  const paymentItemsTotal =
    form8.paymentItems?.map(x => x.totalAmount ?? 0).reduce((prev, next) => prev + next, 0) ?? 0;

  return (
    <StyledForm8Border>
      <Section isCollapsable initiallyExpanded>
        <StyledSubHeader>
          <EditButton
            title="Edit form 8"
            onClick={() => history.push(`${match.url}/${form8.id}`)}
          />
          <StyledRemoveLinkButton
            title="Delete Payment Item"
            data-testid={`paymentItems[${form8Index}].delete-button`}
            variant="light"
            onClick={() => {
              setModalContent({
                ...getDeleteModalProps(),
                handleOk: async () => {
                  onDelete(form8.id!);
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
        </StyledSubHeader>

        <SectionField label="Payee" labelWidth="4">
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
          labelWidth="4"
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
          labelWidth="4"
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
            <Col xs="3" className="pl-1 text-right">
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
  border: solid 0.2rem ${props => props.theme.css.discardedColor};
  margin-bottom: 0.5rem;
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
  border-bottom: solid 0.2rem ${props => props.theme.css.primaryColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.primaryColor};
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

const getInterestHolderLink = (interestHolder: Api_InterestHolder | null): string | null => {
  if (!interestHolder) {
    return null;
  }

  if (interestHolder.personId) {
    return 'P' + interestHolder.person?.id;
  }

  return 'O' + interestHolder.organization?.id;
};

const getInterestHolderDisplayName = (interestHolder: Api_InterestHolder | null): string | null => {
  if (!interestHolder) {
    return null;
  }

  if (interestHolder.personId && interestHolder.person) {
    return formatApiPersonNames(interestHolder.person);
  }

  return interestHolder.organization?.name ?? '';
};
