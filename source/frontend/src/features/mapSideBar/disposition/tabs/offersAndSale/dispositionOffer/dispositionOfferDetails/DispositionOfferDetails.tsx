import styled from 'styled-components';

import { SectionField } from '@/components/common/Section/SectionField';
import { Api_DispositionFileOffer } from '@/models/api/DispositionFile';
import { prettyFormatDate } from '@/utils/dateUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

export interface IDispositionOfferDetailsProps {
  index: number;
  dispositionOffer: Api_DispositionFileOffer;
}

const DispositionOfferDetails: React.FunctionComponent<IDispositionOfferDetailsProps> = ({
  index,
  dispositionOffer: offer,
}) => {
  return (
    <StyledOfferBorder>
      <SectionField
        label="Offer status"
        labelWidth="4"
        tooltip="Rejected, = Offer was not responded to (due to receiving a better competing offer or the offer was just highly undesirable).
        Countered, = Offer was responded to with a counteroffer. If counteroffer is accepted, new terms should be recorded in Notes.
        Accepted= Offer was accepted as-is.
        Collapsed= Offer was cancelled or abandoned."
        valueTestId={`offer[${index}].offerStatusTypeCode`}
      >
        {offer.dispositionOfferStatusType?.description}
      </SectionField>
      <SectionField label="Offer name(s)" labelWidth="4" valueTestId={`offer[${index}].offerName`}>
        {offer.offerName ?? ''}
      </SectionField>
      <SectionField label="Offer date" labelWidth="4" valueTestId={`offer[${index}].offerDate`}>
        {prettyFormatDate(offer.offerDate)}
      </SectionField>
      <SectionField
        label="Offer expiry date"
        labelWidth="4"
        valueTestId={`offer[${index}].offerExpiryDate`}
      >
        {prettyFormatDate(offer.offerExpiryDate)}
      </SectionField>
      <SectionField
        label="Offer price ($)"
        labelWidth="4"
        valueTestId={`offer[${index}].offerPrice`}
      >
        {formatMoney(offer.offerAmount)}
      </SectionField>
      <SectionField
        label="Notes"
        labelWidth="4"
        tooltip="Provide any additional details such as offer terms or conditions, and any commentary on why the offer was accepted/countered/rejected."
        valueTestId={`offer[${index}].notes`}
      >
        {offer.offerNote ?? ''}
      </SectionField>
    </StyledOfferBorder>
  );
};

export default DispositionOfferDetails;

const StyledOfferBorder = styled.div`
  border: solid 0.2rem ${props => props.theme.css.discardedColor};
  margin-bottom: 0.5rem;
`;
