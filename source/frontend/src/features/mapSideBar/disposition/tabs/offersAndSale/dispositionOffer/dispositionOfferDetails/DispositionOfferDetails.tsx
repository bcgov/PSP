import { FaTrash } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { StyledRemoveLinkButton } from '@/components/common/buttons/RemoveButton';
import EditButton from '@/components/common/EditButton';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims, Roles } from '@/constants';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';
import { prettyFormatDate } from '@/utils/dateUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

import DispositionStatusUpdateSolver from '../../../fileDetails/detail/DispositionStatusUpdateSolver';

export interface IDispositionOfferDetailsProps {
  index: number;
  dispositionOffer: ApiGen_Concepts_DispositionFileOffer;
  onDelete: (offerId: number) => void;
  dispositionFile: ApiGen_Concepts_DispositionFile;
}

const DispositionOfferDetails: React.FunctionComponent<IDispositionOfferDetailsProps> = ({
  index,
  dispositionOffer,
  onDelete,
  dispositionFile,
}) => {
  const keycloak = useKeycloakWrapper();
  const history = useHistory();
  const match = useRouteMatch();

  const { setModalContent, setDisplayModal } = useModalContext();

  const statusSolver = new DispositionStatusUpdateSolver(dispositionFile);

  const canEditDetails = () => {
    if (keycloak.hasRole(Roles.SYSTEM_ADMINISTRATOR) || statusSolver.canEditOfferSalesValues()) {
      return true;
    }
    return false;
  };

  return (
    <StyledOfferBorder>
      <StyledSubHeader>
        {keycloak.hasClaim(Claims.DISPOSITION_EDIT) && canEditDetails() && (
          <>
            <EditButton
              title="Edit Offer"
              dataTestId={`Offer[${index}].edit-btn`}
              onClick={() => history.push(`${match.url}/offers/${dispositionOffer.id}/update`)}
            />
            <StyledRemoveLinkButton
              title="Delete Offer"
              data-testid={`Offer[${index}].delete-btn`}
              variant="light"
              onClick={() => {
                setModalContent({
                  ...getDeleteModalProps(),
                  variant: 'error',
                  title: 'Delete Offer',
                  message: `You have selected to delete this offer.
                  Do you want to proceed?`,
                  okButtonText: 'Yes',
                  cancelButtonText: 'No',
                  handleOk: async () => {
                    dispositionOffer?.id && onDelete(dispositionOffer.id);
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
        {keycloak.hasClaim(Claims.DISPOSITION_EDIT) && !canEditDetails() && (
          <TooltipIcon
            toolTipId={`${dispositionFile?.id || 0}-summary-cannot-edit-tooltip`}
            toolTip={cannotEditMessage}
          />
        )}
      </StyledSubHeader>
      <SectionField
        label="Offer status"
        labelWidth="4"
        tooltip="Open = Offer has been received.
        Rejected, = Offer was not responded to (due to receiving a better competing offer or the offer was just highly undesirable).
        Countered, = Offer was responded to with a counteroffer. If counteroffer is accepted, new terms should be recorded in Notes.
        Accepted= Offer was accepted as-is.
        Collapsed= Offer was cancelled or abandoned."
        valueTestId={`offer[${index}].offerStatusTypeCode`}
      >
        {dispositionOffer.dispositionOfferStatusType?.description}
      </SectionField>
      <SectionField label="Offer name(s)" labelWidth="4" valueTestId={`offer[${index}].offerName`}>
        {dispositionOffer.offerName ?? ''}
      </SectionField>
      <SectionField label="Offer date" labelWidth="4" valueTestId={`offer[${index}].offerDate`}>
        {prettyFormatDate(dispositionOffer.offerDate)}
      </SectionField>
      <SectionField
        label="Offer expiry date"
        labelWidth="4"
        valueTestId={`offer[${index}].offerExpiryDate`}
      >
        {prettyFormatDate(dispositionOffer.offerExpiryDate)}
      </SectionField>
      <SectionField
        label="Offer price ($)"
        labelWidth="4"
        valueTestId={`offer[${index}].offerPrice`}
      >
        {formatMoney(dispositionOffer.offerAmount)}
      </SectionField>
      <SectionField
        label="Notes"
        labelWidth="4"
        tooltip="Provide any additional details such as offer terms or conditions, and any commentary on why the offer was accepted/countered/rejected"
        valueTestId={`offer[${index}].notes`}
      >
        {dispositionOffer.offerNote}
      </SectionField>
    </StyledOfferBorder>
  );
};

export default DispositionOfferDetails;

const StyledOfferBorder = styled.div`
  border: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  border-radius: 0.5rem;
`;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: center;
  border-bottom: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.headerTextColor};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;
