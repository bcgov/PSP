import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { RemoveIconButton } from '@/components/common/buttons/RemoveButton';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileOffer } from '@/models/api/generated/ApiGen_Concepts_DispositionFileOffer';
import { prettyFormatDate } from '@/utils/dateUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

export interface IDispositionOfferDetailsProps {
  index: number;
  dispositionOffer: ApiGen_Concepts_DispositionFileOffer;
  onDelete: (offerId: number) => void;
  dispositionFile: ApiGen_Concepts_DispositionFile;
  isFileFinalStatus: boolean;
}

const DispositionOfferDetails: React.FunctionComponent<IDispositionOfferDetailsProps> = ({
  index,
  dispositionOffer,
  onDelete,
  dispositionFile,
  isFileFinalStatus,
}) => {
  const keycloak = useKeycloakWrapper();
  const history = useHistory();
  const match = useRouteMatch();

  const { setModalContent, setDisplayModal } = useModalContext();

  const canEditDetails = () => {
    if (isFileFinalStatus) {
      return false;
    }
    return true;
  };

  return (
    <StyledOfferBorder>
      <StyledSubHeader>
        {keycloak.hasClaim(Claims.DISPOSITION_EDIT) && canEditDetails() && (
          <>
            <EditButton
              title="Edit Offer"
              data-testId={`Offer[${index}].edit-btn`}
              onClick={() => history.push(`${match.url}/offers/${dispositionOffer.id}/update`)}
            />
            <RemoveIconButton
              title="Delete Offer"
              data-testId={`Offer[${index}].delete-btn`}
              onRemove={() => {
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
            />
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
        labelWidth={{ xs: 4 }}
        tooltip="Open = Offer has been received.
        Rejected, = Offer was not responded to (due to receiving a better competing offer or the offer was just highly undesirable).
        Countered, = Offer was responded to with a counteroffer. If counteroffer is accepted, new terms should be recorded in Notes.
        Accepted= Offer was accepted as-is.
        Collapsed= Offer was cancelled or abandoned."
        valueTestId={`offer[${index}].offerStatusTypeCode`}
      >
        {dispositionOffer.dispositionOfferStatusType?.description}
      </SectionField>
      <SectionField
        label="Offer name(s)"
        labelWidth={{ xs: 4 }}
        valueTestId={`offer[${index}].offerName`}
      >
        {dispositionOffer.offerName ?? ''}
      </SectionField>
      <SectionField
        label="Offer date"
        labelWidth={{ xs: 4 }}
        valueTestId={`offer[${index}].offerDate`}
      >
        {prettyFormatDate(dispositionOffer.offerDate)}
      </SectionField>
      <SectionField
        label="Offer expiry date"
        labelWidth={{ xs: 4 }}
        valueTestId={`offer[${index}].offerExpiryDate`}
      >
        {prettyFormatDate(dispositionOffer.offerExpiryDate)}
      </SectionField>
      <SectionField
        label="Offer price ($)"
        labelWidth={{ xs: 4 }}
        valueTestId={`offer[${index}].offerPrice`}
      >
        {formatMoney(dispositionOffer.offerAmount)}
      </SectionField>
      <SectionField
        label="Comments"
        labelWidth={{ xs: 4 }}
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
